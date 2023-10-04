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

        public string CreateNewSubject(SubjectRequest subject) => subjectDAO.CreateSubject(subject);
       

        public string DeleteSubject(Subject subject) => subjectDAO.DeleteSubject(subject);
       

        public List<SubjectResponse> GetAllSubject() => subjectDAO.GetAllSubjects();
       

        public SubjectResponse GetSubjectById(int id) => subjectDAO.GetSubjectById(id);
       

        public List<SubjectResponse> GetSubjectByName(string name) => subjectDAO.GetSubjectByName(name);
        

        public string UpdateSubject(SubjectRequest subject) => subjectDAO.UpdateSubject(subject);
       
    }
}
