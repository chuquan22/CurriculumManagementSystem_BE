using BusinessObject;
using DataAccess.Models.Enums;
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

        public bool CheckClassSessionTypeDuplicate(string name)
        {
            return (_context.ClassSessionType?.Any(x => x.class_session_type_name.Equals(name))).GetValueOrDefault();
        }

        public string CreateClassSessionType(ClassSessionType classSessionType)
        {
            try
            {
                _context.ClassSessionType.Add(classSessionType);
                _context.SaveChanges();
                return Result.createSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateClassSessionType(ClassSessionType classSessionType)
        {
            try
            {
                _context.ClassSessionType.Update(classSessionType);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteClassSessionType(ClassSessionType classSessionType)
        {
            try
            {
                _context.ClassSessionType.Remove(classSessionType);
                _context.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }
    }
}
