using DataAccess.DAO;
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

        public BusinessObject.Session DeleteSession(int id)
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

        public BusinessObject.Session UpdateSession(BusinessObject.Session session)
        {
            return db.UpdateSession(session);

        }
    }
}
