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
       

        public List<Curriculum> GetAllCurriculum(int degree_level_id, string? txtSearch, int? majorId) => curriculumDAO.GetAllCurriculum(degree_level_id, txtSearch, majorId);

        public List<Batch> GetBatchByCurriculumCode(string curriculumCode) => curriculumDAO.GetBatchByCurriculumCode(curriculumCode);
       

        public Curriculum GetCurriculum(string code) => curriculumDAO.GetCurriculum(code);

        public List<Curriculum> GetCurriculumByBatch(int degreeLevel, string batchName) => curriculumDAO.GetCurriculumByBatch(degreeLevel, batchName);
        

        public Curriculum GetCurriculumById(int id) => curriculumDAO.GetCurriculumById(id);

        public string GetCurriculumCode(int batchId, int speId) => curriculumDAO.GetCurriculumCode(batchId, speId);

       

        public List<Batch> GetListBatchNotExsitInCurriculum(string curriculumCode) => curriculumDAO.GetListBatchNotExsitInCurriculum(curriculumCode);

        public List<Curriculum> GetListCurriculumByBatchName(int batchId, string batchName) => curriculumDAO.GetListCurriculumByBatchName(batchId, batchName);
       

        public int GetTotalCredit(int curriculumId) => curriculumDAO.GetTotalCredit(curriculumId);

        public int GetTotalSemester(int speId, int batchId) => curriculumDAO.GetTotalSemester(speId, batchId);
       

        public List<Curriculum> PanigationCurriculum(int page, int limit, int degree_level_id, string txtSearch, int? majorId) => curriculumDAO.PanigationCurriculum(page,limit, degree_level_id, txtSearch, majorId);  
        

        public string RemoveCurriculum(Curriculum curriculum) => curriculumDAO.DeleteCurriculum(curriculum);


        public string UpdateCurriculum(Curriculum curriculum) => curriculumDAO.UpdateCurriculum(curriculum);
       
    }
}
