using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DAO;
namespace Repositories.GradingCLOs
{
    public class GradingCLOsRepository : IGradingCLOsRepository
    {
        public GradingCLOsDAO db = new GradingCLOsDAO();
        public GradingCLO CreateGradingCLO(GradingCLO grading)
        {
            return db.CreateGradingCLO(grading);
        }
    }
}
