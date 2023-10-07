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
    public class CurriculumSubjectDAO
    {
        public readonly CMSDbContext _context = new CMSDbContext();

        public List<CurriculumSubject> GetAll()
        {
            var listCurriculumSubject = _context.CurriculumSubject.Include(x => x.Curriculum).Include(x =>x.Subject).ToList();
            return listCurriculumSubject;
        }

        public List<CurriculumSubject> GetCurriculumBySubject(int subjectId)
        {
            var listCurriculumSubject = _context.CurriculumSubject.Include(x => x.Curriculum).Include(x => x.Subject).Where(x => x.subject_id == subjectId).ToList();
            return listCurriculumSubject;
        }

        public List<CurriculumSubject> GetSubjectByCurriculum(int curriculumId)
        {
            var listCurriculumSubject = _context.CurriculumSubject.Include(x => x.Curriculum).Include(x => x.Subject).Where(x => x.curriculum_id == curriculumId).ToList();
            return listCurriculumSubject;
        }

        public CurriculumSubject GetCurriculumSubjectById(int curriculumId, int subjectId)
        {
            var curriculumSubject = _context.CurriculumSubject.Include(x => x.Curriculum).Include(x =>x.Subject).FirstOrDefault(x => x.curriculum_id == curriculumId && x.subject_id == subjectId);
            return curriculumSubject;
        }

        public string CreateCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            try
            {
                _context.CurriculumSubject.Add(curriculumSubject);
                _context.SaveChanges();
                return Result.createSuccessfull.ToString();
            }catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            try
            {
                _context.CurriculumSubject.Update(curriculumSubject);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeleteCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            try
            {
                _context.CurriculumSubject.Remove(curriculumSubject);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
