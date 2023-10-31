using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.LearningMethods
{
    public class LearningMethodRepository : ILearnningMethodRepository
    {
        private readonly LearningMethodDAO _learningMethodDAO = new LearningMethodDAO();

        public bool CheckLearningMethodDuplicate(string learning_method_name)
        {
            return _learningMethodDAO.CheckLearningMethodDuplicate(learning_method_name);
        }

        public string CreateLearningMethod(LearningMethod learningMethod)
        {
            return _learningMethodDAO.CreateLearningMethod(learningMethod);
        }

        public string DeleteLearningMethod(LearningMethod learningMethod)
        {
            return _learningMethodDAO.DeleteLearningMethod(learningMethod);
        }

        public List<LearningMethod> GetAllLearningMethods()
        {
            return _learningMethodDAO.GetAllLearningMethods();
        }

        public LearningMethod GetLearningMethodById(int id)
        {
            return _learningMethodDAO.GetLearningMethodById(id);
        }

        public List<LearningMethod> PaginationLearningMethod(int page, int limit, string? txtSearch)
        {
            return _learningMethodDAO.PaginationLearningMethod(page, limit, txtSearch);
        }

        public string UpdateLearningMethod(LearningMethod learningMethod)
        {
            return _learningMethodDAO.UpdateLearningMethod(learningMethod);
        }
    }
}
