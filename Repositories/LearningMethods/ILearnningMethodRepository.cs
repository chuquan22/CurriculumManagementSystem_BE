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
        LearningMethod GetLearningMethodById(int id);
        bool CheckLearningMethodDuplicate(string learning_method_name);
        string CreateLearningMethod(LearningMethod learningMethod);
        string UpdateLearningMethod(LearningMethod learningMethod);
        string DeleteLearningMethod(LearningMethod learningMethod);
    }
}
