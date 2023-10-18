using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess.DAO;
using DataAccess.Models.DTO.response;

namespace Repositories.Syllabus
{
    public class SyllabusRepository : ISyllabusRepository
    {
        public SyllabusDAO db = new SyllabusDAO();

        public List<BusinessObject.Syllabus> GetListSyllabus(int start, int end, string txtSearch, string subjectCode)
        {
            return db.GetListSyllabus(start, end, txtSearch);
        }

        public List<PreRequisite> GetPre(int id)
        {
           return db.GetListPre(id);
        }

        public BusinessObject.Syllabus GetSyllabusById(int id)
        {
           return db.GetSyllabusById(id);
        }

        public int GetTotalSyllabus(string txtSearch)
        {
            return db.GetTotalSyllabus(txtSearch);
        }
    }
}
