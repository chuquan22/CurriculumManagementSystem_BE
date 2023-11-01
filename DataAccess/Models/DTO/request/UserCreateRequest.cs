using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class UserCreateRequest
    {
        [Required]
        public string user_name { get; set; }
        [Required]
        public string user_email { get; set; }
        [Required]
        public string full_name { get; set; }
        [Required]
        public int role_id { get; set; }
    }
}
