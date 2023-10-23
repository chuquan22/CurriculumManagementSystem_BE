using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SessionCLOsDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public SessionCLO CreateSessionCLO(SessionCLO sessionCLO)
        {
            _context.SessionCLO.Add(sessionCLO);
            _context.SaveChanges(); 
            return sessionCLO;
        }

       
    }
}
