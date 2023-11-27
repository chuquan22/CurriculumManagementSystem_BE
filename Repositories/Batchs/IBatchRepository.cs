using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Batchs
{
    public interface IBatchRepository
    {
        List<Batch> GetAllBatch();
        int GetBatchIDByName(string batchName);
        List<Batch> PaginationCurriculumBatch(int page, int limit, string? txtSearch);
        int GetTotalCurriculumBatch(string? txtSearch);
        List<Batch> GetBatchBySpe(int speId);
        List<Batch> GetBatchByDegreeLevel(int degreeLevelId);
        List<Batch> GetBatchNotExsitInSemester();
        Batch GetBatchById(int id);
        bool CheckBatchDuplicate(string batch_name, int batchOrder, int degree_Id);
        bool CheckBatchUpdateDuplicate(int id, int batchOrder, int degree_Id);
        bool CheckBatchNameExsit(string batchName, int degreeId);
        bool CheckBatchExsit(int id);
        string CreateBatch(Batch batch);
        string UpdateBatch(Batch batch);
        string DeleteBatch(Batch batch);
    }
}
