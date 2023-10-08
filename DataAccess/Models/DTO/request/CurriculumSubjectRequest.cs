using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
