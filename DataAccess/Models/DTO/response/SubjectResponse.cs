using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace DataAccess.Models.DTO.response
{
    public class SubjectResponse
    {
        public int subject_id { get; set; }
        public string subject_code { get; set; }
        public string subject_name { get; set; }
        public string english_subject_name { get; set; }
        public int learning_method_id { get; set; }
        public string learning_method_name { get; set; }
        public int assessment_method_id { get; set; }
        public string assessment_method_name { get; set; }
        public int credit { get; set; }
        public int total_time { get; set; }
        public int total_time_class { get; set; }
        public int exam_total { get; set; }
        public bool is_active { get; set; }
        public List<PreRequisiteResponse> prerequisites { get; set; }
    }


}
