using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Session
{
    public interface ISessionRepository
    {
        public BusinessObject.Session GetSession(int id);   
    }
}
