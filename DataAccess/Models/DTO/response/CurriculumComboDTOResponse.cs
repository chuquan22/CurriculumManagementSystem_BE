using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class CurriculumComboDTOResponse
    {
        public int combo_id { get; set; }
        public string combo_name { get; set; }
        public bool is_active { get; set; }
    }
}
