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

        public Session GetSession(int id)
        {
            var rs = _cmsDbContext.Session
                .Where(c => c.syllabus_id == id)
                .FirstOrDefault();
            return rs;
        }
    }
}
