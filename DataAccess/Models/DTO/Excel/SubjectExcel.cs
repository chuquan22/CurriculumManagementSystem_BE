using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniExcelLibs.Attributes;

namespace DataAccess.Models.DTO.Excel
{
    public class SubjectExcel
    {
        public int No { get; set; }
        public string subject_code { get; set; }
        public string subject_name { get; set; }
        public string credit { get; set; }
        public string total_time { get; set; }
        public string subject_code_2 { get; set; }
        public string subject_name_2 { get; set; }
        public string credit_2 { get; set; }
        public string total_time_2 { get; set; }
        public string subject_code_3 { get; set; }
        public string subject_name_3 { get; set; }
        public string credit_3 { get; set; }
        public string total_time_3 { get; set; }
    }
}
