using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.LearningResources
{
    public class LearningResourceRepository : ILearningResourceRepository
    {
        public LearningResourceDAO db = new LearningResourceDAO();
        public List<LearningResource> GetLearningResource()
        {
           return db.GetLearningResource(); 
        }
    }
}
