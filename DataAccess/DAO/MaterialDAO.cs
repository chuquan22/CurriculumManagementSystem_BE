using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class MaterialDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public Material GetMaterial(int id)
        {
            var mt = _context.Material.Where(x => x.syllabus_id == id).FirstOrDefault();
            return mt;
        }
    }
}
