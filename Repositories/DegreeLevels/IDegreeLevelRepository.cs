using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DegreeLevels
{
    public interface IDegreeLevelRepository
    {
        List<DegreeLevel> GetAllDegreeLevel();
        int GetDegreeIdByBatch(int bacthId);
    }
}
