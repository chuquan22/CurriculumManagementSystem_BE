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
        [DisplayName("Method")]
        public string learning_method { get; set; }
        [DisplayName("Credit")]
        public string credit { get; set; }
    }
}
