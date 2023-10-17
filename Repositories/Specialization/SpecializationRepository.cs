using DataAccess.Specialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Specialization
{
    public class SpecializationRepository : ISpecializationRepository
    {
        public SpecializationDAO db = new SpecializationDAO();

        public BusinessObject.Specialization CreateSpecialization(BusinessObject.Specialization specification)
        {
            return db.CreateSpecialization(specification);
        }

        public BusinessObject.Specialization DeleteSpecialization(int id)
        {
            return db.DeleteSpecialization(id);
        }

        public BusinessObject.Specialization FindSpeById(int speId)
        {
            throw new NotImplementedException();
        }

        public List<BusinessObject.Specialization> GetSpecialization()
        {
            return db.GetSpec();
        }

        public BusinessObject.Specialization UpdateSpecialization(BusinessObject.Specialization specification)
        {
            return db.UpdateSpecialization(specification);
        }
    }
}
