using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class GradingCLOsDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();
        public GradingCLO CreateGradingCLO(GradingCLO clo)
        {
            _cmsDbContext.GradingCLO.Add(clo);
            _cmsDbContext.SaveChanges();
            return clo;
        }
    }
}
