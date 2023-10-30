using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PLOMappings
{
    public class PLOMappingRepository : IPLOMappingRepository
    {
        private readonly PLOMappingDAO pLOMappingDAO = new PLOMappingDAO();

        public bool CheckPLOMappingExsit(int subjectId, int ploId)
        {
            return pLOMappingDAO.CheckPLOMappingExsit(subjectId, ploId);
        }

        public string CreatePLOMapping(PLOMapping ploMapping)
        {
            return pLOMappingDAO.CreatePLOMapping(ploMapping);
        }

        public string DeletePLOMapping(PLOMapping ploMapping)
        {
            return pLOMappingDAO.DeletePLOMapping(ploMapping);
        }

        public PLOMapping GetPLOMappingExsit(int subjectId, int ploId)
        {
            return pLOMappingDAO.GetPLOMappingExsit(subjectId, ploId);
        }

        public List<PLOMapping> GetPLOMappingsInCurriculum(int curriculumId)
        {
            return pLOMappingDAO.GetPLOMappingsInCurriculum(curriculumId);
        }

        public string UpdatePLOMapping(PLOMapping ploMapping)
        {
            return pLOMappingDAO.UpdatePLOMapping(ploMapping);
        }
    }
}
