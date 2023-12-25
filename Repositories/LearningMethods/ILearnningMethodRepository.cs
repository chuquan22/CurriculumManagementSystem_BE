using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.LearningMethods
{
    public interface ILearnningMethodRepository
    {
        List<LearningMethod> GetAllLearningMethods();
        List<LearningMethod> PaginationLearningMethod(int page, int limit, string? txtSearch);
        int GetTotalLearningMethod(string? txtSearch);
        LearningMethod GetLearningMethodById(int id);
        LearningMethod GetLearningMethodByName(string name);
        bool CheckLearningMethodDuplicate(int id, string learning_method_name);
        bool CheckLearningMethodExsit(int id);
        string CreateLearningMethod(LearningMethod learningMethod);
        string UpdateLearningMethod(LearningMethod learningMethod);
        string DeleteLearningMethod(LearningMethod learningMethod);
    }
}
