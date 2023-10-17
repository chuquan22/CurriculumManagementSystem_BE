﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class BatchDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

       public List<Batch> GetAllBatch()
        {
            var listBatch = _context.Batch
                .OrderByDescending(x => x.batch_name)
                .ToList();
            return listBatch;
        }


    }
}
