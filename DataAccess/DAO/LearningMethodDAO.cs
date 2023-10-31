using BusinessObject;
using DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class LearningMethodDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<LearningMethod> GetAllLearningMethods()
        {
            var listLearningMethod = _cmsDbContext.LearningMethod.ToList();
            return listLearningMethod;
        }

        public List<LearningMethod> PaginationLearningMethod(int page, int limit, string? txtSearch)
        {
            IQueryable<LearningMethod> query = _cmsDbContext.LearningMethod;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.learning_method_name.ToLower().Contains(txtSearch.ToLower()));
            }

            var listLearningMethod = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return listLearningMethod;
        }

        public LearningMethod GetLearningMethodById(int id)
        {
            var learningMethod = _cmsDbContext.LearningMethod.Find(id);
            return learningMethod;
        }

        public bool CheckLearningMethodDuplicate(string learning_method_name)
        {
            return (_cmsDbContext.LearningMethod?.Any(x => x.learning_method_name.Equals(learning_method_name))).GetValueOrDefault();
        }

        public string CreateLearningMethod(LearningMethod learningMethod)
        {
            try
            {
                _cmsDbContext.LearningMethod.Add(learningMethod);
                _cmsDbContext.SaveChanges();
                return Result.createSuccessfull.ToString();
            }catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateLearningMethod(LearningMethod learningMethod)
        {
            try
            {
                _cmsDbContext.LearningMethod.Update(learningMethod);
                _cmsDbContext.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteLearningMethod(LearningMethod learningMethod)
        {
            try
            {
                _cmsDbContext.LearningMethod.Remove(learningMethod);
                _cmsDbContext.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }
    }
}
