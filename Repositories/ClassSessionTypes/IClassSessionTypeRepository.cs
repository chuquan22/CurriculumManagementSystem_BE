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

        bool CheckClassSessionTypeDuplicate(string name);

        string CreateClassSessionType(ClassSessionType classSessionType);
        string UpdateClassSessionType(ClassSessionType classSessionType);
        string DeleteClassSessionType(ClassSessionType classSessionType);

        public ClassSessionType GetClassSessionTypeByName(string name);


    }
}
