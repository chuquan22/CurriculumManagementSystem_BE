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
        public List<SemesterBatch> CreateSemesterBatch(SemesterBatch semesterBatchs);

        public List<SemesterBatch> GetSemesterBatch(int semesterId, string degree_level);

        public string UpdateSemesterBatch(SemesterBatch semesterBatch);
    }
}
