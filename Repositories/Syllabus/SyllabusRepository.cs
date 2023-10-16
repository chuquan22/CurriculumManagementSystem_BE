using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess.DAO;

namespace Repositories.Syllabus
{
    public class SyllabusRepository : ISyllabusRepository
    {
        public SyllabusDAO db = new SyllabusDAO();

        public List<BusinessObject.Syllabus> GetListSyllabus(int start, int end, string txtSearch)
        {
            return db.GetListSyllabus(start, end, txtSearch);
        }

        public int GetTotalSyllabus(string txtSearch)
        {
            return db.GetTotalSyllabus(txtSearch);
        }
    }
}
