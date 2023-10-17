using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class CLOsEditRequest
    {
        public int CLO_id { get; set; }

        public string CLO_name { get; set; }
        public int syllabus_id { get; set; }
        public string CLO_description { get; set; }
    }
}
