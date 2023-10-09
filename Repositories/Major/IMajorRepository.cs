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
    }
}
