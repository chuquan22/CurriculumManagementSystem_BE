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

        public List<Curriculum> GetAllCurriculum(string? txtSearch, int? majorId)
        {
            IQueryable<Curriculum> query = _cmsDbContext.Curriculum
                .Include(x => x.Batch)
                .Include(x => x.Specialization)
                .Include(x => x.Specialization.Major)
                .Where(x => x.is_active == true);

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.curriculum_name.Contains(txtSearch) || x.curriculum_code.Contains(txtSearch));
            }

            if (majorId.HasValue)
            {
                query = query.Where(x => x.Specialization.Major.major_id == majorId);
            }

            var curriculumList = query
                .AsEnumerable()
                .OrderByDescending(x => x.Batch.batch_name)  // Sắp xếp theo Batch Name giảm dần
                .ThenBy(x => x.curriculum_code)
                .ToList();
            return curriculumList;
        }

        public List<Curriculum> PanigationCurriculum(int page, int limit, string? txtSearch, int? majorId)
        {
            IQueryable<Curriculum> query = _cmsDbContext.Curriculum
                .Include(x => x.Batch)
                .Include(x => x.Specialization)
                .Include(x => x.Specialization.Major)
                .Where(x => x.is_active == true);

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.english_curriculum_name.Contains(txtSearch) || x.curriculum_code.Contains(txtSearch));
            }

            if (majorId.HasValue)
            {
                query = query.Where(x => x.Specialization.Major.major_id == majorId);
            }

            var curriculumList = query
                .OrderByDescending(x => x.Batch.batch_name) 
                .ThenBy(x => x.curriculum_code) 
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();

            return curriculumList;
        }





        public int GetTotalCredit(int curriculumId)
        {
            var total = _cmsDbContext.Curriculum
                .Where(x => x.curriculum_id == curriculumId && x.is_active == true)
                .Join(_cmsDbContext.CurriculumSubject,
                     curriculum => curriculum.curriculum_id,
                     curriculumSubject => curriculumSubject.curriculum_id,
                     (curriculum, curriculumSubject) => new { curriculum, curriculumSubject })

                .Join(_cmsDbContext.Subject.Where(x => x.is_active == true),
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
                .Include(x => x.CurriculumSubjects)
                .Where(x => x.is_active == true)
                .FirstOrDefault(x => x.curriculum_id == id);
            return curriculum;
        }

        public List<Batch> GetListBatchNotExsitInCurriculum(string curriculumCode)
        {
            var batchIdsInCurriculum = _cmsDbContext.Curriculum
                .Where(curriculum => curriculum.curriculum_code.Equals(curriculumCode) && curriculum.is_active == true)
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
                .Where(x => x.curriculum_code.Equals(curriculumCode) && x.is_active == true)
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
                .Include(x => x.Specialization.Major)
                .Where(x => x.is_active == true)
                .FirstOrDefault(x => x.curriculum_code.Equals(code) && x.batch_id == batchId);

            return curriculum;
        }

        public string GetCurriculumCode(int batchId, int speId)
        {
            var specialization = _cmsDbContext.Specialization.Find(speId);
            var major = _cmsDbContext.Major.Where(x => x.is_active == true).FirstOrDefault(x => x.major_id == specialization.major_id);
            var batch = _cmsDbContext.Batch.Find(batchId);


            var curriCode = GetAbbreviations(major.major_english_name.ToUpper()) + "-" + GetAbbreviations(specialization.specialization_english_name.ToUpper()) + "-" + batch.batch_name;

            return curriCode;
        }


        private string GetAbbreviations(string name)
        {

            var Abbreviations = "";
            string[] parts = name.Split(' ');
            if (parts.Length >= 2)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    string firstLetter = parts[i].Substring(0, 1);
                    if (!parts[i].Equals("AND"))
                    {
                        Abbreviations += firstLetter;
                    }
                }
            }
            else
            {
                Abbreviations = parts[0].ToUpper();
            }
            return Abbreviations;
        }

        public string CreateCurriculum(Curriculum curriculum)
        {
            try
            {
                curriculum.curriculum_name = curriculum.curriculum_name.Trim();
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
                return ex.InnerException.Message;
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
                return ex.InnerException.Message;
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
                return ex.InnerException.Message;
            }
        }
    }
}
