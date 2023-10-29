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
        public AssessmentMethod GetAssessmentMethodByNameAndTypeId(string name,int id);
        string CreateAssessmentMethod(AssessmentMethod method);
        string UpdateAssessmentMethod(AssessmentMethod method);
        string DeleteAssessmentMethod(AssessmentMethod method);
        bool CheckAssmentMethodDuplicate(string  assmentMethod);
        AssessmentMethod GetAsssentMethodById(int id);

    }
}
