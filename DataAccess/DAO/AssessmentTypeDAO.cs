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
    public class AssessmentTypeDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<AssessmentType> GetAllAssessmentMethod()
        {
            var listAssessmentType= _context.AssessmentType
          
                .ToList();
            return listAssessmentType;
        }

        public AssessmentType GetAssessmentTypeByName(string name)
        {
            var ass = _context.AssessmentType.Where(x => x.assessment_type_name.Equals(name.Trim())).FirstOrDefault();
            return ass;
        }

        public AssessmentType GetAsssentTypeById(int id)
        {
            var rs = _context.AssessmentType.Where(x => x.assessment_type_id == id).FirstOrDefault();
            return rs;
        }

        public bool CheckAssmentTypeDuplicate(string name)
        {
            return (_context.AssessmentType?.Any(x => x.assessment_type_name == name)).GetValueOrDefault();
        }

        public bool CheckAssmentTypeExsit(int id)
        {
            return (_context.AssessmentMethod?.Any(x => x.assessment_type_id == id)).GetValueOrDefault();
        }

        public string CreateAssessmentType(AssessmentType type)
        {
            try
            {
                _context.AssessmentType.Add(type);
                _context.SaveChanges();
                return Result.createSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateAssessmentType(AssessmentType type)
        {
            try
            {
                _context.AssessmentType.Update(type);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteAssessmentType(AssessmentType type)
        {
            try
            {
                _context.AssessmentType.Remove(type);
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
