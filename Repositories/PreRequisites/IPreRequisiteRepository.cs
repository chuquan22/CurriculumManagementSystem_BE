using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PreRequisites
{
    public interface IPreRequisiteRepository
    {
        List<PreRequisite> GetAllPreRequisite();
        List<PreRequisite> GetPreRequisitesBySubject(int subjectId);
        PreRequisite GetPreRequisite(int subjectId, int preSubjectId);
        string CreatePreRequisite(PreRequisite preRequisite);
        string UpdatePreRequisite(PreRequisite preRequisite);
        string DeletePreRequisite(PreRequisite preRequisite);
    }
}
