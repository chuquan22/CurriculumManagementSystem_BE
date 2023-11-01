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
        List<PreRequisiteType> PaginationPreRequisiteType(int page, int limit, string? txtSearch);
        int GetTotalPreRequisite(string? txtSearch);
        bool CheckPreRequisiteTypeDuplicate(int id, string name);
        bool CheckPreRequisiteTypeExsit(int id);
        PreRequisiteType GetPreRequisiteType(int id);
        string CreatePreRequisiteType(PreRequisiteType preRequisiteType);
        string UpdatePreRequisiteType(PreRequisiteType preRequisiteType);
        string DeletePreRequisiteType(PreRequisiteType preRequisiteType);
    }
}
