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
        Subject GetSubjectBySyllabus(int syllabus_id);
        List<Subject> GetSubjectByName(string name);
        Subject GetSubjectByCode(string code);
        List<Subject> GetListSubjectByTermNo(int term_no, int curriculum_id);
        List<Subject> GetSubjectByCurriculum(int curriculumId);
        List<Subject> GetSubjectByLearningMethod(int learningMethodId);
        List<Subject> GetSubjectBySpecialization(int speId);
        int GetNumberSubjectNoSyllabus(List<Subject> subjects);
        List<Subject> GetSubjectByMajorId(int majorId);
        string CreateNewSubject(Subject subject);
        string UpdateSubject(Subject subject);
        string DeleteSubject(Subject subject);
    }
}
