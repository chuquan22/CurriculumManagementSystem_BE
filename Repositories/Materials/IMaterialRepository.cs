using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Materials
{
    public interface IMaterialRepository
    {
        public List<BusinessObject.Material> GetMaterial(int id);
        public BusinessObject.Material CreateMaterial(BusinessObject.Material mate);
        public BusinessObject.Material EditMaterial(BusinessObject.Material mate);
        public BusinessObject.Material DeleteMaterial(int id);

    }
}
