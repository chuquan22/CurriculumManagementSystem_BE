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

        public BusinessObject.Syllabus CreateSyllabus(BusinessObject.Syllabus rs)
        {
            return db.CreateSyllabus(rs);
        }

        public List<BusinessObject.Syllabus> GetListSyllabus(int start, int end, string txtSearch, string subjectCode)
        {
            return db.GetListSyllabus(start, end, txtSearch, subjectCode);
        }

        public List<PreRequisite> GetPre(int id)
        {
           return db.GetListPre(id);
        }

        public BusinessObject.Syllabus GetSyllabusById(int id)
        {
           return db.GetSyllabusById(id);
        }

        public int GetTotalSyllabus(string? txtSearch, string? subjectCode)
        {
            return db.GetTotalSyllabus(txtSearch, subjectCode);
        }

        public bool SetApproved(int id)
        {
            return db.SetApproved(id);
        }

        public bool SetStatusSyllabus(int id)
        {
            return db.SetStatus(id);
        }

        public string UpdatePatchSyllabus(BusinessObject.Syllabus syllabus)
        {
            return db.UpdatePatchSyllabus(syllabus);
        }
    }
}
