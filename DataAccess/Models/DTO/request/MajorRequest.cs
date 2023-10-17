using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class MajorRequest
    {
        public string major_code { get; set; }
        public string major_name { get; set; }
        public string major_english_name { get; set; }
        public bool is_active { get; set; }
    }
}
