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
    public class AssessmentMethodDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<AssessmentMethod> GetAllAssessmentMethod()
        {
            var listAssessmentMethod = _context.AssessmentMethod
                .Include(x => x.AssessmentType)
                .ToList();
            return listAssessmentMethod;
        }

        public AssessmentMethod GetAsssentMethodByName(string name, int id)
        {
            string[] name2 = name.Split(' ');

            var rs = _context.AssessmentMethod
                .Include(a => a.AssessmentType)
                .Where(x =>
                    (x.assessment_method_component.ToUpper().Trim().Contains(name2[0].ToUpper().Trim()) ||
                     x.assessment_method_component.ToUpper().Trim().Contains(name.ToUpper().Trim())) &&
                    x.assessment_type_id == id)
                .FirstOrDefault();

            return rs;
        }

        public AssessmentMethod GetAsssentMethodById(int id)
        {
            var rs = _context.AssessmentMethod.Include(a => a.AssessmentType).Where(x => x.assessment_method_id == id).FirstOrDefault();
            return rs;
        }

        public bool CheckAssmentMethodDuplicate(string name)
        {
            return (_context.AssessmentMethod?.Any(x => x.assessment_method_component == name)).GetValueOrDefault();
        }



        public string CreateAssessmentMethod(AssessmentMethod method)
        {
            try
            {
                _context.AssessmentMethod.Add(method);
                _context.SaveChanges();
                return Result.createSuccessfull.ToString();
            }catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateAssessmentMethod(AssessmentMethod method)
        {
            try
            {
                _context.AssessmentMethod.Update(method);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteAssessmentMethod(AssessmentMethod method)
        {
            try
            {
                _context.AssessmentMethod.Remove(method);
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
