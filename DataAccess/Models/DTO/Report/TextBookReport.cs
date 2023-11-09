using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Report
{
    public class TextBookReport
    {
        public string specialization_name { get; set; }
        public int total_subject { get; set; }
        public List<LearningResourceReport> LearningResource { get; set; }

    }
}
