using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace Repositories.SemesterPlans
{
    public interface ISemesterPlanRepository
    {
        public string CreateSemesterPlan(SemesterPlan semesterPlans);
    }
}
