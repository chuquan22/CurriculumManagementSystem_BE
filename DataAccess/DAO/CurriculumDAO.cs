using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CurriculumDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<Curriculum> GetAllCurriculum()
        {
            var curriculumList = _cmsDbContext.Curriculum
                .Include(x => x.Batch)
                .Include(x => x.Specialization)
                .AsEnumerable()
                .DistinctBy(x => x.curriculum_code)
                .ToList();
            return curriculumList;
        }

        public Curriculum GetCurriculumById(int id)
        {
            var curriculum = _cmsDbContext.Curriculum.Include(x => x.Batch).Include(x => x.Specialization).FirstOrDefault(x => x.curriculum_id == id);
            return curriculum;
        }

        public Curriculum GetCurriculum(string code, int batchId)
        {
            var curriculum = _cmsDbContext.Curriculum
                .Include(x => x.Batch)
                .Include(x => x.Specialization)
                .FirstOrDefault(x => x.curriculum_code.Equals(code) && x.batch_id == batchId);
                
            return curriculum;
        }

        public string CreateCurriculum(Curriculum curriculum)
        {
            try
            {
                _cmsDbContext.Curriculum.Add(curriculum);
                int number = _cmsDbContext.SaveChanges();
                if (number > 0)
                {
                    return "OK";
                }
                else
                {
                    return "Fail";
                }
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateCurriculum(Curriculum curriculum)
        {
            try
            {
                _cmsDbContext.Curriculum.Update(curriculum);
                _cmsDbContext.SaveChanges();
                int number = _cmsDbContext.SaveChanges();
                if (number > 0)
                {
                    return "OK";
                }
                else
                {
                    return "Fail";
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }

        public string DeleteCurriculum(Curriculum curriculum)
        {
            try
            {
                _cmsDbContext.Curriculum.Remove(curriculum);
                int number = _cmsDbContext.SaveChanges();
                if (number > 0)
                {
                    return "OK";
                }
                else
                {
                    return "Fail";
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }
    }
}
