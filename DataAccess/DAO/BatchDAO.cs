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
                .ThenByDescending(x => x.batch_order)
                .ToList();
            return listBatch;
        }

        public int GetBatchIDByName(string batchName)
        {

            var batch = _context.Batch.Where(x => x.batch_name.Equals(batchName)).FirstOrDefault();
            var batch_id = 0;
            if (batch != null)
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

        public List<Batch> GetBatchByDegreeLevel(int degreeId)
        {
            var listBatch = _context.Semester.Include(x => x.Batch).Where(x => x.degree_level_id == degreeId).Select(x => x.Batch).ToList(); ;
            return listBatch;
        }


        public bool CheckBatchDuplicate(string batch_name)
        {
            return (_context.Batch?.Any(x => x.batch_name.Equals(batch_name))).GetValueOrDefault();
        }

        public bool CheckBatchUpdateDuplicate(int id, string batch_name)
        {
            return (_context.Batch?.Any(x => x.batch_name.Equals(batch_name) && x.batch_id != id)).GetValueOrDefault();
        }

        public bool CheckBatchExsit(int id)
        {
            var exsitInCurri = _context.CurriculumBatch.FirstOrDefault(x => x.batch_id == id);
            if (exsitInCurri == null)
            {
                return false;
            }
            return true;
        }

        public List<Batch> GetBatchBySpe(int speId)
        {
            var specialization = _context.Specialization
                .Include(x => x.Major)
                .Include(x => x.Semester.Batch)
                .FirstOrDefault(x => x.specialization_id == speId);
            var batch_name = specialization.Semester.Batch.batch_name;
            var listBatch = new List<Batch>();
            foreach (var batch in GetBatchByDegreeLevel(specialization.Major.degree_level_id))
            {
                double batchValue;
                if (double.TryParse(batch.batch_name, out batchValue))
                {
                    if (batchValue >= double.Parse(batch_name))
                    {
                        listBatch.Add(batch);
                    }
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
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateBatch(Batch batch)
        {
            try
            {
                _context.Batch.Update(batch);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
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
