using BusinessObject;
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

        public int GetBatchIDByName(string batchName)
        {
            
            var batch = _context.Batch.Where(x => x.batch_name.Equals(batchName)).FirstOrDefault();
            var batch_id = 0;
            if(batch != null)
            {
                batch_id = batch.batch_id;
            }
           
            return batch_id;
        }

        private int CreateBatch(Batch batch)
        {
            _context.Batch.Add(batch);
            _context.SaveChanges();
            return batch.batch_id;
        }

    }
}
