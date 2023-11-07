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
        public string learning_method_onl_name { get; set; }
        public int total_subject_onl { get; set; }
        public double ratio_onl { get; set; }
        public string learning_method_blended_name { get; set; }
        public int total_subject_blended { get; set; }
        public double ratio_blended { get; set; }
        public string learning_method_traditional_name { get; set; }
        public int total_subject_traditional { get; set; }
        public double ratio_traditional { get; set; }
    }
}
