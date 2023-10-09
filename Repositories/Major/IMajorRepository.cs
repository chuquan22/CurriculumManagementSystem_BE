using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repositories.Major
{
    public interface IMajorRepository
    {
        public List<BusinessObject.Major> GetAllMajor();

        public BusinessObject.Major AddMajor(BusinessObject.Major major);

        public BusinessObject.Major EditMajor(BusinessObject.Major major);
        public void DeleteMajor(int id);

        public BusinessObject.Major FindMajorById(int majorId);

    }
}
