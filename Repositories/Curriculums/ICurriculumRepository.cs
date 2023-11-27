using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Curriculums
{
    public interface ICurriculumRepository
    {
        List<Curriculum> GetAllCurriculum(int degree_level_id, string? txtSearch, int? majorId);
        List<Curriculum> PanigationCurriculum(int page, int limit, int degree_level_id, string txtSearch, int? majorId);
        List<Curriculum> GetCurriculumByBatch(int degreeLevel, string batchName);
        Curriculum GetCurriculum(string code);
        List<Batch> GetListBatchNotExsitInCurriculum(string curriculumCode);
        string GetCurriculumCode(int batchId, int speId);
        Curriculum GetCurriculumById(int id);
        int GetTotalSemester(int speId, int batchId);
        int GetTotalCredit(int curriculumId);
        List<Batch> GetBatchByCurriculumCode(string curriculumCode);
        List<Curriculum> GetListCurriculumByBatchName(int batchId, string batchName);
        string CreateCurriculum(Curriculum curriculum);
        string UpdateCurriculum(Curriculum curriculum);
        string RemoveCurriculum(Curriculum curriculum);
    }
}
