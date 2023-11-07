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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Google.Apis.Gmail.v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration config;
        private IUsersRepository repo;
        private readonly IMapper _mapper;
        private static string ClientId = "780549906802-4k5phhf2h582rbhfc55qqn9tmi3ir24k.apps.googleusercontent.com";
        private static string ClientSecret = "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";
        private static string ApplicationName = "Web client 1";
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
       
        public static UserCredential GetUserCredential(out string error)
        {
            UserCredential credential = null;
            error = null;
            try
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = "780549906802-4k5phhf2h582rbhfc55qqn9tmi3ir24k.apps.googleusercontent.com",
                        ClientSecret = "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",
                    },
            
                    Scopes,
                    Environment.UserName,
                    CancellationToken.None,
                    new FileDataStore("Google Oauth2 Client")).Result;
                


            }
            catch (Exception ex)
            {

                credential = null;
                error = "Failed to UserCredential Intilization: " + ex.Message;
            }
            return credential; 

        }
        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult Login([FromBody] UserLoginRequest userLoginRequest)
        //{
        //    User user = AuthenticateUser(userLoginRequest.email);
     
        //    if (user != null)
        //    {
        //        UserLoginResponse userResponse = _mapper.Map<UserLoginResponse>(user);
        //        var token = GenerateToken(user);
        //        var data = new[]
        //        {
        //           new {
        //               Token = token,
        //               UserData = userResponse
        //               },
        //         };
        //        if(userResponse.is_active == false)
        //        {
        //            return BadRequest(new BaseResponse(true, "Your account has been locked for violating system policies. You can send an unlock request to the system adminstrator via email at admin-cms@fpoly.fpt.edu.vn!", null));
        //        }
        //        return Ok(new BaseResponse(false, "Login Successful", data));
        //    }
        //    return Unauthorized(new BaseResponse(false, "Your account is not allowed to log into the system!", null));
        //}

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login()
        {
            string credentialError = null;
            string refreshToken = null;
            
                UserCredential credential = GetUserCredential(out credentialError);
                if(credential != null && string.IsNullOrEmpty(credentialError))
                {
                    refreshToken = credential.Token.RefreshToken;
                }
                var data = new[]
                {
                   new {
                       Token = refreshToken,
                       UserData = credential
                       },
                 };
                
                return Ok(new BaseResponse(false, "Login Successful", data));
            
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
        [HttpGet("login-with-google")]
        [AllowAnonymous]
        public IActionResult LoginWithGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("Cookies");

            if (!result.Succeeded)
            {
                // Handle authentication failure...
                return BadRequest("Google Authentication failed.");
            }

            // Access user information from result.Principal and process it as needed.
            // Create or update a user's account in your database and sign them in.

            // Redirect or return to your application's main page.
            return Redirect("/"); // Replace with your desired URL.
        }

    }
}
