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
        public List<LearningMethod> GetAllLearningMethods()
        {
            return _learningMethodDAO.GetAllLearningMethods();
        }
    }
}
