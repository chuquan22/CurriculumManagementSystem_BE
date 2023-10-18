﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CurriculumSubjects
{
    public interface ICurriculumSubjectRepository
    {
        CurriculumSubject GetCurriculumSubjectById(int curriculumId, int subjectId);
        List<CurriculumSubject> GetListCurriculumBySubject(int subjectId);
        List<CurriculumSubject> GetListSubjectByCurriculum(int curriculumId);
        List<CurriculumSubject> GetCurriculumSubjectByTermNo(int term_no);
        List<CurriculumSubject> GetListCurriculumSubject(int curriculumId);
        List<Subject> GetListSubject(int curriculumId);
        string CreateCurriculumSubject(CurriculumSubject curriculumSubject);
        string UpdateCurriculumSubject(CurriculumSubject curriculumSubject);
        string DeleteCurriculumSubject(CurriculumSubject curriculumSubject);
    }
}
