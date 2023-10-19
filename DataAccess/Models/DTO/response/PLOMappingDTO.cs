using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class PLOMappingDTO
    {
        public int subject_id { get; set; }
        public string subject_code { get; set; }
        public Dictionary<string, bool> PLOs { get; set; }
    }
}
