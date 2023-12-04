using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Excel
{
    public class SubjectImportExcel
    {
        [DisplayName("SubjectCode")]
        public string subject_code { get; set; }
        [DisplayName("SubjectName")]
        public string subject_name { get; set;}
        [DisplayName("English SubjectName")]
        public string english_subject_name { get; set; }
        [DisplayName("Learning Method")]
        public string learning_method { get; set; }
        [DisplayName("Credits")]
        public string credit { get; set; }
        [DisplayName("Total Time")]
        public string total_time { get; set; }
        [DisplayName("Class Time")]
        public string class_time { get; set; }
        [DisplayName("Exam Time")]
        public string exam_time { get; set; }
        [DisplayName("Assessment Method")]
        public string assessment_method { get; set; }
    }
}
