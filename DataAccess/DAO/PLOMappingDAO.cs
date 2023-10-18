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

            var ploMapping = _context.PLOMapping
                .Join(plo,
                    mapping => mapping.PLO_id,
                    ploItem => ploItem.PLO_id,
                    (mapping, ploItem) => new { Mapping = mapping, PLO = ploItem })
                .Join(subject,
                    joinResult => joinResult.Mapping.subject_id,
                    subjectItem => subjectItem.subject_id,
                    (joinResult, subjectItem) => joinResult.Mapping)
                .ToList();
            return ploMapping;
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
                return ex.Message;
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
                return ex.Message;
            }
        }


    }
}
