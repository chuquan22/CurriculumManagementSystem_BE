using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SemesterResponse
    {
        public string semester_name { get; set; }
        public DateTime semester_start_date { get; set; }
        public DateTime semester_end_date { get; set; }
        public int school_year { get; set; }
        public int batch_id { get; set; }
        public string batch_name { get; set; }
    }
}
