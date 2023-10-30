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
        public List<SemesterBatch> CreateSemesterBatch(SemesterBatch semesterBatchs)
        {
            return db.CreateSemesterBatch(semesterBatchs);
        }

        public List<SemesterBatch> GetSemesterBatch(int semesterId, string degree_level)
        {
            return db.GetSemesterBatch(semesterId, degree_level);
        }

        public string UpdateSemesterBatch(SemesterBatch semesterBatch)
        {
            return db.UpdateSemesterBatch(semesterBatch);
        }
    }
}
