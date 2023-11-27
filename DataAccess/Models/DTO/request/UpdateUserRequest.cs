using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class UpdateUserRequest
    {
        public string user_name { get; set; }
        public string full_name { get; set; }
    }
}
