using BusinessObject;
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
        bool CheckPLONameExsit(string ploName, int curriId);
        PLOs GetPLOsByName(string ploName, int curriId);
        string CreatePLOs(PLOs plo);
        string UpdatePLOs(PLOs plo);
        string DeletePLOs(PLOs plo);

    }
}
