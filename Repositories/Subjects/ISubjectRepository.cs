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
        List<SubjectResponse> GetAllSubject();
        SubjectResponse GetSubjectById(int id);
        List<SubjectResponse> GetSubjectByName(string name);
        string CreateNewSubject(SubjectRequest subject);
        string UpdateSubject(SubjectRequest subject);
        string DeleteSubject(Subject subject);
    }
}
