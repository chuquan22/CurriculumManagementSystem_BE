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

        public bool CheckLearningResourceDuplicate(string type)
        {
            return db.CheckLearningResourceDuplicate(type);
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

        public List<LearningResource> PaginationLearningResource(int page, int limit, string? txtSearch)
        {
            return db.PaginationLearningResource(page, limit, txtSearch);
        }

        public string UpdateLearningResource(LearningResource learningResource)
        {
            return db.DeleteLearningResource(learningResource);
        }
    }
}
