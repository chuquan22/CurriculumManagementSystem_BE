using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SyllabusDetailsResponse
    {
        public int syllabus_id { get; set; }
        public int decision_No { get; set; }

        public string document_type { get; set; }
        public string subject_name { get; set; }
        public string english_subject_name { get; set; }
        public string subject_code { get; set; }
        public string learning_teaching_method { get; set; }
        public string credit { get; set; }

        public int degree_level { get; set; }
        public string time_allocation { get; set; }
        public List<PreRequisiteResponse2> pre_required { get; set; }

        public string syllabus_description { get; set; }
        public int subject_id { get; set; }
        
        public string student_task { get; set; }
        public string? syllabus_tool { get; set; }
        public string? syllabus_note { get; set; }
        public decimal min_GPA_to_pass { get; set; }
        public int scoring_scale { get; set; }
        public DateTime approved_date { get; set; }
        public int syllabus_status { get; set; }

    }
}
