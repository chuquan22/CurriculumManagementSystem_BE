using BusinessObject;
using DataAccess.DAO;
using DataAccess.Models.DTO.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SemesterPlans
{
    public class SemesterPlanRepository : ISemesterPlanRepository
    {
        public SemesterPlanDAO db = new SemesterPlanDAO();
        public string CreateSemesterPlan(SemesterPlan semesterPlans)
        {
            return db.CreateSemesterPlan(semesterPlans);
        }

        public List<SemesterPlanDTO> GetSemesterPlan(int semester_id)
        {
            return db.GetSemesterPlan(semester_id);
        }

        public SemesterPlanDetailsResponse GetSemesterPlanDetails(int semester_id)
        {
            return db.GetSemesterPlanDetails(semester_id);
        }

        public List<CreateSemesterPlanResponse> GetSemesterPlanOverView(int semester_id)
        {
            return db.GetSemesterPlanOverView(semester_id);
        }
    }
}
