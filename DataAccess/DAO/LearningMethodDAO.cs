using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class LearningMethodDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<LearningMethod> GetAllLearningMethods()
        {
            var listLearningMethod = _cmsDbContext.LearningMethod.ToList();
            return listLearningMethod;
        }
    }
}
