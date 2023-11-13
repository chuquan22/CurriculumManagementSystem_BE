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
    public class LearningResourceDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<LearningResource> GetLearningResource()
        {
            var rs = _cmsDbContext.LearningResource
                .ToList();
            return rs;
        }

        public List<LearningResource> PaginationLearningResource(int page, int limit, string? txtSearch)
        {
            IQueryable<LearningResource> query = _cmsDbContext.LearningResource;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.learning_resource_type.ToLower().Contains(txtSearch.ToLower().Trim()));
            }

            var listLearningResource = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return listLearningResource;
        }

        public int GetTotalLearningResource(string? txtSearch)
        {
            IQueryable<LearningResource> query = _cmsDbContext.LearningResource;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.learning_resource_type.ToLower().Contains(txtSearch.ToLower().Trim()));
            }

            var listLearningResource = query
                .ToList();
            return listLearningResource.Count;
        }

        public LearningResource GetLearningResourceByName(string name)
        {
            return _cmsDbContext.LearningResource.Where(r => r.learning_resource_type.Equals(name)).FirstOrDefault();
        }

        public bool CheckLearningResourceDuplicate(int id, string type)
        {
            return (_cmsDbContext.LearningResource?.Any(x => x.learning_resource_type.Equals(type) && x.learning_resource_id != id)).GetValueOrDefault();
        }

        public bool CheckLearningResourceExsit(int id)
        {
            return (_cmsDbContext.Material?.Any(x => x.learning_resource_id == id)).GetValueOrDefault();
        }

        public LearningResource GetLearningResource(int id) 
        { 
            var learningResource = _cmsDbContext.LearningResource.FirstOrDefault(x => x.learning_resource_id == id);
            return learningResource;
        }

        public string CreateLearningResource(LearningResource learningResource)
        {
            try
            {
                _cmsDbContext.LearningResource.Add(learningResource);
                _cmsDbContext.SaveChanges();
                return Result.createSuccessfull.ToString();
            }catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateLearningResource(LearningResource learningResource)
        {
            try
            {
                _cmsDbContext.LearningResource.Update(learningResource);
                _cmsDbContext.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteLearningResource(LearningResource learningResource)
        {
            try
            {
                _cmsDbContext.LearningResource.Remove(learningResource);
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
