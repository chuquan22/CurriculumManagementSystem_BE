using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Materials
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly MaterialDAO db = new MaterialDAO();
        public Material GetMaterial(int id)
        {
            return db.GetMaterial(id);
        }
    }
}
