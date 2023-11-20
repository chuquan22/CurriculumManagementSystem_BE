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
    public class CurriculumBatchDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<CurriculumBatch> GetAllCurriculumBatch()
        {
            var listCurriBatch = _context.CurriculumBatch
                .Include(x => x.Curriculum)
                .Include(x => x.Batch)
                .ToList();
            return listCurriBatch;
        }

        public List<CurriculumBatch> GetCurriculumBatchByBatchId(int batchId)
        {
            var curriBatch = _context.CurriculumBatch
                .Include(x => x.Curriculum)
                .Include(x => x.Batch)
                .Where(x => x.batch_id == batchId)
                .ToList();
            return curriBatch;

        }


        public bool CheckCurriculumBatchDuplicate(int curriId, int batchId)
        {
            return (_context.CurriculumBatch?.Any(x => x.curriculum_id == curriId && x.batch_id == batchId)).GetValueOrDefault();
        }

        public string CreateCurriculumBatch(CurriculumBatch curriculumBatch)
        {
            try
            {
                _context.CurriculumBatch.Add(curriculumBatch);
                _context.SaveChanges();
                return Result.createSuccessfull.ToString();
            }catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateCurriculumBatch(CurriculumBatch curriculumBatch)
        {
            try
            {
                _context.CurriculumBatch.Update(curriculumBatch);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteCurriculumBatch(CurriculumBatch curriculumBatch)
        {
            try
            {
                _context.CurriculumBatch.Remove(curriculumBatch);
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
