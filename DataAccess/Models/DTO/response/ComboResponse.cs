using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class ComboResponse
    {
        public string combo_id { get; set; }

        public string combo_code { get; set; }
        public string combo_name { get; set; }
        public string combo_english_name { get; set; }
        public string combo_description { get; set; }
        public int specialization_id { get; set; }
        public bool is_active { get; set; }
    }
}
