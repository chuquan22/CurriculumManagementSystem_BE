using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess.Models.DTO.response;

namespace Repositories.SemesterPlans
{
    public interface ISemesterPlanRepository
    {
        public string CreateSemesterPlan(SemesterPlan semesterPlans); 

        public List<SemesterPlanDTO> GetSemesterPlan(int semester_id);

        public SemesterPlanDetailsResponse GetSemesterPlanDetails(int semester_id);

        //public SemesterPlanDetailsResponse GetSemesterPlanDetails(int semesterId, string degree_level);

    }
}
