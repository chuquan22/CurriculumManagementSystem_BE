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
        public string decisionNo { get; set; }
        public int isActive { get; set; }
        public DateTime isApproved { get; set; }

    }
}
