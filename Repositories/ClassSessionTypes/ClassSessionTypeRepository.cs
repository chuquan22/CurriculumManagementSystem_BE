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

        public bool CheckClassSessionTypeDuplicate(int id, string name)
        {
            return db.CheckClassSessionTypeDuplicate(id, name);
        }

        public bool CheckClassSessionTypeExsit(int id)
        {
            return db.CheckClassSessionTypeExsit(id);
        }

        public string CreateClassSessionType(ClassSessionType classSessionType)
        {
            return db.CreateClassSessionType(classSessionType);
        }

        public string DeleteClassSessionType(ClassSessionType classSessionType)
        {
            return db.DeleteClassSessionType(classSessionType);
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

        public int GetTotalClassSessionType(string? txtSearch)
        {
            return db.GetTotalClassSessionType(txtSearch);
        }

        public List<ClassSessionType> PaginationClassSessionType(int page, int limit, string? txtSearch)
        {
            return db.PaginationClassSessionType(page, limit, txtSearch);
        }

        public string UpdateClassSessionType(ClassSessionType classSessionType)
        {
            return db.UpdateClassSessionType(classSessionType);
        }
    }
}
