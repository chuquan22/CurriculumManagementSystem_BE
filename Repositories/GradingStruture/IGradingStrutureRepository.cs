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
    }
}
