using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SubjectRequest
    {
        [Required]
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
