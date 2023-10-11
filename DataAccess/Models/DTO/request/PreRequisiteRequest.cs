using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DataAccess.Models.DTO.request
{
    public class PreRequisiteRequest
    {
        [Required]
        public int subject_id { get; set; }
        [Required]
        public int pre_subject_id { get; set; }
        [AllowNull]
        public string? pre_requisite_name { get; set; }
        [Required]
        public int pre_requisite_type_id { get; set; }
    }
}
