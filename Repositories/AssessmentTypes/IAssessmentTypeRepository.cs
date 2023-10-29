using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.AssessmentTypes
{
    public interface IAssessmentTypeRepository
    {
        public List<AssessmentType> GetAllAssessmentType();
        AssessmentType GetAsssentTypeById(int id);
        public AssessmentType GetAssessmentTypeByName(string name);
        bool CheckAssmentTypeDuplicate(string name);
        string CreateAssessmentType(AssessmentType type);
        string UpdateAssessmentType(AssessmentType type);
        string DeleteAssessmentType(AssessmentType type);

    }
}
