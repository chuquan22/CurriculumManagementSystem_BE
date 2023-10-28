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
        public AssessmentMethod GetAssessmentMethodByName(string name);
        string CreateAssessmentMethod(AssessmentMethod method);
        string UpdateAssessmentMethod(AssessmentMethod method);
        string DeleteAssessmentMethod(AssessmentMethod method);


    }
}
