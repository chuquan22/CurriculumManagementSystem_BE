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

        public bool CheckBatchDuplicate(string batch_name, int batchOrder, int degree_Id)
        {
            return _batchDAO.CheckBatchDuplicate(batch_name, batchOrder ,degree_Id);
        }

        public bool CheckBatchExsit(int id)
        {
            return _batchDAO.CheckBatchExsit(id);
        }

        public bool CheckBatchUpdateDuplicate(int id, int batchOrder, int degree_Id)
        {
            return _batchDAO.CheckBatchUpdateDuplicate(id, batchOrder, degree_Id);
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

        public List<Batch> GetBatchByDegreeLevel(int degreeLevelId)
        {
            return _batchDAO.GetBatchByDegreeLevel(degreeLevelId);
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

        public List<Batch> GetBatchNotExsitInSemester()
        {
            return _batchDAO.GetBatchNotExsitInSemester();
        }

        public int GetTotalCurriculumBatch(string? txtSearch)
        {
            return _batchDAO.GetTotalCurriculumBatch(txtSearch);
        }

        public List<Batch> PaginationCurriculumBatch(int page, int limit, string? txtSearch)
        {
            return _batchDAO.PaginationCurriculumBatch(page, limit, txtSearch);
        }

        public string UpdateBatch(Batch batch)
        {
            return _batchDAO.UpdateBatch(batch);
        }
    }
}
