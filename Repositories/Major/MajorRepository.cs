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

        public void DeleteMajor(int id)
        {
            db.DeleteMajor(id);
        }

        public BusinessObject.Major EditMajor(BusinessObject.Major major)
        {
            return db.EditMajor(major);
        }

        public BusinessObject.Major FindMajorById(int majorId)
        {
            return db.FindMajorById(majorId);
        }

        public List<BusinessObject.Major> GetAllMajor()
        {
            return db.GetAllMajor();
        }
    }
}
