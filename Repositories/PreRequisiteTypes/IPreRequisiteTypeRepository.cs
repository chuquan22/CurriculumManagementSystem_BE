using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PreRequisiteTypes
{
    public interface IPreRequisiteTypeRepository
    {
        List<PreRequisiteType> GetAllPreRequisiteTypes();
        PreRequisiteType GetPreRequisiteType(int id);
        string CreatePreRequisiteType(PreRequisiteType preRequisiteType);
        string UpdatePreRequisiteType(PreRequisiteType preRequisiteType);
        string DeletePreRequisiteType(PreRequisiteType preRequisiteType);
    }
}
