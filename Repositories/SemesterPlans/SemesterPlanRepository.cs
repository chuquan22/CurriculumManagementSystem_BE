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

        public List<SemesterPlanResponse> GetSemesterPlan(int semesterId, string degree_level)
        {
            return db.GetAllSemesterPlan(semesterId, degree_level);
        }

        public List<SemesterPlanDetailsResponse> GetSemesterPlanDetails(int semesterId, string degree_level)
        {
            return db.GetSemesterPlanDetails(semesterId, degree_level);
        }
    }
}
