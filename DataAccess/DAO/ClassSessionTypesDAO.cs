using BusinessObject;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
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

        public List<ClassSessionType> PaginationClassSessionType(int page, int limit, string? txtSearch)
        {
            IQueryable<ClassSessionType> query = _context.ClassSessionType;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.class_session_type_name.ToLower().Contains(txtSearch.ToLower()));
            }

            var listClassSessionType = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return listClassSessionType;
        }

        public int GetTotalClassSessionType(string? txtSearch)
        {
            IQueryable<ClassSessionType> query = _context.ClassSessionType;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.class_session_type_name.ToLower().Contains(txtSearch.ToLower().Trim()));
            }

            var total = query
                .ToList().Count;
            return total;
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

        public bool CheckClassSessionTypeExsit(int id)
        {
            return (_context.Session?.Any(x => x.class_session_type_id == id)).GetValueOrDefault();
        }

        public ClassSessionType GetClassSessionTypeByName(string name)
        {
            var classSessionTyoe = _context.ClassSessionType.Where(x => x.class_session_type_name.Contains(name)).FirstOrDefault();
            return classSessionTyoe;

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
