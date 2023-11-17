using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DegreeLevels
{
    public class DegreeLevelRepository : IDegreeLevelRepository
    {
        public readonly DegreeLevelDAO degreeLevelDAO = new DegreeLevelDAO();
        public List<DegreeLevel> GetAllDegreeLevel()
        {
            return degreeLevelDAO.GetAllDegreeLevel();
        }

        public DegreeLevel GetDegreeLevelByEnglishName(string name)
        {
            return degreeLevelDAO.GetDegreeLevelByEnglishName(name);
        }

        public DegreeLevel GetDegreeLevelByID(int name)
        {
            return degreeLevelDAO.GetDegreeLevelByID(name);
        }

        public DegreeLevel GetDegreeLevelByName(string name)
        {
            return degreeLevelDAO.GetDegreeLevelByName(name);
        }

        public int GetDegreeIdByBatch(int bacthId)
        {
            return degreeLevelDAO.GetDegreeIdByBatch(bacthId);

        }
    }
}
