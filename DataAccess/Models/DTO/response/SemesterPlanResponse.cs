using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SemesterPlanResponse
    {
        public string spe { get; set; }
        public int totalSemester { get; set; }
        public string semester { get; set; }
        public List<SemesterBatchResponse> batch { get; set; }
    }
}
