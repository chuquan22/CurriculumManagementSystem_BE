using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.GradingStruture
{
    public interface IGradingStrutureRepository
    {
        public List<BusinessObject.GradingStruture> GetGradingStruture(int id);
        public BusinessObject.GradingStruture CreateGradingStruture(BusinessObject.GradingStruture gra);
        public string UpdateGradingStruture(BusinessObject.GradingStruture gra, List<int> listClo);

        public BusinessObject.GradingStruture DeleteGradingStruture(int id);
        BusinessObject.GradingStruture GetGradingStrutureById(int id);
        string DeleteGradingStrutureBySyllabusId(int syllabusId);
    }
}
