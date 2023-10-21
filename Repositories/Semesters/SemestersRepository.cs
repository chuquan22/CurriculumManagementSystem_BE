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
        public List<Semester> GetSemesters()
        {
            return db.GetSemesters();
        }
    }
}
