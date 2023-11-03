using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SemesterBatchs
{
    public interface ISemesterBatchRepository
    {
        public List<SemesterPlanBatch> CreateSemesterBatch(SemesterPlanBatch semesterBatchs);

        public List<SemesterPlanBatch> GetSemesterBatch(int semesterId, int degree_level);

        public string UpdateSemesterBatch(SemesterPlanBatch semesterBatch);
    }
}
