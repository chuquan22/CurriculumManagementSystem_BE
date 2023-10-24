using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ClassSessionTypesDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<ClassSessionType> GetListClassSessionType()
        {
            var listClassSessionType = _context.ClassSessionType

                .ToList();
            return listClassSessionType;
        }
        public ClassSessionType GetClassSessionType(int id)
        {
            var classSessionTyoe = _context.ClassSessionType.Where(x => x.class_session_type_id == id).FirstOrDefault();
            return classSessionTyoe;
        }
    }
}
