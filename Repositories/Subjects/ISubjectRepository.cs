using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Subjects
{
    public interface ISubjectRepository
    {
        List<Subject> GetAllSubject();
        Subject GetSubjectById(int id);
        List<Subject> GetSubjectByName(string name);
        Subject GetSubjectByCode(string code);
        List<Subject> GetSubjectByCurriculum(int curriculumId);
        string CreateNewSubject(Subject subject);
        string UpdateSubject(Subject subject);
        string DeleteSubject(Subject subject);
    }
}
