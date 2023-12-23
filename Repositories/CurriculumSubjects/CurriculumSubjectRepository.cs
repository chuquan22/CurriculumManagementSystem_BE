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

        public bool CheckCreditComboSubjectOrOptionSubjectMustEqualInTermNo(int curriId, int credit, int term_no, int? option, int? combo)
        {
            return curriculumDAO.CheckCreditComboSubjectOrOptionSubjectMustEqualInTermNo(curriId, credit, term_no, option, combo);
        }

        public string CreateCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            return curriculumDAO.CreateCurriculumSubject(curriculumSubject);
        }

        public bool CurriculumSubjectExist(int curriId, int subId)
        {
            return curriculumDAO.CurriculumSubjectExist(curriId, subId);
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

        public CurriculumSubject GetCurriculumSubjectByTermNoAndSubjectGroup(int curriId, int term_no, int subject_id, int option)
        {
            return curriculumDAO.GetCurriculumSubjectByTermNoAndSubjectGroup( curriId,term_no, subject_id, option);

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
