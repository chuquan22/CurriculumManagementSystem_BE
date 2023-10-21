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
       

        public List<Subject> GetAllSubject() => subjectDAO.GetAllSubjects();

        public Subject GetSubjectByCode(string code)
        {
            return subjectDAO.GetSubjectByCode(code);
        }

        public List<Subject> GetSubjectByCurriculum(int curriculumId)
        {
            return subjectDAO.GetSubjectByCurriculum(curriculumId);
        }

        public Subject GetSubjectById(int id) => subjectDAO.GetSubjectById(id);
       

        public List<Subject> GetSubjectByName(string name) => subjectDAO.GetSubjectByName(name);
        

        public string UpdateSubject(Subject subject) => subjectDAO.UpdateSubject(subject);
       
    }
}
