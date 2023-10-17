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
        public BusinessObject.Specialization UpdateSpecialization(BusinessObject.Specialization speEdit)
        {
            BusinessObject.Specialization spe = new BusinessObject.Specialization();
            try
            {
                spe = db.Specialization.Where(x => x.specialization_id == speEdit.specialization_id).ToList().FirstOrDefault();
                if(spe != null)
                {
                    spe.specialization_code = speEdit.specialization_code;
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

        public BusinessObject.Specialization DeleteSpecialization(int id)
        {
            BusinessObject.Specialization spe = new BusinessObject.Specialization();
            try
            {
                spe = db.Specialization.Where(x => x.specialization_id == id).ToList().FirstOrDefault();
                if(spe != null)
                {
                    db.Specialization.Remove(spe);
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
    }
}
