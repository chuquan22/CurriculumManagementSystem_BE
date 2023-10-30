using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CurriculumSubjects
{
    public class CurriculumSubjectRepository : ICurriculumSubjectRepository
    {
        private readonly CurriculumSubjectDAO curriculumDAO = new CurriculumSubjectDAO();
        public string CreateCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            return curriculumDAO.CreateCurriculumSubject(curriculumSubject);
        }

        public string DeleteCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            return curriculumDAO.DeleteCurriculumSubject(curriculumSubject);
        }

        public CurriculumSubject GetCurriculumSubjectById(int curriculumId, int subjectId)
        {
            return curriculumDAO.GetCurriculumSubjectById(curriculumId, subjectId);
        }

        public List<CurriculumSubject> GetCurriculumSubjectByTermNo(int term_no)
        {
            return curriculumDAO.GetCurriculumSubjectByTermNo(term_no);
        }

        public CurriculumSubject GetCurriculumSubjectByTermNoAndSubjectGroup(int term_no, string subjectGroup, int subjectId)
        {
            return curriculumDAO.GetCurriculumSubjectByTermNoAndSubjectGroup(term_no, subjectGroup, subjectId);
        }

        public List<CurriculumSubject> GetListCurriculumBySubject(int subjectId)
        {
            return curriculumDAO.GetCurriculumBySubject(subjectId);
        }

        public List<CurriculumSubject> GetListCurriculumSubject(int curriculumId)
        {
            return curriculumDAO.GetListCurriculumSubject(curriculumId);
        }

        public List<Subject> GetListSubject(int curriculumId)
        {
           return curriculumDAO.GetListSubject(curriculumId);
        }

        public List<CurriculumSubject> GetListSubjectByCurriculum(int curriculumId)
        {
            return curriculumDAO.GetListSubjectByCurriculum(curriculumId);
        }

        public string UpdateCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            return curriculumDAO.UpdateCurriculumSubject(curriculumSubject);
        }
    }
}
