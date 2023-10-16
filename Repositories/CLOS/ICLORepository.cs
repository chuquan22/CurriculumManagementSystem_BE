using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CLOS
{
    public interface ICLORepository
    {
        public BusinessObject.CLO GetCLOsById(int id);

        public BusinessObject.CLO CreateCLOs(BusinessObject.CLO clo);

        public BusinessObject.CLO UpdateCLOs(BusinessObject.CLO clo);

        public BusinessObject.CLO DeleteCLOs(int id);
    }
}
