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

        public List<Subject> GetSubjectBySpecialization(int speId) => subjectDAO.GetSubjectBySpecialization(speId);
        

        public Subject GetSubjectBySyllabus(int syllabus_id) => subjectDAO.GetSubjectBySyllabus(syllabus_id);
        

        public string UpdateSubject(Subject subject) => subjectDAO.UpdateSubject(subject);
       
    }
}
