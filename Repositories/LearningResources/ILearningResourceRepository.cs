using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.LearningResources
{
    public interface ILearningResourceRepository
    {
        public List<LearningResource> GetLearningResource();
        List<LearningResource> PaginationLearningResource(int page, int limit, string? txtSearch);
        bool CheckLearningResourceDuplicate(string type);
        LearningResource GetLearningResource(int id);
        string CreateLearningResource(LearningResource learningResource);
        string UpdateLearningResource(LearningResource learningResource);
        string DeleteLearningResource(LearningResource learningResource);
    }
}
