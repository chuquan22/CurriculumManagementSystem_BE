using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Major;

namespace Repositories.Major
{
    public class MajorRepository : IMajorRepository
    {
        public MajorDAO db = new MajorDAO();

        public BusinessObject.Major AddMajor(BusinessObject.Major major)
        {
            return db.AddMajor(major);
        }

        public List<BusinessObject.Major> GetAllMajor()
        {
            return db.GetAllMajor();
        }
    }
}
