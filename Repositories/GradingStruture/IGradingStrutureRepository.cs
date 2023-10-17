using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.GradingStruture
{
    public interface IGradingStrutureRepository
    {
        public BusinessObject.GradingStruture GetGradingStruture(int id);
        public BusinessObject.GradingStruture CreateGradingStruture(BusinessObject.GradingStruture gra);
        public BusinessObject.GradingStruture UpdateGradingStruture(BusinessObject.GradingStruture gra);

        public BusinessObject.GradingStruture DeleteGradingStruture(int id);

    }
}
