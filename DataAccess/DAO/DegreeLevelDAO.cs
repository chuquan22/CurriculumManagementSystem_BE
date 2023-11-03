using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class DegreeLevelDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<DegreeLevel> GetAllDegreeLevel()
        {
            var listDegreeLevel = _cmsDbContext.DegreeLevel.ToList();
            return listDegreeLevel;
        }
    }
}
