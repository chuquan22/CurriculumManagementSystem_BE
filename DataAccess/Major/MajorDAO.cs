using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace DataAccess.Major
{
    public class MajorDAO
    {
        public CMSDbContext db = new CMSDbContext();
        public List<BusinessObject.Major> GetAllMajor()
        {
            List<BusinessObject.Major> list = new List<BusinessObject.Major>();
            list = db.Major.ToList();
            return list;
        }

        public BusinessObject.Major AddMajor(BusinessObject.Major major)
        {
            db.Major.Add(major);
            db.SaveChanges();
            return major;
        }
    }
}
