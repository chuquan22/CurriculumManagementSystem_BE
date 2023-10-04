using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class SubjectDTO
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
        public string option { get; set; }
        [Required]
        public string subject_status { get; set; }
    }

    public class SubjectCreateDTO
    {
        [Required]
        [MaxLength(10)]
        public string subject_code { get; set; }
        [Required]
        public string subject_name { get; set; }
        [Required]
        public string english_subject_name { get; set; }
        [Required]
        public int learning_method_id { get; set; }
        [Required]
        public int assessment_method_id { get; set; }
        [Required]
        public int credit { get; set; }
        [Required]
        public string option { get; set; }
        [Required]
        public string subject_status { get; set; }
    }
}
