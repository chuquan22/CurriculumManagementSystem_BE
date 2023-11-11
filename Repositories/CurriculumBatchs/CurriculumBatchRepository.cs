using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CurriculumBatchs
{
    public class CurriculumBatchRepository : ICurriculumBatchRepository
    {
        private readonly CurriculumBatchDAO _curriculumBatchDAO = new CurriculumBatchDAO();

        public bool CheckCurriculumBatchDuplicate(int curriId, int batchId)
        {
            return _curriculumBatchDAO.CheckCurriculumBatchDuplicate(curriId, batchId);
        }

        public string CreateCurriculumBatch(CurriculumBatch curriculumBatch)
        {
            return _curriculumBatchDAO.CreateCurriculumBatch(curriculumBatch);
        }

        public string DeleteCurriculumBatch(CurriculumBatch curriculumBatch)
        {
            return _curriculumBatchDAO.DeleteCurriculumBatch(curriculumBatch);
        }

        public List<CurriculumBatch> GetAllCurriculumBatch()
        {
            return _curriculumBatchDAO.GetAllCurriculumBatch();
        }

        public List<CurriculumBatch> GetCurriculumBatchByBatchId(int batchId)
        {
            return _curriculumBatchDAO.GetCurriculumBatchByBatchId(batchId);
        }


        public string UpdateCurriculumBatch(CurriculumBatch curriculumBatch)
        {
            return _curriculumBatchDAO.UpdateCurriculumBatch(curriculumBatch);
        }
    }
}
