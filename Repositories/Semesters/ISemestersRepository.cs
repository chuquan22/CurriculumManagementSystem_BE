using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Semesters
{
    public interface ISemestersRepository
    {
        public List<Semester> GetSemesters();
        List<Semester> PaginationSemester(int? degree_id, int page, int limit, string? txtSearch);
        List<Semester> GetSemesterBySpe(int speId);
        int GetTotalSemester(int? degree_level, string? txtSearch);
        Semester GetSemester(int id);
        bool CheckSemesterDuplicate(int id, string name, int schoolYear, int batchId);
        bool CheckSemesterExsit(int id);
        string CreateSemester(Semester semester);
        string UpdateSemester(Semester semester);
        string DeleteSemester(Semester semester);

        List<Semester> GetAllSemestersByMajorId(int id);
        List<Semester> GetSemesterByDegreeLevel(int id);
    }
}
