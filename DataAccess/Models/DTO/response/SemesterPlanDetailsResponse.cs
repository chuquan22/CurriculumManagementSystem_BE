using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SemesterPlanDetailsResponse
    {
        public string semesterName { get; set; }
        public string degreeLevel { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public List<SemesterPlanDetailsTermResponse> spe { get; set; }
    }
}