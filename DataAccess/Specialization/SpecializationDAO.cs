using BusinessObject;
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
               list = db.Specialization.ToList();              
            }
            catch (Exception)
            {
                
            }
            finally
            {

            }
            return list;
        }

        public BusinessObject.Specialization CreateSpecialization(BusinessObject.Specialization spe)
        {
            try
            {
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
        public BusinessObject.Specialization EditSpecialization(BusinessObject.Specialization speEdit)
        {
            BusinessObject.Specialization spe = new BusinessObject.Specialization();
            try
            {
                spe = db.Specialization.Where(x => x.specialization_id == speEdit.specialization_id).ToList().FirstOrDefault();
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
    }
}
