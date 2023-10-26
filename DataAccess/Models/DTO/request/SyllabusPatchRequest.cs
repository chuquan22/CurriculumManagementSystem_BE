using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SyllabusPatchRequest
    {
        public int? syllabus_id { get; set; }
        public string? syllabus_description { get; set; }
        public string? degree_level { get; set; }

        public string? syllabus_tool { get; set; }

    }
}
