using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SubjectResponse
    {
        public int subject_id { get; set; }
        [Required]
        public string subject_code { get; set; }
        [Required]
        public string subject_name { get; set; }
        [Required]
        public string english_subject_name { get; set; }
        [Required]
        public string learning_method_name { get; set; }
        [Required]
        public string assessment_method_name { get; set; }
        [Required]
        public int credit { get; set; }
        [Required]
        public int total_time { get; set; }
        [Required]
        public int total_time_class { get; set; }
        [Required]
        public int exam_total { get; set; }
        [Required]
        public bool is_active { get; set; }
    }


}
