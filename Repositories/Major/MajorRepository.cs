using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Major;

namespace Repositories.Major
{
    public class MajorRepository : IMajorRepository
    {
        public MajorDAO db = new MajorDAO();

        public BusinessObject.Major AddMajor(BusinessObject.Major major)
        {
            return db.AddMajor(major);
        }

        public BusinessObject.Major CheckMajorbyMajorCode(string code)
        {
            return db.CheckMajorbyMajorCode(code);
        }

        public BusinessObject.Major CheckMajorbyMajorEnglishName(string eng_name)
        {
            return db.CheckMajorbyMajorEnglishName(eng_name);
        }

        public BusinessObject.Major CheckMajorbyMajorName(string name)
        {
            return db.CheckMajorbyMajorName(name);
        }

        public void DeleteMajor(int id)
        {
            db.DeleteMajor(id);
        }

        public BusinessObject.Major EditMajor(BusinessObject.Major major)
        {
            return db.EditMajor(major);
        }

        public BusinessObject.Major FindMajorById(int majorId)
        {
            return db.FindMajorById(majorId);
        }

        public List<BusinessObject.Major> GetAllMajor()
        {
            return db.GetAllMajor();
        }

        public List<BusinessObject.Major> GetMajorByBatch(int batchId)
        {
            return db.GetMajorByBatch(batchId);
        }

        public List<BusinessObject.Major> GetMajorByDegreeLevel(int degreeId)
        {
            return db.GetMajorByDegreeLevel(degreeId);
        }
    }
}
