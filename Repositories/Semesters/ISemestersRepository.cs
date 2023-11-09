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
        List<Semester> PaginationSemester(int page, int limit, string? txtSearch);
        List<Semester> GetSemesterBySpe(int speId);
        int GetTotalSemester(string? txtSearch);
        Semester GetSemester(int id);
        bool CheckSemesterDuplicate(int id, string name, int schoolYear, int degreeId);
        bool CheckSemesterExsit(int id);
        string CreateSemester(Semester semester);
        string UpdateSemester(Semester semester);
        string DeleteSemester(Semester semester);

        List<Semester> GetAllSemestersByMajorId(int id);
        object GetSemesterByDegreeLevel(int id);
    }
}
