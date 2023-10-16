using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Materials
{
    public interface IMaterialRepository
    {
        public BusinessObject.Material GetMaterial(int id);
    }
}
