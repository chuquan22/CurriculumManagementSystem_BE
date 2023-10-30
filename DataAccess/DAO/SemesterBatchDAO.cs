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
        public List<SemesterBatch> CreateSemesterBatch(SemesterBatch se)
        {
            var semester = _context.Semester.FirstOrDefault(x => x.semester_id == se.semester_id);

            if (semester == null)
            {
                return null;
            }

            var listBatch = _context.Batch.Where(x => x.batch_id >= semester.batch_id).ToList();

            List<SemesterBatch> list = new List<SemesterBatch>();

            foreach (var item in listBatch)
            {
                var newSemesterBatch = new SemesterBatch
                {
                    semester_id = se.semester_id,
                    batch_id = item.batch_id,
                    degree_level = se.degree_level,
                    term_no = null
                };

                list.Add(newSemesterBatch);
                _context.SemesterBatch.Add(newSemesterBatch);
            }

            _context.SaveChanges();
            return list;
        }

        public string UpdateSemesterBatch(SemesterBatch semesterBatch)
        {
           
            //Update Batch
            var oldSeBa = _context.SemesterBatch.Where(s => (s.semester_id == semesterBatch.semester_id && s.batch_id == semesterBatch.batch_id && s.degree_level.Equals(semesterBatch.degree_level))).FirstOrDefault();
            if(oldSeBa == null)
            {
                return "Semester Batch not in dataabase";

            }
            oldSeBa.term_no = semesterBatch.term_no;
            _context.SaveChanges();
            return Result.updateSuccessfull.ToString();
        }

        public List<SemesterBatch> GetSemesterBatch(int semester_id, string degree_level)
        { 
            var list = _context.SemesterBatch.Where(x => (x.semester_id == semester_id && x.degree_level.Equals(degree_level))).ToList();
            return list;
        }

        }
}
