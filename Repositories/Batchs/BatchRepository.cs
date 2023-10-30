using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Batchs
{
    public class BatchRepository : IBatchRepository
    {
        private readonly BatchDAO _batchDAO = new BatchDAO();

        public bool CheckBatchDuplicate(string batch_name)
        {
            return _batchDAO.CheckBatchDuplicate(batch_name);
        }

        public string CreateBatch(Batch batch)
        {
            return _batchDAO.CreateBatch(batch);
        }

        public string DeleteBatch(Batch batch)
        {
            return _batchDAO.DeleteBatch(batch);
        }

        public List<Batch> GetAllBatch()
        {
            return _batchDAO.GetAllBatch(); 
        }

        public Batch GetBatchById(int id)
        {
            return _batchDAO.GetBatchById(id);
        }

        public List<Batch> GetBatchBySpe(int speId)
        {
            return _batchDAO.GetBatchBySpe(speId);
        }

        public int GetBatchIDByName(string batchName)
        {
            return _batchDAO.GetBatchIDByName(batchName);
        }

        public List<Batch> PaginationBatch(int page, int limit, string? txtSearch)
        {
            return _batchDAO.PaginationBatch(page, limit, txtSearch);
        }
    }
}
