using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SessionCLOs
{
    public interface ISessionCLOsRepository
    {
        public SessionCLO CreateSessionCLO(SessionCLO sessionCLO);
    }
}
