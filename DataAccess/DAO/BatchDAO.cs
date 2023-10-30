using BusinessObject;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
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

        public List<Batch> PaginationBatch(int page, int limit, string? txtSearch)
        {
            IQueryable<Batch> query = _context.Batch;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.batch_name.Contains(txtSearch));
            }

            var listBatch = query
                .Skip((page - 1) * limit)
                .Take(limit)
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

        public Batch GetBatchById(int id)
        {
            var batch = _context.Batch.Where(x => x.batch_id == id).FirstOrDefault();
            return batch;
        }

        public bool CheckBatchDuplicate(string batch_name)
        {
            return (_context.Batch?.Any(x => x.batch_name.Equals(batch_name))).GetValueOrDefault();
        }

        public List<Batch> GetBatchBySpe(int speId)
        {
            var specialization = _context.Specialization.Include(x => x.Semester.Batch).FirstOrDefault(x => x.specialization_id == speId);
            var batch_name = specialization.Semester.Batch.batch_name;
            var listBatch = new List<Batch>();
            foreach(var batch in GetAllBatch())
            {
                if(double.Parse(batch.batch_name) >= double.Parse(batch_name))
                {
                    listBatch.Add(batch);
                }
            }
            return listBatch;
        }

        public string CreateBatch(Batch batch)
        {
            try
            {
                _context.Batch.Add(batch);
                _context.SaveChanges();
                return Result.createSuccessfull.ToString();
            }catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteBatch(Batch batch)
        {
            try
            {
                _context.Batch.Remove(batch);
                _context.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

    }
}
