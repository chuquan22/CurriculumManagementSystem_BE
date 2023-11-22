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
        public int specialization_id { get; set; }
        public int start_batch_id { get; set; }
        public string specialization_name { get; set; }
        public string batch_name { get; set; }
        public string decision_No { get; set; }
        public int total_credit { get; set; }
        public int total_semester { get; set; }
        public string degree_level { get; set; }
        public string Formality { get; set; }
        public string Semester { get; set; }
        public string vocational_code { get; set; }
        public string vocational_name { get; set; }
        public string vocational_english_name { get; set; }
        public DateTime approved_date { get; set; }
    }
}
