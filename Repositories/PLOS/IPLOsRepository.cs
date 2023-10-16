﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PLOS
{
    public interface IPLOsRepository
    {
        List<PLOs> GetListPLOsByCurriculum(int curriculumId);
        PLOs GetPLOsById(int id);
        string CreatePLOs(PLOs plo);
        string UpdatePLOs(PLOs plo);
        string DeletePLOs(PLOs plo);

    }
}