using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SemesterBatchs
{
    public class SemesterBatchRepository : ISemesterBatchRepository
    {
        public SemesterBatchDAO db = new SemesterBatchDAO();
        public List<SemesterPlanBatch> CreateSemesterBatch(SemesterPlanBatch semesterBatchs)
        {
            return db.CreateSemesterBatch(semesterBatchs);
        }

        public List<SemesterPlanBatch> GetSemesterBatch(int semesterId, int degree_level)
        {
            return db.GetSemesterBatch(semesterId, degree_level);
        }

        public string UpdateSemesterBatch(SemesterPlanBatch semesterBatch)
        {
            return db.UpdateSemesterBatch(semesterBatch);
        }
    }
}
