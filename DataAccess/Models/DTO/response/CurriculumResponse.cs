using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DataAccess.Models.DTO.response
{
    public class CurriculumResponse
    {
        public int curriculum_id { get; set; }
        public string curriculum_code { get; set; }
        public string curriculum_name { get; set; }
        public string english_curriculum_name { get; set; }
        public string curriculum_description { get; set; }
        public string specialization_name { get; set; }
        public string batch_name { get; set; }
        public string decision_No { get; set; }
        public int total_credit { get; set; }
        public DateTime approved_date { get; set; }
        public bool is_active { get; set; }

    }
}
