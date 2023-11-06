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

        public Material CreateMaterial(Material mate)
        {
            return db.CreateMaterial(mate);
        }

        public Material DeleteMaterial(int id)
        {
           return db.DeleteMaterial(id);
        }

        public Material EditMaterial(Material mate)
        {
            return db.EditMaterial(mate);
        }

        public List<BusinessObject.Material> GetMaterial(int id)
        {
            return db.GetMaterial(id);
        }

        public Material GetMaterialById(int id)
        {
            return db.GetMaterialById(id);
        }

        public List<Material> GetMaterialListBysubject(int sid)
        {
            return db.GetMaterialListBysubject(sid);
        }
    }
}
