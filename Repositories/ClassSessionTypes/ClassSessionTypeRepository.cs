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
    }
}
