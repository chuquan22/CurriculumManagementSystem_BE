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

        public BusinessObject.GradingStruture CreateGradingStruture(BusinessObject.GradingStruture gra)
        {
            return db.CreateGradingStruture(gra);
        }

        public BusinessObject.GradingStruture DeleteGradingStruture(int id)
        {
            return db.DeleteGradingStruture(id);
        }

        public BusinessObject.GradingStruture GetGradingStruture(int id)
        {
           return db.GetGradingStruture(id);
        }

        public BusinessObject.GradingStruture UpdateGradingStruture(BusinessObject.GradingStruture gra)
        {
            return db.UpdateGradingStruture(gra);
        }
    }
}
