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


        public DegreeLevel GetDegreeLevelByID(int id)
        {
            return _cmsDbContext.DegreeLevel.Where(d => d.degree_level_id == id).FirstOrDefault();
        }

        public DegreeLevel GetDegreeLevelByEnglishName(string name)
        {
            return _cmsDbContext.DegreeLevel.Where(d => d.degree_level_english_name.Equals(name)).FirstOrDefault();
        }

        public DegreeLevel GetDegreeLevelByVietnameseName(string name)
        {
            return _cmsDbContext.DegreeLevel.Where(d => d.degree_level_name.Equals(name)).FirstOrDefault();

        }
        public int GetDegreeIdByBatch(int bacthId)
        {
            var id = _cmsDbContext.Semester.FirstOrDefault(x => x.start_batch_id == bacthId).degree_level_id;
            return id;

        }
    }
}
