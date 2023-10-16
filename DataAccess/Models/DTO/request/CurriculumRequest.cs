using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DataAccess.Models.DTO.request
{
    public class CurriculumRequest
    {
        [Required]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter Code between 2 to 10 character")]
        public string curriculum_code { get; set; }
        [Required]
        public string curriculum_name { get; set; }
        [Required]
        public string english_curriculum_name { get; set; }
        [Required]
        public string curriculum_description { get; set; }
        [Required]
        public int specialization_id { get; set; }
        [Required]
        public int batch_id { get; set; }
        [Required]
        public string decision_No { get; set; }
        [Required]
        public string decision_Link { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime approved_date { get; set; }
        [Required]
        public int curriculum_status { get; set; }
        
    }
}
