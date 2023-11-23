using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Major
{
    public class MajorDAO
    {
        public CMSDbContext db = new CMSDbContext();
        public List<BusinessObject.Major> GetAllMajor()
        {
            List<BusinessObject.Major> list = new List<BusinessObject.Major>();
            list = db.Major.Include(x => x.DegreeLevel).Where(x => x.is_active == true).ToList();
            return list;
        }

        public List<BusinessObject.Major> GetMajorByDegreeLevel(int degreeId)
        {
            List<BusinessObject.Major> list = new List<BusinessObject.Major>();
            list = db.Major.Include(x => x.DegreeLevel).Include(x => x.Specialization).Where(x => x.degree_level_id == degreeId).ToList();
            return list;
        }

        public BusinessObject.Major GetMajorBySubjectId(int subjectId)
        {
            var major = db.Subject
                .Where(x => x.subject_id == subjectId)
                .Select(x => x.CurriculumSubjects
                                .Select(cs => cs.Curriculum.Specialization.Major)
                                .FirstOrDefault())
                .FirstOrDefault();

            return major;
        }

        public List<BusinessObject.Major> GetMajorByBatch(int batchId)
        {
            List<BusinessObject.Major> list = new List<BusinessObject.Major>();
            var degreeId = db.Batch.FirstOrDefault(x => x.batch_id ==  batchId).degree_level_id;
            list = GetMajorByDegreeLevel(degreeId);
            return list;
        }

        public BusinessObject.Major AddMajor(BusinessObject.Major major)
        {
            major.major_english_name = major.major_english_name.Trim();
            db.Major.Add(major);
            db.SaveChanges();
            return major;
        }

        public BusinessObject.Major EditMajor(BusinessObject.Major major)
        {
            // Check for duplicate major_name or major_english_name
            var isDuplicateCode = db.Major.Any(x => x.major_id != major.major_id && x.major_code == major.major_code);

            if (isDuplicateCode)
            {
                throw new Exception("Duplicate major code!");
            }


            var editMajor = db.Major.FirstOrDefault(x => x.major_id == major.major_id);

            if (editMajor != null)
            {
                // Update the Major object if it exists
                editMajor.major_name = major.major_name;
                editMajor.is_active = major.is_active;
                editMajor.major_english_name = major.major_english_name;          
                db.Major.Update(editMajor);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Major not found.");
            }

            return major;
        }


        public void DeleteMajor(int id)
        {
            BusinessObject.Major major = db.Major.Where(x => x.major_id == id).FirstOrDefault();
            db.Major.Remove(major);
            db.SaveChanges();
        }

        public BusinessObject.Major FindMajorById(int id)
        {
            BusinessObject.Major major = db.Major.Include(x => x.DegreeLevel).Where(x => x.major_id == id).FirstOrDefault();
            return major;
        }

        public BusinessObject.Major CheckMajorbyMajorCode(string code)
        {
            return db.Major?.Include(x =>x.DegreeLevel).FirstOrDefault(e => e.major_code.ToLower().Equals(code.ToLower()));
        }

        public BusinessObject.Major CheckMajorbyMajorName(string name)
        {
            return db.Major?.FirstOrDefault(e => e.major_name.ToLower().Equals(name.ToLower()));
        }

        public BusinessObject.Major CheckMajorbyMajorEnglishName(string eng_name)
        {
            return db.Major?.FirstOrDefault(e => e.major_english_name.ToLower().Equals(eng_name.ToLower()));
        }
    }
}
