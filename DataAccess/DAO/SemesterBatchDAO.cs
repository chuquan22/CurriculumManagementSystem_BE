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
    public class SemesterBatchDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();
        public List<SemesterPlanBatch> CreateSemesterBatch(SemesterPlanBatch se)
        {
            var semester = _context.Semester
                .Include(x => x.Batch)
                .FirstOrDefault(x => x.semester_id == se.semester_id);

            if (semester == null)
            {
                return null;
            }

            // Fetch all batches into memory
            var batches = _context.Batch
            
                .OrderByDescending(x => x.batch_name)
                .ToList();

            List<Batch> recentBatches = new List<Batch>();

            if (double.TryParse(semester.Batch.batch_name, out double semesterBatch))
            {
                foreach (var batch in batches)
                {
                    if (double.TryParse(batch.batch_name, out double batchValue))
                    {
                        if (batchValue >= semesterBatch)
                        {
                            recentBatches.Add(batch);
                        }
                        if (recentBatches.Count == 7)
                        {
                            break;
                        }
                    }
                }
            }
            
            else
            {
                recentBatches = batches;
            }

            List<SemesterPlanBatch> list = new List<SemesterPlanBatch>();

            foreach (var item in recentBatches)
            {
                var newSemesterBatch = new SemesterPlanBatch
                {
                    semester_id = se.semester_id,
                    batch_id = item.batch_id,
                    degree_level_id = se.degree_level_id,
                    term_no = null
                };

                list.Add(newSemesterBatch);
                _context.SemesterBatch.Add(newSemesterBatch);
            }

            _context.SaveChanges();
            return list;
        }



        public string UpdateSemesterBatch(SemesterPlanBatch semesterBatch)
        {
           
            //Update Batch
            var oldSeBa = _context.SemesterBatch.Where(s => (s.semester_id == semesterBatch.semester_id && s.batch_id == semesterBatch.batch_id && s.degree_level_id == semesterBatch.degree_level_id)).FirstOrDefault();
            if(oldSeBa == null)
            {
                return "Semester Batch not in dataabase";

            }
            oldSeBa.term_no = semesterBatch.term_no;
            _context.SaveChanges();
            return Result.updateSuccessfull.ToString();
        }

        public List<SemesterPlanBatch> GetSemesterBatch(int semester_id, int degree_level_id)
        { 
            var list = _context.SemesterBatch
               
                .Include(s => s.Semester)
                .Include(s => s.Batch)
                .Where(x => (x.semester_id == semester_id && x.degree_level_id == degree_level_id)).ToList();
            return list;
        }

    }
}
