using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SyllabusResponse
    {
        public int syllabus_id { get; set; }
        public string syllabus_name { get; set; }
        public string subject_code { get; set; }
        public string subject_name { get; set; }
        public string decision_No { get; set; }
        public bool syllabus_status { get; set; }
        public bool syllabus_approved { get; set; }
        public DateTime approved_date { get; set; }

    }
}
