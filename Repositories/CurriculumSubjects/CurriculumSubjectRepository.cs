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

        public List<CurriculumSubject> GetAllCurriculumSubject()
        {
            return curriculumDAO.GetAll();
        }

        public CurriculumSubject GetCurriculumSubjectById(int curriculumId, int subjectId)
        {
            return curriculumDAO.GetCurriculumSubjectById(curriculumId, subjectId);
        }

        public List<CurriculumSubject> GetListCurriculumBySubject(int subjectId)
        {
            return curriculumDAO.GetCurriculumBySubject(subjectId);
        }

        public List<CurriculumSubject> GetListSubjectByCurriculum(int curriculumId)
        {
            return curriculumDAO.GetSubjectByCurriculum(curriculumId);
        }

        public string UpdateCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            return curriculumDAO.UpdateCurriculumSubject(curriculumSubject);
        }
    }
}
