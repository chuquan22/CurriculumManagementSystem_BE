using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class ComboSpeResponse
    {
        public string specialization_name { get; set; }
        public string specializataion_english_name { get; set; }
        public List<ComboResponse> combo_response { get; set; }
    }
}
