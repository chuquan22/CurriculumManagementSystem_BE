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

        public AssessmentType GetAssessmentTypeByName(string name);
    }
}
