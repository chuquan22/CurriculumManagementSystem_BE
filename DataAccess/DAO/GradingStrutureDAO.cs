using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class GradingStrutureDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public GradingStruture GetGradingStruture(int id)
        {
            var rs = _cmsDbContext.GradingStruture
                .Where(c => c.syllabus_id == id)
                .FirstOrDefault();
            return rs;
        }
    }
}
