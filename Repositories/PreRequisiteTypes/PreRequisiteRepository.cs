using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PreRequisiteTypes
{
    public class PreRequisiteRepository : IPreRequisiteTypeRepository
    {
        public readonly PreRequisiteTypeDAO _preRequisiteTypeDAO = new PreRequisiteTypeDAO();
        public string CreatePreRequisiteType(PreRequisiteType preRequisiteType)
        {
            return _preRequisiteTypeDAO.CreatePreRequisiteType(preRequisiteType);
        }

        public string DeletePreRequisiteType(PreRequisiteType preRequisiteType)
        {
            return _preRequisiteTypeDAO.DeletePreRequisiteType(preRequisiteType);
        }

        public List<PreRequisiteType> GetAllPreRequisiteTypes()
        {
            return _preRequisiteTypeDAO.ListPreRequisiteTypes();
        }

        public PreRequisiteType GetPreRequisiteType(int id)
        {
            return _preRequisiteTypeDAO.GetPreRequisiteType(id);
        }

        public string UpdatePreRequisiteType(PreRequisiteType preRequisiteType)
        {
            return _preRequisiteTypeDAO.UpdatePreRequisiteType(preRequisiteType);
        }
    }
}
