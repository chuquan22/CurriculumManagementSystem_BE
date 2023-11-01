using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PLOS
{
    public class PLOsRepository : IPLOsRepository
    {
        private PLOsDAO _plosDAO = new PLOsDAO();

        public bool CheckPLONameExsit(string ploName, int curriId)
        {
            return _plosDAO.CheckPLONameExsit(ploName, curriId);
        }

        public string CreatePLOs(PLOs plo)
        {
            return _plosDAO.CreatePLOs(plo);
        }

        public string DeletePLOs(PLOs plo)
        {
            return _plosDAO.DeletePLOs(plo);
        }

        public List<PLOs> GetListPLOsByCurriculum(int curriculumId)
        {
            return _plosDAO.GetListPLOsByCurriculum(curriculumId);
        }

        public PLOs GetPLOsById(int id)
        {
            return _plosDAO.GetPLOsById(id);
        }

       

        public PLOs GetPLOsByName(string ploName, int curriId)
        {
            return _plosDAO.GetPLOsByName(ploName, curriId);
        }

        public string UpdatePLOs(PLOs plo)
        {
            return _plosDAO.UpdatePLOs(plo);
        }
    }
}
