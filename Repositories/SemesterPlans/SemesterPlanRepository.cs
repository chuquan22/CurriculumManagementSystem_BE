using BusinessObject;
using DataAccess.DAO;
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

        public List<SemesterPlan> GetSemesterPlan(int semesterId, string degree_level)
        {
            return db.GetAllSemesterPlan(semesterId, degree_level);
        }
    }
}
