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
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    
}
