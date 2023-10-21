using BusinessObject;
using DataAccess.Models.DTO.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Syllabus
{
    public interface ISyllabusRepository
    {
         public List<BusinessObject.Syllabus> GetListSyllabus(int start, int end, string txtSearch, string subjectCode);
         public int GetTotalSyllabus(string txtSearch, string subjectCode);
        public List<PreRequisite> GetPre(int id);
        public BusinessObject.Syllabus GetSyllabusById(int id);    

    }
}
