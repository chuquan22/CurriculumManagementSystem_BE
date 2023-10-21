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

        public BusinessObject.Session UpdateSession(BusinessObject.Session session);

        public BusinessObject.Session DeleteSession(int id);

        public BusinessObject.Session GetSessionById(int id);

    }
}
