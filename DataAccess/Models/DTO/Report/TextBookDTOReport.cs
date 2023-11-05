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
        public List<TextBookReport> textBookReports { get; set; }
    }
}
