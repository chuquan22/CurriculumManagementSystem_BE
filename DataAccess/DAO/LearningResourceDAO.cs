using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class LearningResourceDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<LearningResource> GetLearningResource()
        {
            var rs = _cmsDbContext.LearningResource
               
                .ToList();
            return rs;
        }

    }
}
