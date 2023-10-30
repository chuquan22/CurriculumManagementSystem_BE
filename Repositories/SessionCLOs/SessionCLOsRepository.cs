using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SessionCLOs
{
    public class SessionCLOsRepository : ISessionCLOsRepository
    {
        public SessionCLOsDAO db = new SessionCLOsDAO();
        public SessionCLO CreateSessionCLO(SessionCLO sessionCLO)
        {
            return db.CreateSessionCLO(sessionCLO);
        }
    }
}
