﻿using System;
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
            list = db.Major.ToList();
            return list;
        }

        public BusinessObject.Major AddMajor(BusinessObject.Major major)
        {
            db.Major.Add(major);
            db.SaveChanges();
            return major;
        }

        public BusinessObject.Major EditMajor(BusinessObject.Major major)
        {
            var editMajor = db.Major.FirstOrDefault(x => x.major_id == major.major_id);
            editMajor.major_name = major.major_name;
            editMajor.is_active = major.is_active;
            //editMajor.major_code = major.major_code;
            editMajor.major_english_name = major.major_english_name;
            db.Major.Update(editMajor); 
            db.SaveChanges();
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
            BusinessObject.Major major = db.Major.Where(x => x.major_id == id).FirstOrDefault();
            return major;
        }

        public BusinessObject.Major CheckMajorbyMajorCode(string code)
        {
            return db.Major?.FirstOrDefault(e => e.major_code.Equals(code));
        }
    }
}
