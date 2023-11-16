using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Report
{
    public class TextBookDTOReport
    {
        public string batch_name { get; set; }
        public string major_name { get; set; }
        public string learning_resource_T01_name { get; set; }
        public string learning_resource_T02_name { get; set; }
        public string learning_resource_T03_name { get; set; }
        public string learning_resource_T04_name { get; set; }
        public string learning_resource_T05_name { get; set; }
        public string learning_resource_T06_name { get; set; }
        public string learning_resource_T07_name { get; set; }
        public List<TextBookReport> textBookReports { get; set; }
    }
}
