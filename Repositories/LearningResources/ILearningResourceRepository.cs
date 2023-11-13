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

        LearningResource GetLearningResourceByName(string name);   
        List<LearningResource> PaginationLearningResource(int page, int limit, string? txtSearch);
        bool CheckLearningResourceDuplicate(int id, string type);
        int GetTotalLearningResource(string? txtSearch);
        LearningResource GetLearningResource(int id);
        bool CheckLearningResourceExsit(int id);
        string CreateLearningResource(LearningResource learningResource);
        string UpdateLearningResource(LearningResource learningResource);
        string DeleteLearningResource(LearningResource learningResource);
    }
}
