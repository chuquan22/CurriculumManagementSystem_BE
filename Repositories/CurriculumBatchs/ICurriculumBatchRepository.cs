using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CurriculumBatchs
{
    public interface ICurriculumBatchRepository
    {
        List<CurriculumBatch> GetAllCurriculumBatch();
        List<CurriculumBatch> GetCurriculumBatchByBatchId(int batchId);

        bool CheckCurriculumBatchDuplicate(int curriId, int batchId);
        string CreateCurriculumBatch(CurriculumBatch curriculumBatch);
        string UpdateCurriculumBatch(CurriculumBatch curriculumBatch);
        string DeleteCurriculumBatch(CurriculumBatch curriculumBatch);

    }
}
