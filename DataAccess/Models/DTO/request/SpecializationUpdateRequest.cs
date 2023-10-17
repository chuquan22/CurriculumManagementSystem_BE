using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SpecializationUpdateRequest
    {
        public int specialization_id { get; set; }

        public string specialization_code { get; set; }
  
        public string specialization_name { get; set; }
        public string specialization_english_name { get; set; }
        public int major_id { get; set; }
        public int semester_id { get; set; }
        public bool is_active { get; set; }
    }
}
