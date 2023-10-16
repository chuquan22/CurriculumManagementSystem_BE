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
            Material mt = _context.Material.Where(x => x.syllabus_id == id).FirstOrDefault();
            return mt;
        }

        public Material CreateMaterial(Material material)
        {
            _context.Material.Add(material);
            _context.SaveChanges();
            return material;
        }

        public Material EditMaterial(Material material)
        {
            var oldMate = _context.Material.Where(a => a.material_id == material.material_id).FirstOrDefault();
            if (oldMate != null)
            {
                oldMate.material_description = material.material_description;
                oldMate.material_purpose = material.material_purpose;
                oldMate.material_ISBN = material.material_ISBN;
                oldMate.syllabus_id = material.syllabus_id;
                oldMate.material_note = material.material_note;
                oldMate.learning_resource_id = material.learning_resource_id;
                oldMate.material_author = material.material_author;
                oldMate.material_publisher = material.material_publisher;
                oldMate.material_published_date = material.material_published_date;
                oldMate.material_edition = material.material_edition;
            }
            _context.SaveChanges();
            return material;
        }

        public Material DeleteMaterial(int id)
        {
            var oldMate = _context.Material.Where(a => a.material_id == id).FirstOrDefault();
            _context.Material.Remove(oldMate);
            return oldMate;
        }
    }
}
