using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CLOsDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public CLO GetCLOsById(int id)
        {
            var rs = _cmsDbContext.CLO
                .Where(c => c.syllabus_id == id)
                .FirstOrDefault();
            return rs;
        }
    }
}
