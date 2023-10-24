using BusinessObject;
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
                rs = db.Combo.Where(x => x.specialization_id == specId).ToList();
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
                if (combo.is_active = false)
                {
                    combo.is_active = true;
                }
                else
                {
                    combo.is_active = false;
                }
                return true;
            }
            else
            {
                return false;   
            }
        }

        public Combo FindComboById(int id)
        {
            var combo = new Combo();
            try
            {
                 combo = db.Combo.Where(x => x.combo_id == id).FirstOrDefault();
                
            }
            catch (Exception)
            {

                throw;
            }
            return combo;
        }

        public Combo FindComboByCode(string comboCode)
        {
            var combo = new Combo();
            try
            {
                combo = db.Combo.Where(x => x.combo_code == comboCode).FirstOrDefault();

            }
            catch (Exception)
            {

                throw;
            }
            return combo;
        }

        public Combo CreateCombo(Combo cb)
        {
            Combo combo = new Combo();
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
                    oldCombo.combo_code = cb.combo_code;
                    oldCombo.combo_name = cb.combo_name;
                    oldCombo.combo_english_name = cb.combo_english_name;
                    oldCombo.combo_description = cb.combo_description;
                    oldCombo.specialization_id = cb.specialization_id;
                    oldCombo.is_active = cb.is_active;
                    db.Combo.Update(oldCombo);
                    db.SaveChanges();
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Combo DeleteCombo(int id)
        {
            Combo combo = new Combo();
            try
            {
                combo = db.Combo.Where(c => c.combo_id == id).FirstOrDefault();
                if(combo != null)
                {
                    db.Combo.Remove(combo);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return combo;
        }
    }
    
}
