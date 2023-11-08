using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.GoogleLogin
{
    public class LoginResponse
    {
        public bool IsAdmin { get; set; }
        public bool IsFirstTime { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime? IsVerify { get; set; }
        public string? VerifyToken { get; set; }
    }
}
