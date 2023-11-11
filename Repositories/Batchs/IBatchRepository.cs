﻿using BusinessObject;
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
        Batch GetBatchById(int id);
        bool CheckBatchDuplicate(string batch_name);
        bool CheckBatchUpdateDuplicate(int id, string batch_name);
        bool CheckBatchExsit(int id);
        string CreateBatch(Batch batch);
        string UpdateBatch(Batch batch);
        string DeleteBatch(Batch batch);
    }
}
