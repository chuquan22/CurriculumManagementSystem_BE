using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class CurriculumUpdateRequest
    {
        [Required]
        public string curriculum_name { get; set; }
        [Required]
        public string english_curriculum_name { get; set; }
        [Required]
        public string curriculum_description { get; set; }
        [Required]
        public string decision_No { get; set; }
        [Required]
        public bool is_active { get; set; }
    }
}
