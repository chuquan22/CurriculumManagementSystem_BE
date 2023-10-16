using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.GradingStruture
{
    public class GradingStrutureRepository : IGradingStrutureRepository
    {
        public GradingStrutureDAO db = new GradingStrutureDAO();


        public BusinessObject.GradingStruture GetGradingStruture(int id)
        {
           return db.GetGradingStruture(id);
        }
    }
}
