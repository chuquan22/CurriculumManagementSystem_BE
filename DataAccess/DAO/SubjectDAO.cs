
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SubjectDAO
    {
        public readonly CMSDbContext CMSDbContext = new CMSDbContext();


        public List<Subject> GetAllSubjects(string txtSearch)
        {
            IQueryable<Subject> query = CMSDbContext.Subject
               .Include(x => x.AssessmentMethod.AssessmentType)
               .Include(x => x.LearningMethod)
               .Include(x => x.CurriculumSubjects)
               .Where(x => x.is_active == true);

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.subject_code.ToLower().Contains(txtSearch.ToLower()));
            }

            var subjectList = query
                .AsEnumerable()
                .ToList();
            return subjectList;
        }

        public Subject GetSubjectById(int id)
        {
            try
            {
                var subject = CMSDbContext.Subject.Include(x => x.AssessmentMethod.AssessmentType).Include(x => x.LearningMethod).Where(x => x.is_active == true).SingleOrDefault(x => x.subject_id == id);
                return subject;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Subject> GetSubjectByCurriculum(int curriculumId)
        {
            var listSubjectIds = CMSDbContext.Curriculum
                .Include(x => x.CurriculumSubjects)
                .Where(x => x.curriculum_id == curriculumId)

                .Join(CMSDbContext.CurriculumSubject.Include(x => x.Subject),
                    curriculum => curriculum.curriculum_id,
                    curriculumSubject => curriculumSubject.curriculum_id,
                     (curriculum, curriculumSubject) => curriculumSubject.Subject)
                .ToList();

            return listSubjectIds;


        }

        public List<Subject> GetSubjectBySpecialization(int speId)
        {
            var listcurri = CMSDbContext.Curriculum.Where(x => x.specialization_id == speId && x.is_active == true).ToList();
            var listSubjects = new List<Subject>();
            foreach (var item in listcurri)
            {
                var Subjects = CMSDbContext.Curriculum
               .Include(x => x.CurriculumSubjects)
               .Where(x => x.curriculum_id == item.curriculum_id && x.is_active == true)
               .Join(CMSDbContext.CurriculumSubject.Include(x => x.Subject),
                   curriculum => curriculum.curriculum_id,
                   curriculumSubject => curriculumSubject.curriculum_id,
                    (curriculum, curriculumSubject) => curriculumSubject.Subject)
               .ToList();

                foreach (var subject in Subjects)
                {
                    listSubjects.Add(subject);
                }
            }
            return listSubjects;
        }

        public List<Subject> GetSubjectByMajorId(int majorId)
        {
            var listSubject = new List<Subject>();
            var listSpe = CMSDbContext.Specialization.Where(x => x.major_id == majorId && x.is_active == true).ToList();
            foreach (var spe in listSpe)
            {
                var subjects = GetSubjectBySpecialization(spe.specialization_id);
                foreach (var s in subjects)
                {
                    listSubject.Add(s);
                }
            }
            return listSubject;
        }

        public Subject GetSubjectBySyllabus(int syllabus_id)
        {
            var syllabus = CMSDbContext.Syllabus.Include(x => x.Subject).FirstOrDefault(x => x.syllabus_id == syllabus_id);
            var subject = CMSDbContext.Subject.FirstOrDefault(x => x.subject_id == syllabus.subject_id);

            return subject;


        }

        public int GetNumberSubjectNoSyllabus(List<Subject> subjects)
        {
            var count = 0;
            foreach (var subject in subjects)
            {
                var syllabus = CMSDbContext.Syllabus.Where(x => x.subject_id == subject.subject_id).FirstOrDefault();
                if (syllabus == null)
                {
                    count++;
                }
            }
            return count;
        }

        public List<Subject> GetSubjectByName(string name)
        {
            try
            {
                var list = CMSDbContext.Subject.Include(x => x.AssessmentMethod).Include(x => x.LearningMethod).Where(x => x.is_active == true).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Subject> GetSubjectByLearningMethod(int learningMethodId)
        {
            try
            {
                var list = CMSDbContext.Subject.Include(x => x.AssessmentMethod).Include(x => x.LearningMethod).Where(x => x.is_active == true && x.learning_method_id == learningMethodId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Subject GetSubjectByCode(string code)
        {
            try
            {
                var subject = CMSDbContext.Subject.Where(x => x.is_active == true).FirstOrDefault(x => x.subject_code.ToUpper().Equals(code.ToUpper()));
                return subject;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Subject> GetListSubjectByTermNo(int term_no, int curriculumId)
        {
            var listSubject = new List<Subject>();
            var curriculumSubject = CMSDbContext.CurriculumSubject.Where(x => x.term_no == term_no && x.curriculum_id == curriculumId).ToList();
            foreach (var item in curriculumSubject)
            {
                var subject = CMSDbContext.Subject.Where(x => x.is_active == true).FirstOrDefault(x => x.subject_id == item.subject_id);
                listSubject.Add(subject);
            }
            return listSubject;
        }

        public string CreateSubject(Subject subject)
        {
            try
            {
                CMSDbContext.Subject.Add(subject);
                CMSDbContext.SaveChanges();
                return "OK";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }

        public string UpdateSubject(Subject subject)
        {
            try
            {
                CMSDbContext.Subject.Update(subject);
                CMSDbContext.SaveChanges();
                return "OK";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }

        public string DeleteSubject(Subject subject)
        {
            try
            {
                CMSDbContext.Subject.Remove(subject);
                CMSDbContext.SaveChanges();
                return "OK";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }
    }
}
