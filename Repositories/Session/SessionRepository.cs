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
        public List<BusinessObject.Session> GetSession(int id)
        {
            return db.GetSession(id);  
        }
    }
}
