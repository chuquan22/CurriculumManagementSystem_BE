﻿using BusinessObject;
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

        DegreeLevel GetDegreeLevelByID(int id);
        DegreeLevel GetDegreeLevelByEnglishName(string name);

        DegreeLevel GetDegreeLevelByName(string name);
        int GetDegreeIdByBatch(int bacthId);

    }
}
