using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Report
{
    public class TKOLReport
    {
        public string specialization_name { get; set; }
        public int total_subject { get; set; }
        public string learning_method_T01_name { get; set; }
        public int total_subject_T01 { get; set; }
        public double ratio_T01 { get; set; }
        public string learning_method_T02_name { get; set; }
        public int total_subject_T02 { get; set; }
        public double ratio_T02 { get; set; }
        public string learning_method_T03_name { get; set; }
        public int total_subject_T03 { get; set; }
        public double ratio_T03 { get; set; }
    }
}
