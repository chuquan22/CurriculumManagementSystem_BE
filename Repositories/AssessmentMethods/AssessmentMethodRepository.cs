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

        public bool CheckAssmentMethodDuplicate(int id, string assmentMethod, int type)
        {
            return assessmentMethodDAO.CheckAssmentMethodDuplicate(id, assmentMethod, type);
        }

        public bool CheckAssmentMethodExsit(int id)
        {
            return assessmentMethodDAO.CheckAssmentMethodExsit(id);
        }

        public string CreateAssessmentMethod(AssessmentMethod method)
        {
            return assessmentMethodDAO.CreateAssessmentMethod(method);
        }

        public string DeleteAssessmentMethod(AssessmentMethod method)
        {
            return assessmentMethodDAO.DeleteAssessmentMethod(method);
        }

        public List<AssessmentMethod> GetAllAssessmentMethod()
        {
            return assessmentMethodDAO.GetAllAssessmentMethod();
        }

      

        public AssessmentMethod GetAssessmentMethodByNameAndTypeId(string name, int id)
        {
            return assessmentMethodDAO.GetAsssentMethodByName(name,id);

        }

        public AssessmentMethod GetAsssentMethodById(int id)
        {
            return assessmentMethodDAO.GetAsssentMethodById(id);
        }

        public int GetTotalAssessmentMethod(string? txtSearch)
        {
            return assessmentMethodDAO.GetTotalAssessmentMethod(txtSearch);
        }

        public List<AssessmentMethod> PaginationAssessmentMethod(int page, int limit, string? txtSearch)
        {
            return assessmentMethodDAO.PaginationAssessmentMethod(page, limit, txtSearch);
        }

        public string UpdateAssessmentMethod(AssessmentMethod method)
        {
            return assessmentMethodDAO.UpdateAssessmentMethod(method);
        }
    }
}
