using BusinessObject;
using DataAccess.DAO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Subjects
{
    public class SubjectRepository : ISubjectRepository
    {
        public readonly SubjectDAO subjectDAO = new SubjectDAO();

        public bool CheckCodeExist(string code)
        {
            return subjectDAO.CheckCodeExist(code);
        }

        public bool CheckIdExist(int id)
        {
            return subjectDAO.CheckIdExist(id);

        }

        public bool CheckIdExistInSyllabus(int id)
        {
            return subjectDAO.CheckIdExistInSyllabus(id);
        }

        public bool CheckSubjectExist(int subject_id)
        {
            return subjectDAO.CheckSubjectExist(subject_id);
        }

        public string CreateNewSubject(Subject subject) => subjectDAO.CreateSubject(subject);
       

        public string DeleteSubject(Subject subject) => subjectDAO.DeleteSubject(subject);
       

        public List<Subject> GetAllSubject(string txtSearch) => subjectDAO.GetAllSubjects(txtSearch);

        public List<Subject> GetListSubjectByTermNo(int term_no, int curriculumId) => subjectDAO.GetListSubjectByTermNo(term_no, curriculumId);

        public int GetNumberSubjectNoSyllabus(List<Subject> subjects) => subjectDAO.GetNumberSubjectNoSyllabus(subjects);
        
        public Subject GetSubjectByCode(string code)
        {
            return subjectDAO.GetSubjectByCode(code);
        }

        public List<Subject> GetSubjectByCurriculum(int curriculumId)
        {
            return subjectDAO.GetSubjectByCurriculum(curriculumId);
        }

        public Subject GetSubjectById(int id) => subjectDAO.GetSubjectById(id);

        public List<Subject> GetSubjectByLearningMethod(int learningMethodId) => subjectDAO.GetSubjectByLearningMethod(learningMethodId);

        public List<Subject> GetSubjectByMajorId(int majorId) => subjectDAO.GetSubjectByMajorId(majorId);
       

        public List<Subject> GetSubjectByName(string name) => subjectDAO.GetSubjectByName(name);


        public List<Subject> GetSubjectBySpecialization(int speId, string batch_name) => subjectDAO.GetSubjectBySpecialization(speId, batch_name);
        

        public Subject GetSubjectBySyllabus(int syllabus_id) => subjectDAO.GetSubjectBySyllabus(syllabus_id);

        public List<Subject> PaginationSubject(int page, int limit, string? txtSearch)
        {
            return subjectDAO.PaginationSubject(page, limit, txtSearch);
        }

        public string UpdateSubject(Subject subject) => subjectDAO.UpdateSubject(subject);

        public int GetTotalSubject(string? txtSearch)
        {
            return subjectDAO.GetTotalSubject(txtSearch);
        }


    }
}
