using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SemesterPlanDTO
    {
        public string speName { get; set; }
        public List<SemesterPlan> listSemester { get;set; }
    }
}
