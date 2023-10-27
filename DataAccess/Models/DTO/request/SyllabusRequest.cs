using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SyllabusRequest
    {
        public string document_type { get; set; }
        public string program { get; set; }
        public string decision_No { get; set; }
        public string degree_level { get; set; }
        public string syllabus_description { get; set; }
        public int subject_id { get; set; }
        public string student_task { get; set; }
        public string? time_allocation { get; set; }

        public string? syllabus_tool { get; set; }
        public string? syllabus_note { get; set; }
        
        public decimal min_GPA_to_pass { get; set; }
        public int scoring_scale { get; set; }
        public DateTime? approved_date { get; set; }
        public bool syllabus_status { get; set; }
        public bool syllabus_approved { get; set; }
    }
}
