using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.AssessmentMethods
{
    public interface IAssessmentMethodRepository
    {
        List<AssessmentMethod> GetAllAssessmentMethod();
        List<AssessmentMethod> PaginationAssessmentMethod(int page, int limit, string? txtSearch);
        public AssessmentMethod GetAssessmentMethodByNameAndTypeId(string name,int id);
        int GetTotalAssessmentMethod(string? txtSearch);
        string CreateAssessmentMethod(AssessmentMethod method);
        string UpdateAssessmentMethod(AssessmentMethod method);
        string DeleteAssessmentMethod(AssessmentMethod method);
        bool CheckAssmentMethodDuplicate(string assmentMethod);
        bool CheckAssmentMethodExsit(int id);
        AssessmentMethod GetAsssentMethodById(int id);

    }
}
