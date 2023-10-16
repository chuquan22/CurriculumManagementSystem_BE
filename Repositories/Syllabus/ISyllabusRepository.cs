using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Syllabus
{
    public interface ISyllabusRepository
    {
         public List<BusinessObject.Syllabus> GetListSyllabus(int start, int end, string txtSearch);
        public int GetTotalSyllabus(string txtSearch);

    }
}
