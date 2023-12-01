using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.DTO.response
{
    public class CurriculumSubjectResponse
    {
        public int curriculum_id { get; set; }
        public int subject_id { get; set; }
        public string subject_code { get; set; }
        public string subject_name { get; set; }
        public int credit { get; set; }
        public int total_time { get; set; }
        public int combo_id { get; set; }
        public string combo_name { get; set; }
        public int specialization_id { get; set; }
        public int term_no { get; set; }
        public int? option { get; set; }
        
    }
}
