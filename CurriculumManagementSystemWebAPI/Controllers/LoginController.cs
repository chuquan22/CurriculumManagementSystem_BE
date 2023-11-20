using Microsoft.AspNetCore.Mvc;
using BusinessObject;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using DataAccess.Models.DTO.response;
using Repositories.Users;
using AutoMapper;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;

using Google.Apis.Auth.OAuth2.Flows;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration config;
        private IUsersRepository repo;
        private readonly IMapper _mapper;
        private string accessToken = null;
        public LoginController(IConfiguration configuration, IMapper mapper)
        {
            config = configuration;
            repo = new UsersRepository();
            _mapper = mapper;
        }
        public static string[] Scopes =
        {
            GmailService.Scope.GmailCompose,
            GmailService.Scope.GmailSend
        };
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GoogleOAuthLogin()
        {
            try
            {
                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = config["Authentication:Google:ClientId"],
                        ClientSecret = config["Authentication:Google:ClientSecret"]
                    },
                    Scopes = Scopes
                });

                var authUri = flow.CreateAuthorizationCodeRequest(config["Authentication:Google:CallBackUrl"]).Build();
                return Ok(authUri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to initiate Google OAuth login: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("CallBack")]
        public async Task<ActionResult> GoogleOAuthCallback(string? code, string? scope)
        {
            try
            {

                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = config["Authentication:Google:ClientId"],
                        ClientSecret = config["Authentication:Google:ClientSecret"]
                    },
                    Scopes = Scopes
                });
                var token = await flow.ExchangeCodeForTokenAsync("user", code, config["Authentication:Google:CallBackUrl"], CancellationToken.None);
                var credential = new UserCredential(flow, "user", token);
                var services = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });
                var userInfo = services.Users.GetProfile("me").Execute();
                string userEmail = userInfo.EmailAddress;
                if (!userEmail.EndsWith("@fpt.edu.vn"))
                {
                    return Ok(new BaseResponse(true, "To access the system, you must log in with @fpt.edu.vn account.", null));
                }
                User user = AuthenticateUser(userEmail);
                if (user == null)
                {
                    return Ok(new BaseResponse(true, "User authentication failed. Contact adminstrator cmspoly@fpt.edu.vn.", null));
                }
                if (user.is_active == false)
                {
                    return Ok(new BaseResponse(true, "Your account has been locked. Please contact the adminstrator cmspoly@fpt.edu.vn if you have questions about the locked issue.", null));
                }
                UserLoginResponse userResponse = _mapper.Map<UserLoginResponse>(user);
                var tokenJWTuser = GenerateToken(user);
                var refreshToken = GenerateRefreshToken();
                repo.SaveRefreshTokenUser(user.user_id, refreshToken);
                var data = new[]
                    {
                   new {
                       Token = tokenJWTuser,
                       RefreshToken = refreshToken,
                       UserData = userResponse
                       },
                 };
                return Ok(new BaseResponse(false, "Login Successfully!", data));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Login Google Authenticator False. Please Try Login Google Again.", null));
            }
        }
        [HttpPost("get-refresh-token")]
        [AllowAnonymous]
        public ActionResult GetRefreshToken(string refreshToken)
        {

            User user = repo.GetUserByRefreshToken(refreshToken);
            if (user == null)
            {
                return BadRequest(new BaseResponse(true, "Token refreshed not avaiable!", null));
            }
            var token = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            repo.SaveRefreshTokenUser(user.user_id, newRefreshToken);
            var data = new[]
                {
                   new {
                       Token = token,
                       RefreshToken = newRefreshToken
                       },
                 };

            return Ok(new BaseResponse(false, "Token refreshed successfully!", data));
        }

        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser == null)
                {
                    return BadRequest(new BaseResponse(true, "Logout failed. User not logged in system."));
                }
               //repo.DeleteRefreshToken(currentUser.user_id);
                return Ok(new BaseResponse(false, "Logout system successfully!", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        private User AuthenticateUser(string email)
        {
            User userLogged = repo.Login(email);
            if (userLogged == null)
            {
                return null;
            }
            return userLogged;
        }
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.user_id.ToString()),
                new Claim(ClaimTypes.Name,user.full_name),
                new Claim(ClaimTypes.Email,user.user_email),
                new Claim(ClaimTypes.Surname,user.full_name),
                new Claim(ClaimTypes.Role,user.Role.role_name),
            };

            var token = new JwtSecurityToken(config["JWT:Issuer"], config["JWT:Issuer"], claims, expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString();

            return refreshToken;
        }




        [HttpGet("get-current-user")]
        public UserLoginResponse GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                UserLoginResponse data = new UserLoginResponse
                {
                    user_id = Convert.ToInt32(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                    user_name = identity.FindFirst(ClaimTypes.Name)?.Value,
                    user_email = identity.FindFirst(ClaimTypes.Email)?.Value,
                    full_name = identity.FindFirst(ClaimTypes.Surname)?.Value,
                    role_id = Convert.ToInt32(identity.FindFirst(ClaimTypes.Role)?.Value),
                };
                return data;
            }
            return null;
        }


    }
}
