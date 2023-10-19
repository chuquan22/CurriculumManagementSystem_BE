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
    }
}
