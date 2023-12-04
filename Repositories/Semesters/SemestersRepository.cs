using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Semesters
{
    public class SemestersRepository : ISemestersRepository
    {
        public SemesterDAO db = new SemesterDAO();

        public bool CheckSemesterDuplicate(int id, string name, int schoolYear, int batchId)
        {
            return db.CheckSemesterDuplicate(id, name, schoolYear, batchId);
        }

        public bool CheckSemesterExsit(int id)
        {
            return db.CheckSemesterExsit(id);
        }

        public string CreateSemester(Semester semester)
        {
            return db.CreateSemester(semester);
        }

        public string DeleteSemester(Semester semester)
        {
            return db.DeleteSemester(semester);
        }

        public List<Semester> GetAllSemestersByMajorId(int id)
        {
            return db.GetAllSemestersByMajorId(id);
        }

        public Semester GetSemester(int id)
        {
            return db.GetSemester(id);
        }

        public List<Semester> GetSemesterByDegreeLevel(int id)
        {
            return db.GetSemesterByDegreeLevel(id);
        }

        public List<Semester> GetSemesterBySpe(int speId)
        {
            return db.GetSemesterBySpe(speId);
        }

        public List<Semester> GetSemesters()
        {
            return db.GetSemesters();
        }

        public int GetTotalSemester(string? txtSearch)
        {
            return db.GetTotalSemester(txtSearch);
        }

        public List<Semester> PaginationSemester(int page, int limit, string? txtSearch)
        {
            return db.PaginationSemester(page, limit, txtSearch);
        }

        public string UpdateSemester(Semester semester)
        {
            return db.UpdateSemester(semester);
        }
    }
}
