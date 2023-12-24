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

        public List<AssessmentMethod> PaginationAssessmentMethod(int page, int limit, string? txtSearch)
        {
            IQueryable<AssessmentMethod> query = _context.AssessmentMethod
                .Include(x => x.AssessmentType);

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.assessment_method_component.ToLower().Contains(txtSearch.ToLower().Trim()));
            }

            var listAssessmentMethod = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return listAssessmentMethod;
        }

        public int GetTotalAssessmentMethod(string? txtSearch)
        {
            IQueryable<AssessmentMethod> query = _context.AssessmentMethod
                .Include(x => x.AssessmentType);

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.assessment_method_component.ToLower().Contains(txtSearch.ToLower().Trim()));
            }
            var total = query
                .ToList().Count;
            return total;
        }

        public AssessmentMethod GetAsssentMethodByName(string name, int id)
        {

            var rs = _context.AssessmentMethod
                .Include(a => a.AssessmentType)
                .Where(x =>
                    
                     x.assessment_method_component.ToUpper().Trim().Contains(name.ToUpper().Trim()) &&
                    x.assessment_type_id == id)
                .FirstOrDefault();

            return rs;
        }

        public AssessmentMethod GetAsssentMethodById(int id)
        {
            var rs = _context.AssessmentMethod.Include(a => a.AssessmentType).Where(x => x.assessment_method_id == id).FirstOrDefault();
            return rs;
        }

        public AssessmentMethod GetAsssentMethodByName(string name)
        {
            var rs = _context.AssessmentMethod.Include(a => a.AssessmentType).Where(x => x.assessment_method_component.ToLower().Equals(name.Trim().ToLower())).FirstOrDefault();
            return rs;
        }

        public bool CheckAssmentMethodDuplicate(int id,string name, int type)
        {
            return (_context.AssessmentMethod?.Any(x => x.assessment_method_component.ToLower().Equals(name.ToLower()) && x.assessment_type_id == type && x.assessment_method_id != id)).GetValueOrDefault();
        }

        public bool CheckAssmentMethodExsit(int id)
        {
            var isExsitInSubject = _context.Subject.Where(x => x.assessment_method_id == id).FirstOrDefault();
            //var isExsitInGradingStructure = _context.GradingStruture.Where(x => x.assessment_method_id == id).FirstOrDefault();
            if (isExsitInSubject == null)
            {
                return false;
            }
            return true;

        }

        public string CreateAssessmentMethod(AssessmentMethod method)
        {
            try
            {
                //_context.AssessmentMethod.Add(method);
                //_context.SaveChanges();
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
                //_context.AssessmentMethod.Remove(method);
                //_context.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }
    }
}
