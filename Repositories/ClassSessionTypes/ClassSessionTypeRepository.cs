using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ClassSessionTypes
{
    public class ClassSessionTypeRepository : IClassSessionTypeRepository
    {
        public ClassSessionTypesDAO db = new ClassSessionTypesDAO();

        public bool CheckClassSessionTypeDuplicate(string name)
        {
            return db.CheckClassSessionTypeDuplicate(name);
        }

        public string CreateClassSessionType(ClassSessionType classSessionType)
        {
            return db.CreateClassSessionType(classSessionType);
        }

        public string DeleteClassSessionType(ClassSessionType classSessionType)
        {
            return db.UpdateClassSessionType(classSessionType);
        }

        public ClassSessionType GetClassSessionType(int id)
        {
            return db.GetClassSessionType(id); 
        }

        public ClassSessionType GetClassSessionTypeByName(string name)
        {
            return db.GetClassSessionTypeByName(name);
        }

        public List<ClassSessionType> GetListClassSessionType()
        {
            return db.GetListClassSessionType();
        }

        public string UpdateClassSessionType(ClassSessionType classSessionType)
        {
            return db.DeleteClassSessionType(classSessionType);
        }
    }
}
