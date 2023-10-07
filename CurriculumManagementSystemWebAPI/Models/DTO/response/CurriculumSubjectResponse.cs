using System.ComponentModel.DataAnnotations.Schema;

namespace CurriculumManagementSystemWebAPI.Models.DTO.response
{
    public class CurriculumSubjectResponse
    {
        public string subject_code { get; set; }
        public string subject_name { get; set; }
        public string curriculum_code { get; set; }
        public string curriculum_name { get; set; }
        public int term_no { get; set; }
    }
}
