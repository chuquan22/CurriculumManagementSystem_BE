using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PLOMappings
{
    public interface IPLOMappingRepository
    {
        List<PLOMapping> GetPLOMappingsInCurriculum(int curriculumId);
        string CreatePLOMapping(PLOMapping ploMapping);
        string UpdatePLOMapping(PLOMapping ploMapping);
    }
}
