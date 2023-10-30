using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SemesterBatchRequest
    {
        public int semester_id { get; set; }
        public int batch_id { get; set; }

        public int? term_no { get; set; }
        public string degree_level { get; set; }
    }
}
