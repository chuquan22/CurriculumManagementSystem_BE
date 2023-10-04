using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class UserLoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
