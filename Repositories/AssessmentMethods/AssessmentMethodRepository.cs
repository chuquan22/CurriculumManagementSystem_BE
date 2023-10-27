using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.AssessmentMethods
{
    public class AssessmentMethodRepository : IAssessmentMethodRepository
    {
        private readonly AssessmentMethodDAO assessmentMethodDAO = new AssessmentMethodDAO();
        public List<AssessmentMethod> GetAllAssessmentMethod()
        {
            return assessmentMethodDAO.GetAllAssessmentMethod();
        }

        public AssessmentMethod GetAssessmentMethodByName(string name)
        {
            return assessmentMethodDAO.GetAsssentMethodByName(name);

        }
    }
}
