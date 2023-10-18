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

        public List<Curriculum> GetAllCurriculum(string? txtSearch, int? specializationId)
        {
            IQueryable<Curriculum> query = _cmsDbContext.Curriculum
                .Include(x => x.Batch)
                .Include(x => x.Specialization);

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.curriculum_name.Contains(txtSearch) || x.curriculum_code.Contains(txtSearch));
            }

            if (specializationId.HasValue)
            {
                query = query.Where(x => x.specialization_id == specializationId);
            }

            var curriculumList = query
                .AsEnumerable()
                .ToList();
            return curriculumList;
        }

        public List<Curriculum> PanigationCurriculum(int page, int limit, string? txtSearch, int? specializationId)
        {
            IQueryable<Curriculum> query = _cmsDbContext.Curriculum
                .Include(x => x.Batch)
                .Include(x => x.Specialization);

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.curriculum_name.Contains(txtSearch) || x.curriculum_code.Contains(txtSearch));
            }

            if (specializationId.HasValue)
            {
                query = query.Where(x => x.specialization_id == specializationId);
            }

            var curriculumList = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .AsEnumerable()
                .ToList();

            return curriculumList;
        }


      

        public int GetTotalCredit(int curriculumId)
        {
            var total = _cmsDbContext.Curriculum
                .Where(x => x.curriculum_id == curriculumId)
                .Join(_cmsDbContext.CurriculumSubject,
                     curriculum => curriculum.curriculum_id,
                     curriculumSubject => curriculumSubject.curriculum_id,
                     (curriculum, curriculumSubject) => new { curriculum, curriculumSubject })

                .Join(_cmsDbContext.Subject,
                        joinResult => joinResult.curriculumSubject.subject_id,
                        subject => subject.subject_id,
                        (joinResult, subject) => subject)
                 .Sum(subject => subject.credit);
            return total;
        }


        public Curriculum GetCurriculumById(int id)
        {
            var curriculum = _cmsDbContext.Curriculum
                .Include(x => x.Batch)
                .Include(x => x.Specialization)
                .Include(x => x.Specialization.Major)
                .FirstOrDefault(x => x.curriculum_id == id);
            return curriculum;
        }

        public List<Batch> GetListBatchNotExsitInCurriculum(string curriculumCode)
        {
            var batchIdsInCurriculum = _cmsDbContext.Curriculum
                .Where(curriculum => curriculum.curriculum_code.Equals(curriculumCode))
                .Select(curriculum => curriculum.batch_id)
                .ToList();

            // Lấy danh sách tất cả các batch
            var allBatches = _cmsDbContext.Batch.ToList();

            // Lọc danh sách batch không chứa batch_id trong danh sách batchIdsInCurriculum
            var batchesNotInCurriculum = allBatches
                .Where(batch => !batchIdsInCurriculum.Contains(batch.batch_id))
                .ToList();

            return batchesNotInCurriculum;
        }

        public List<Batch> GetBatchByCurriculumCode(string curriculumCode)
        {
            var listBatch = _cmsDbContext.Curriculum
                .Where(x => x.curriculum_code.Equals(curriculumCode))
                .Join(_cmsDbContext.Batch,
                      curriculum => curriculum.batch_id,
                      batch => batch.batch_id,
                      (curriculum, batch) => batch)
                .ToList();

            return listBatch;
        }


        public Curriculum GetCurriculum(string code, int batchId)
        {
            var curriculum = _cmsDbContext.Curriculum
                .Include(x => x.Batch)
                .Include(x => x.Specialization)
                .FirstOrDefault(x => x.curriculum_code.Equals(code) && x.batch_id == batchId);

            return curriculum;
        }

        public string GetCurriculumCode(int batchId, int speId)
        {
            var specialization = _cmsDbContext.Specialization.Find(speId);
            var batch = _cmsDbContext.Batch.Find(batchId);

            var curriCode = specialization.specialization_code + "_" + batch.batch_name;

            return curriCode;
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
