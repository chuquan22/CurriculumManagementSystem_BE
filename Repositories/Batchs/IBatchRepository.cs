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
        List<Batch> GetBatchBySpe(int speId);

        string CreateBatch(Batch batch);
        string DeleteBatch(Batch batch);
    }
}
