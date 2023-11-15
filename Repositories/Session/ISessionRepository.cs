using DataAccess.Models.DTO.request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Session
{
    public interface ISessionRepository
    {
        public List<BusinessObject.Session> GetSession(int id);
        public BusinessObject.Session CreateSession(BusinessObject.Session session);

        public string UpdateSession(BusinessObject.Session session, List<SessionCLOsRequest> listClos);

        public string DeleteSession(int id);

        public BusinessObject.Session GetSessionById(int id);
        string UpdatePatchSession(BusinessObject.Session rs);

        public BusinessObject.Session IsSessionNoExist(int sessionNo, int scheduleId);

        public string DeleteSessionBySyllabusId(int syllabus_id);
    }
}
