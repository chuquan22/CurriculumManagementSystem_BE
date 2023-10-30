using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DataAccess.Models.DTO.request
{
    public class CurriculumSubjectRequest
    {
        [Required]
        public int subject_id { get; set; }
        [Required]
        public int curriculum_id { get; set; }
        [Required]
        public int term_no { get; set; }
        [Required]
        public string subject_group { get; set; }
        [AllowNull]
        public int? combo_id { get; set; }
        [Required]
        public bool option { get; set; }
    }
}
