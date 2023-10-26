using DataAccess.DAO;
using DataAccess.Models.DTO.request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Session
{

    public class SessionRepository : ISessionRepository
    {
        private readonly SessionDAO  db = new SessionDAO();

        public BusinessObject.Session CreateSession(BusinessObject.Session session)
        {
            return db.CreateSession(session);
        }

        public string DeleteSession(int id)
        {
            return db.DeleteSession(id);
        }

        public List<BusinessObject.Session> GetSession(int id)
        {
            return db.GetSession(id);
        }

        public BusinessObject.Session GetSessionById(int id)
        {
            return db.GetSessionById(id);

        }

        public string UpdatePatchSession(BusinessObject.Session rs)
        {
            return db.UpdatePatchSession(rs);
        }

        public string UpdateSession(BusinessObject.Session session, List<SessionCLOsRequest> listClos)
        {
            return db.UpdateSession(session,listClos);
        }
    }
}
