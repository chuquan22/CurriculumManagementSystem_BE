using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SpecializationResponse
    {
        public int specialization_id { get; set; }
        public string specialization_code { get; set; }
        public string specialization_name { get; set; }
        public string specialization_english_name { get; set; }
    }
}
