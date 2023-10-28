using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ClassSessionTypes
{
    public interface IClassSessionTypeRepository
    {
        public List<ClassSessionType> GetListClassSessionType();

        public ClassSessionType GetClassSessionType(int id);
        public ClassSessionType GetClassSessionTypeByName(string name);

    }
}
