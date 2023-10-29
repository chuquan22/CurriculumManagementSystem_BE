﻿using BusinessObject;
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

        public List<Batch> GetBatchBySpe(int speId)
        {
            return _batchDAO.GetBatchBySpe(speId);
        }

        public int GetBatchIDByName(string batchName)
        {
            return _batchDAO.GetBatchIDByName(batchName);
        }
    }
}
