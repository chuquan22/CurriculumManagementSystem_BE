using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.GradingCLOs
{
    public interface IGradingCLOsRepository
    {
        public GradingCLO CreateGradingCLO(GradingCLO grading);
        
    }
}
