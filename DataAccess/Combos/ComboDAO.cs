﻿using BusinessObject;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Combos
{
    public class ComboDAO
    {
        public CMSDbContext db = new CMSDbContext();

        public List<Combo> GetListCombo(int specId)
        {
            List<Combo> rs = new List<Combo>();
            try
            {
                rs = db.Combo.Include(x => x.Specialization).Where(x => x.specialization_id == specId).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return rs;
        }

        public List<Combo> GetListComboByCurriId(int curriId)
        {
            List<Combo> rs = new List<Combo>();
            try
            {
                var curriculum = db.Curriculum.FirstOrDefault(x => x.curriculum_id == curriId);
                rs = GetListCombo(curriculum.specialization_id);
            }
            catch (Exception)
            {

                throw;
            }
            return rs;
        }

        public bool DisableCombo(int id)
        {
            var combo = db.Combo.Where(x => x.combo_id == id).FirstOrDefault();
            if(combo != null)
            {
                if (combo.is_active == false)
                {
                    combo.is_active = true;
                }
                else
                {
                    combo.is_active = false;
                }
                db.Combo.Update(combo);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;   
            }
        }

        public bool IsCodeExist(string code, int speId)
        {
            var combo = new Combo();
            try
            {
                var spe = db.Specialization
            .Where(x => x.specialization_id == speId && x.Combos.Any(c => c.combo_code.Equals(code)))
            .FirstOrDefault();

               
                if(spe != null)
                {
                    return true;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }

        public Combo FindComboById(int id)
        {
            var combo = new Combo();
            try
            {
                 combo = db.Combo.Include(x => x.Specialization).Where(x => x.combo_id == id).FirstOrDefault();
                
            }
            catch (Exception)
            {

                return null;
            }
            return combo;
        }

        public Combo FindComboByCode(string comboCode, int speId)
        {
            var combo = new Combo();
            try
            {
                combo = db.Combo.Include(x => x.Specialization).Where(x => x.combo_code == comboCode && x.specialization_id == speId).FirstOrDefault();

            }
            catch (Exception)
            {

                throw;
            }
            return combo;
        }

        public Combo CreateCombo(Combo cb)
        {
            try
            {
                db.Combo.Add(cb);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return cb;
        }
        public Combo UpdateCombo(Combo cb)
        {
            try
            {
                var oldCombo = db.Combo.Where(c => c.combo_id == cb.combo_id).FirstOrDefault();
                if (oldCombo != null)
                {
                    oldCombo.combo_name = cb.combo_name;
                    oldCombo.combo_english_name = cb.combo_english_name;
                    oldCombo.specialization_id = cb.specialization_id;
                    oldCombo.is_active = cb.is_active;
                    db.Combo.Update(oldCombo);
                    db.SaveChanges();
                    return cb;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string DeleteCombo(int id)
        {
            Combo combo = new Combo();
            if (CheckComboExist(id))
            {
                return null;
            }
            try
            {
                combo = db.Combo.Where(c => c.combo_id == id).FirstOrDefault();
                if(combo != null)
                {
                    db.Combo.Remove(combo);
                    db.SaveChanges();
                    return "Delete sucessfully.";
                }
                else
                {
                    return "Combo is not exist in system!";
                }

            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;

            }
            return null;
        }

        private bool CheckComboExist(int combo_id)
        {
            var listCurriSubject = db.CurriculumSubject.ToList();
            foreach(var c in listCurriSubject)
            {
                if(c.combo_id == combo_id)
                {
                    return true;
                }
            }
            return false;
        }
    }
    
}
