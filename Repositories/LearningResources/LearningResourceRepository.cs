using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.LearningResources
{
    public class LearningResourceRepository : ILearningResourceRepository
    {
        public LearningResourceDAO db = new LearningResourceDAO();

        public bool CheckLearningResourceDuplicate(int id, string type)
        {
            return db.CheckLearningResourceDuplicate(id, type);
        }

        public bool CheckLearningResourceExsit(int id)
        {
            return db.CheckLearningResourceExsit(id);
        }

        public string CreateLearningResource(LearningResource learningResource)
        {
            return db.CreateLearningResource(learningResource);
        }

        public string DeleteLearningResource(LearningResource learningResource)
        {
            return db.DeleteLearningResource(learningResource);
        }

        public List<LearningResource> GetLearningResource()
        {
           return db.GetLearningResource(); 
        }

        public LearningResource GetLearningResource(int id)
        {
            return db.GetLearningResource(id);
        }

        public LearningResource GetLearningResourceByName(string name)
        {
            return db.GetLearningResourceByName(name);
        }

        public int GetTotalLearningResource(string? txtSearch)
        {
            return db.GetTotalLearningResource(txtSearch);
        }

        public List<LearningResource> PaginationLearningResource(int page, int limit, string? txtSearch)
        {
            return db.PaginationLearningResource(page, limit, txtSearch);
        }

        public string UpdateLearningResource(LearningResource learningResource)
        {
            return db.UpdateLearningResource(learningResource);
        }
    }
}
