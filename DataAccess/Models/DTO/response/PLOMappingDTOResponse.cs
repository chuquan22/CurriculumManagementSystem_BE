using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class PLOMappingDTOResponse
    {
        public int PLO_id { get; set; }
        public string PLO_name { get; set; }
        public int subject_id { get; set; }
        public string subject_code { get; set; }
    }
}
