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
            var listCurriculum = _cmsDbContext.Curriculum.Include(x => x.Batch).Include(x => x.Specialization).ToList();
            return listCurriculum;
        }

        public Curriculum GetCurriculumById(int id)
        {
            var curriculum = _cmsDbContext.Curriculum.Include(x => x.Batch).Include(x => x.Specialization).FirstOrDefault(x => x.curriculum_id == id);
            return curriculum;
        }

        public string CreateCurriculum(Curriculum curriculum)
        {
            try
            {
                _cmsDbContext.Curriculum.Add(curriculum);
                _cmsDbContext.SaveChanges();
                return "OK";
            }catch (Exception ex)
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
                return "OK";
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
                _cmsDbContext.SaveChanges();
                return "OK";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }
    }
}
