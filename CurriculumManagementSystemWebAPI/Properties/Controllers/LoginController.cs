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

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration config;
        private IUsersRepository repo;
        private readonly IMapper _mapper;

        public LoginController(IConfiguration configuration, IMapper mapper)
        {
            config = configuration;
            repo = new UsersRepository();
            _mapper = mapper;  
        }
       
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] UserLoginRequest userLoginRequest)
        {
            User user = AuthenticateUser(userLoginRequest.email);
            if (user != null)
            {
                UserLoginResponse userResponse = _mapper.Map<UserLoginResponse>(user);
                var token = GenerateToken(user);
                var data = new[]
                {
                   new {
                       Token = token,
                       UserData = userResponse
                       },
                 };
                return Ok(new BaseResponse(false, "Login Successful", data));
            }
            return Unauthorized(new BaseResponse(false, "Login False", null));
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
