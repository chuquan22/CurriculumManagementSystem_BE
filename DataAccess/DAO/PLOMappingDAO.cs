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
    public class PLOMappingDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<PLOMapping> GetPLOMappingsInCurriculum(int curriculumId)
        {
            var plo = _context.PLOs.Where(x => x.curriculum_id == curriculumId).ToList();
            var subject = _context.CurriculumSubject
                .Where(x => x.curriculum_id == curriculumId)
                .Join(_context.Subject,
                    x => x.subject_id,
                    y => y.subject_id,
                    (x, y) => y)
                .ToList();

            var listPloMapping = new List<PLOMapping>();
            
            foreach (var s in subject)
            {
                foreach (var p in plo)
                {
                    var ploMapping = GetPLOMappingExsit(s.subject_id, p.PLO_id);
                    if(ploMapping != null)
                    {
                        listPloMapping.Add(ploMapping);
                    }
                }
            }

            return listPloMapping;
        }

        public PLOMapping GetPLOMappingExsit(int subjectId, int ploId)
        {
            return _context.PLOMapping?.Include(x => x.Subject).Include(x => x.PLOs).FirstOrDefault(e => e.subject_id == subjectId && e.PLO_id == ploId);
        }

        public bool CheckPLOMappingExsit(int subjectId, int ploId)
        {
            return (_context.PLOMapping?.Include(x => x.Subject).Include(x => x.PLOs).Any(e => e.subject_id == subjectId && e.PLO_id == ploId)).GetValueOrDefault();
        }

        public string CreatePLOMapping(PLOMapping ploMapping)
        {
            try
            {
                _context.PLOMapping.Add(ploMapping);
                _context.SaveChanges();
                return Result.createSuccessfull.ToString();
            }catch (DbUpdateConcurrencyException ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdatePLOMapping(PLOMapping ploMapping)
        {
            try
            {
                _context.PLOMapping.Update(ploMapping);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeletePLOMapping(PLOMapping ploMapping)
        {
            try
            {
                _context.PLOMapping.Remove(ploMapping);
                _context.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.InnerException.Message;
            }
        }


    }
}
