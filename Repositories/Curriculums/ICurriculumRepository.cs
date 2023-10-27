﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Curriculums
{
    public interface ICurriculumRepository
    {
        List<Curriculum> GetAllCurriculum(string? txtSearch, int? majorId);
        List<Curriculum> PanigationCurriculum(int page, int limit, string txtSearch, int? majorId);
        Curriculum GetCurriculum(string code, int batchId);
        List<Batch> GetListBatchNotExsitInCurriculum(string curriculumCode);
        string GetCurriculumCode(int batchId, int speId, string degree_level);
        Curriculum GetCurriculumById(int id);
        int GetTotalCredit(int curriculumId);
        List<Batch> GetBatchByCurriculumCode(string curriculumCode);
        string CreateCurriculum(Curriculum curriculum);
        string UpdateCurriculum(Curriculum curriculum);
        string RemoveCurriculum(Curriculum curriculum);
    }
}
