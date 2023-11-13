using Microsoft.AspNetCore.Mvc;
using BusinessObject;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Repositories.Users;
using AutoMapper;
using Google.Apis.Util;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;

using DataAccess.Models.DTO.GoogleLogin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;
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
        private readonly SignInManager<IdentityUser> _signInManager ;
        private static string accessToken = null;
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
                accessToken = token.AccessToken;
                if (!userEmail.EndsWith("@fpt.edu.vn"))
                {
                    return Unauthorized(new BaseResponse(true, "To access the system, you must log in with @fpt.edu.vn account."));
                }
                User user = AuthenticateUser(userEmail);
                if (user == null)
                {
                    return Unauthorized(new BaseResponse(true, "User authentication failed. Contact adminstrator cmspoly@fpt.edu.vn."));
                }
                UserLoginResponse userResponse = _mapper.Map<UserLoginResponse>(user);
                var tokenJWTuser = GenerateToken(user);
                var data = new[]
                    {
                   new {
                       Token = tokenJWTuser,
                       UserData = userResponse
                       },
                 };
                return Ok(new BaseResponse(false, "Login Successfully!", data));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "To access the system, you must log in with @fpt.edu.vn account.", null));
            }
        }

        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {

            try
            {
                if (accessToken == null)
                {
                    return BadRequest(new BaseResponse(true, "Logout failed. User not logged in system."));
                }
                var httpClient = new HttpClient();
                var revokeTokenEndpoint = $"https://oauth2.googleapis.com/revoke?token={accessToken}";

                var response = await httpClient.PostAsync(revokeTokenEndpoint, null);

                // If the response is successful, the token was revoked
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
        [HttpPost("get-token")]
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
                new Claim(ClaimTypes.Role,user.role_id.ToString()),
            };

            var token = new JwtSecurityToken(config["JWT:Issuer"], config["JWT:Issuer"], claims, expires: DateTime.Now.AddMinutes(5), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
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
