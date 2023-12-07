using BusinessObject;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class MaterialDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<BusinessObject.Material> GetMaterial(int id)
        {
            List<BusinessObject.Material> mt = _context.Material
                .Include(x => x.LearningResource)
                .Where(x => x.syllabus_id == id).ToList();
            return mt;
        }

        public List<Material> GetMaterialListBysubject(List<Subject> subjects) 
        {
            var listSyllabus = new List<Syllabus>();
            foreach (var subject in subjects)
            {
                var ListSyllabus = _context.Syllabus.Where(x => x.subject_id == subject.subject_id && x.syllabus_approved == true && x.syllabus_status == true).ToList();
                foreach (var syllabus in ListSyllabus)
                {
                    listSyllabus.Add(syllabus);
                }
            }
            
            var listMaterials = new List<Material>();
            foreach (var syllabus in listSyllabus)
            {
                var listMaterial = _context.Syllabus
                .Include(x => x.Materials)
                .Where(x => x.syllabus_id == syllabus.syllabus_id)
                .Join(_context.Material,
                    syllabus => syllabus.syllabus_id,
                    material => material.syllabus_id,
                     (syllabus, material) => material)
                .ToList();

                foreach (var material in listMaterial)
                {
                    listMaterials.Add(material);
                }
            }

            return listMaterials;
        }

        public Material CreateMaterial(Material material)
        {
            try
            {
                _context.Material.Add(material);
                _context.SaveChanges();
                return material;
            }
            catch (Exception)
            {

                throw new Exception("Create Materials Fail!");
            }
           
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
            _context.SaveChanges();
            return oldMate;
        }
        public string DeleteMaterialBySyllabusId(int syllabus_id)
        {
            var oldMate = _context.Material.Where(a => a.syllabus_id == syllabus_id).ToList();
            foreach (var item in oldMate)
            {
                _context.Material.Remove(item);
            }
            _context.SaveChanges();
            return Result.deleteSuccessfull.ToString();
        }
        public Material GetMaterialById(int id)
        {
            var oldMate = _context.Material
                 .Include(x => x.LearningResource)
                .Where(a => a.material_id == id).FirstOrDefault();
            if(oldMate == null)
            {
                return null;
            }
            return oldMate;
        }
    }
}
