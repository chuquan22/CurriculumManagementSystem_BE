using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SessionDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<Session> GetSession(int id)
        {
            var rs = _cmsDbContext.Session
                .Where(c => c.syllabus_id == id)
                .ToList();
            return rs;
        }

        public Session CreateSession(Session session)
        {
            _cmsDbContext.Session.Add(session); 
            _cmsDbContext.SaveChanges();
            return session;
        }

        public Session DeleteSession(int id)
        {
            var rs = _cmsDbContext.Session
              .Where(c => c.syllabus_id == id)
              .ToList();
            return rs;
        }

        public Session UpdateSession(Session session)
        {
            throw new NotImplementedException();
        }

        public Session GetSessionById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
