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
        List<Batch> PaginationBatch(int page, int limit, string? txtSearch);
        int GetBatchIDByName(string batchName);
        int GetTotalBatch(string? txtSearch);
        List<Batch> GetBatchBySpe(int speId);
        Batch GetBatchById(int id);
        bool CheckBatchDuplicate(string batch_name);
        bool CheckBatchExsit(int id);
        string CreateBatch(Batch batch);
        string DeleteBatch(Batch batch);
    }
}
