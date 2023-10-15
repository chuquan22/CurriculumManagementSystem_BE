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
    public class PLOsDTORequest
    {
        [Required]
        public string PLO_name { get; set; }
        [Required]
        public int curriculum_id { get; set; }
        [AllowNull]
        public string? PLO_description { get; set; }
    }
}
