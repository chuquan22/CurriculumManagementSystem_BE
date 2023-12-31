﻿using BusinessObject;
using DataAccess.Combos;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Specialization
{
    public class SpecializationDAO
    {
        public CMSDbContext db = new CMSDbContext();

        public List<BusinessObject.Specialization> GetSpec()
        {
            List<BusinessObject.Specialization> list = new List<BusinessObject.Specialization>();
            try
            {
               list = db.Specialization.Include(x => x.Major.DegreeLevel).ToList();              
            }
            catch (Exception)
            {
                
            }
            finally
            {

            }
            return list;
        }
        public List<BusinessObject.Specialization> GetSpecByPagging(int degree_id, int page, int limit, string txtSearch, string majorId)
        {
            List<BusinessObject.Specialization> rs = new List<BusinessObject.Specialization>();
            try
            {
                rs = db.Specialization.Include(x => x.Major).ThenInclude(x => x.DegreeLevel).Where(x => x.Major.degree_level_id == degree_id).ToList();
                if (!string.IsNullOrEmpty(txtSearch))
                {
                    rs = rs.Where(sy => sy.specialization_name.Contains(txtSearch)
                    || sy.specialization_code.Contains(txtSearch)
                    || sy.specialization_english_name.Contains(txtSearch)
                    ).ToList();
                }
                if (!string.IsNullOrEmpty(majorId))
                {
                    int m = int.Parse(majorId);

                    rs = rs.Where(sy => sy.major_id == m).ToList();
                }
                rs = rs
                .Skip((page - 1) * limit).Take(limit).ToList();
            }
            catch (Exception)
            {

            }
            return rs;
        }

        public BusinessObject.Specialization GetSpeById(int speId)
        {
            var spe = db.Specialization.Include(x => x.Semester.Batch).Where(x => x.specialization_id == speId).ToList().FirstOrDefault();
            return spe;
        }

        public List<BusinessObject.Specialization> GetSpeByMajorId(int majorId, string batch_name)
        {
            var listSpe = new List<BusinessObject.Specialization>();
            var spe = db.Specialization.Include(x => x.Semester.Batch).Where(x => x.major_id == majorId && x.is_active == true).ToList();
            foreach (var item in spe)
            {
                if (double.Parse(item.Semester.Batch.batch_name) <= double.Parse(batch_name))
                {
                    listSpe.Add(item);
                }
            }
            return listSpe;
        }

        public List<BusinessObject.Specialization> GetSpeByBatchId(int batchId)
        {
            var listSpe = new List<BusinessObject.Specialization>();
            var batch = db.Batch.FirstOrDefault(x => x.batch_id == batchId);
            var major = db.Major.Where(x => x.degree_level_id == batch.degree_level_id && x.is_active == true).ToList();

            foreach (var item in major)
            {
                var spe = db.Specialization.Where(x => x.major_id == item.major_id  && x.is_active == true).ToList();
                foreach (var item1 in spe)
                {
                    listSpe.Add(item1);
                }
            }
            return listSpe;
        }

        public List<BusinessObject.Specialization> GetSpeByBatchId(int batchId, int majorId)
        {
            var bacth = db.Batch.FirstOrDefault(x => x.batch_id == batchId);
            var listSpe = new List<BusinessObject.Specialization>();
            var spe = db.Specialization.Include(x => x.Semester.Batch).Where(x => x.major_id == majorId && x.is_active == true).ToList();
            foreach (var item in spe)
            {
                if (double.Parse(item.Semester.Batch.batch_name) <= double.Parse(bacth.batch_name))
                {
                    listSpe.Add(item);
                }
            }
            return listSpe;
        }

        public bool IsCodeExist(string code)
        {
            var spe = db.Specialization.Where(x => x.specialization_code.Equals(code)).ToList().FirstOrDefault();
            if (spe != null)
            {
                return true;
            }        
            return false;
        }

        public int GetTotalSpecialization(int degree_id, string txtSearch, string majorId)
        {
            int rs = 0;
            using (var context = new CMSDbContext())
            {
                var query = context.Specialization.Include(x => x.Major).Where(x => x.Major.degree_level_id == degree_id).AsQueryable(); 

                if (!string.IsNullOrEmpty(txtSearch))
                {
                    query = query.Where(sy => sy.specialization_name.Contains(txtSearch)
                        || sy.specialization_code.Contains(txtSearch)
                        || sy.specialization_english_name.Contains(txtSearch));
                }

                if (!string.IsNullOrEmpty(majorId))
                {
                    int m = int.Parse(majorId);
                    query = query.Where(sy => sy.major_id == m);
                }

                rs = query.Count(); 
            }

            return rs;
        }

        public BusinessObject.Specialization FindSpeById(int id)
        {
            var specialization = new BusinessObject.Specialization();
            specialization = db.Specialization.FirstOrDefault(x => x.specialization_id == id);
            return specialization;
        }

        public BusinessObject.Specialization CreateSpecialization(BusinessObject.Specialization spe)
        {
            try
            {
                spe.specialization_english_name = spe.specialization_english_name.Trim();
                db.Specialization.Add(spe);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            finally
            {

            }
            return spe;
        }
        public BusinessObject.Specialization UpdateSpecialization(BusinessObject.Specialization speEdit)
        {
            BusinessObject.Specialization spe = new BusinessObject.Specialization();
            try
            {
                spe = db.Specialization.Where(x => x.specialization_id == speEdit.specialization_id).ToList().FirstOrDefault();
                if(spe != null)
                {
                    spe.specialization_name = speEdit.specialization_name;
                    spe.specialization_english_name = speEdit.specialization_english_name;
                    spe.major_id = speEdit.major_id;
                    spe.semester_id = speEdit.semester_id;
                    spe.is_active = speEdit.is_active;
                    db.Specialization.Update(spe);
                    db.SaveChanges();
                }
              
            }
            catch (Exception)
            {

            }
            finally
            {

            }
            return spe;
        }

        public string DeleteSpecialization(int id)
        {
            if (CheckSpeExistInCurriculum(id))
            {
                return null;
            }
            ComboDAO comboDAO = new ComboDAO();
            BusinessObject.Specialization spe = new BusinessObject.Specialization();
            try
            {
                spe = db.Specialization.Where(x => x.specialization_id == id).ToList().FirstOrDefault();
                var combo = comboDAO.GetListCombo(spe.specialization_id);
                foreach (var item in combo)
                {
                    comboDAO.DeleteCombo(item.combo_id);
                }
                if (spe != null)
                {
                    db.Specialization.Remove(spe);
                    db.SaveChanges();
                    return "Delete Specialization Sucessfully!";
                }
                else
                {
                    return "Specialization is not exist in system!";
                }
                 
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;

            }
            finally
            {

            }
            return "Delete false";
        }


        public int GetSpecializationIdByCode(string spe_code)
        {
            int id = 0;
            var specialization = db.Specialization.FirstOrDefault(x => x.specialization_code.Equals(spe_code));
            if(specialization != null)
            {
                id = specialization.specialization_id;
            }
            return id;
        }

        private bool CheckSpeExistInCurriculum(int spe_id)
        {
            var curriculum = db.Curriculum.Where(x => x.is_active == true).ToList();
            foreach (var item in curriculum)
            {
                if(item.specialization_id == spe_id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
