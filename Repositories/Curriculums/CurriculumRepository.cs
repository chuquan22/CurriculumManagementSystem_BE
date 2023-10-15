using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Curriculums
{
    public class CurriculumRepository : ICurriculumRepository
    {
        public readonly CurriculumDAO curriculumDAO = new CurriculumDAO();
        public string CreateCurriculum(Curriculum curriculum) => curriculumDAO.CreateCurriculum(curriculum);
       

        public List<Curriculum> GetAllCurriculum() => curriculumDAO.GetAllCurriculum();

        public Curriculum GetCurriculum(string code, int batchId) => curriculumDAO.GetCurriculum(code, batchId);
       

        public Curriculum GetCurriculumById(int id) => curriculumDAO.GetCurriculumById(id);


        public string RemoveCurriculum(Curriculum curriculum) => curriculumDAO.DeleteCurriculum(curriculum);


        public string UpdateCurriculum(Curriculum curriculum) => curriculumDAO.UpdateCurriculum(curriculum);
       
    }
}
