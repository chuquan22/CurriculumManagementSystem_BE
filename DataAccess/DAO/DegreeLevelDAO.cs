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

        public int GetDegreeIdByBatch(int bacthId)
        {
            var id = _cmsDbContext.Semester.FirstOrDefault(x => x.batch_id == bacthId).degree_level_id;
            return id;
        }
    }
}
