using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class UserResponse
    {
        public string user_name { get; set; }
        public string user_email { get; set; }
        public int? user_phone { get; set; }
        public string full_name { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }
        public bool is_active { get; set; }
    }
}
