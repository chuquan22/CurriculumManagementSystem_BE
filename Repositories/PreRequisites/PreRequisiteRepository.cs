using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PreRequisites
{
    public class PreRequisiteRepository : IPreRequisiteRepository
    {
        private PreRequisiteDAO _preRequisiteDAO= new PreRequisiteDAO();
        public string CreatePreRequisite(PreRequisite preRequisite)
        {
            return _preRequisiteDAO.CreatePreRequisite(preRequisite);
        }

        public string DeletePreRequisite(PreRequisite preRequisite)
        {
            return _preRequisiteDAO.DeletePreRequisite(preRequisite) ;
        }

        public PreRequisite GetPreRequisite(int subjectId, int preSubjectId)
        {
            return _preRequisiteDAO.GetPreRequisite(subjectId, preSubjectId) ;
        }

        public List<PreRequisite> GetPreRequisitesBySubject(int subjectId)
        {
            return _preRequisiteDAO.GetPreRequisiteBySubject(subjectId) ;
        }

        public string UpdatePreRequisite(PreRequisite preRequisite)
        {
            return _preRequisiteDAO.UpdatePreRequisite(preRequisite);
        }
    }
}
