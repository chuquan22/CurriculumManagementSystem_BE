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
    }
}
