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
       

        public List<Curriculum> GetAllCurriculum(string? txtSearch, int? majorId) => curriculumDAO.GetAllCurriculum(txtSearch, majorId);

        public List<Batch> GetBatchByCurriculumCode(string curriculumCode) => curriculumDAO.GetBatchByCurriculumCode(curriculumCode);
       

        public Curriculum GetCurriculum(string code, int batchId) => curriculumDAO.GetCurriculum(code, batchId);

        

        public Curriculum GetCurriculumById(int id) => curriculumDAO.GetCurriculumById(id);

        public string GetCurriculumCode(int batchId, int speId) => curriculumDAO.GetCurriculumCode(batchId, speId);
        

        public List<Batch> GetListBatchNotExsitInCurriculum(string curriculumCode) => curriculumDAO.GetListBatchNotExsitInCurriculum(curriculumCode);
        

        public int GetTotalCredit(int curriculumId) => curriculumDAO.GetTotalCredit(curriculumId);
        

        public List<Curriculum> PanigationCurriculum(int page, int limit, string txtSearch, int? majorId) => curriculumDAO.PanigationCurriculum(page,limit, txtSearch, majorId);  
        

        public string RemoveCurriculum(Curriculum curriculum) => curriculumDAO.DeleteCurriculum(curriculum);


        public string UpdateCurriculum(Curriculum curriculum) => curriculumDAO.UpdateCurriculum(curriculum);
       
    }
}
