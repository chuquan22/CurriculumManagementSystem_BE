using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class UserLoginResponse
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }
        public string? user_address { get; set; }
        public int user_phone { get; set; }
        public string full_name { get; set; }
        public int role_id { get; set; }
        public bool is_active { get; set; }
    }
}
