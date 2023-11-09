﻿using Microsoft.AspNetCore.Mvc;
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

        private static string ClientsId = "727708784205-5cpdj755l32h8ddrh1husncpdj7e84hk.apps.googleusercontent.com";
        private static string ClientsSecret = "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";
        private static string ApplicationName = "Web client 1";
        private static string CallBackUrl = "https://localhost:8080/api/Login/CallBack";
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
                        ClientId = ClientsId,
                        ClientSecret = ClientsSecret
                    },
                    Scopes = Scopes
                });

                var authUri = flow.CreateAuthorizationCodeRequest(CallBackUrl).Build();
                return Redirect(authUri.AbsoluteUri);
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
                        ClientId = ClientsId,
                        ClientSecret = ClientsSecret
                    },
                    Scopes = Scopes
                });

                // Exchange the authorization code for a token
                var token = await flow.ExchangeCodeForTokenAsync("user", code, CallBackUrl, CancellationToken.None);

                // Use the token to create UserCredential if needed
                var credential = new UserCredential(flow, "user", token);
                var services = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });
                var userInfo = services.Users.GetProfile("me").Execute();
                string userEmail = userInfo.EmailAddress;
                User user = AuthenticateUser(userEmail);
                if (user == null)
                {
                    Logout();
                    return Unauthorized(new BaseResponse(true, "User authentication failed."));
                }
                //string TokenUser = GenerateToken(user);
                UserLoginResponse userResponse = _mapper.Map<UserLoginResponse>(user);
                var data = new[]
                    {
                   new {
                       Token = token,
                       UserData = userResponse
                       },
                 };
                return Ok(new BaseResponse(false, "Login Successful!", data));
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to complete Google OAuth callback: " + ex.Message);
            }
        }


        public static UserCredential GetUserCredential(out string error)
        {
            UserCredential credential = null;
            error = null;
            try
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                     new ClientSecrets
                     {
                         ClientId = ClientsId,
                         ClientSecret = ClientsSecret,
                     },

                     Scopes,
                     Environment.UserName,
                     CancellationToken.None
                     ).Result;
                if (credential.Token.IsExpired(Google.Apis.Util.SystemClock.Default))
                {
                    credential.RefreshTokenAsync(CancellationToken.None).Wait();
                }
                var services = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });


            }
            catch (Exception ex)
            {
                credential = null;
                error = "Failed to UserCredential Initialization: " + ex.Message;
            }
            return credential;
        }
        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {

            string error;
            UserCredential credential = GetUserCredential(out error);
            if (credential != null)
            {
                RevokeUserCredential(credential, out error);
                if (error != null)
                {
                    return BadRequest(new BaseResponse(true, error, null));
                }
                else
                {
                    return Ok(new BaseResponse(false, "User logged out successfully!", null));

                }
            }
            else
            {
                return BadRequest(new BaseResponse(true, "User credentials are not available or could not be obtained.", null));
            }
        }

        public static void RevokeUserCredential(UserCredential credential, out string error)
        {
            error = null;
            try
            {
                credential.RevokeTokenAsync(CancellationToken.None).Wait();
            }
            catch (Exception ex)
            {
                error = "Failed to revoke the user's credentials: " + ex.Message;
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
