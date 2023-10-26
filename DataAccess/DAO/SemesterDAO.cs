using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SemesterDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<Semester> GetSemesters()
        {
            var listSemesters = _cmsDbContext.Semester.Include(x => x.Batch).ToList();
            return listSemesters;
        }

        public Semester GetSemester(int id)
        {
            var semester = _cmsDbContext.Semester.FirstOrDefault(x => x.semester_id == id);
            return semester;
        }

    }
}
