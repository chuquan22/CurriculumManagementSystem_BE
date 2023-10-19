using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Excel
{
    public class CurriculumSubjectExcel
    {
        [DisplayName("SubjectCode")]
        public string subject_code { get; set; }
        [DisplayName("SubjectName")]
        public string subject_name { get; set; }
        [DisplayName("English SubjectName")]
        public string english_subject_name { get; set; }
        [DisplayName("TermNo")]
        public int term_no { get; set; }
        [DisplayName("Credits")]
        public int credit { get; set; }
        [DisplayName("Options")]
        public string option { get; set; }

    }
}
