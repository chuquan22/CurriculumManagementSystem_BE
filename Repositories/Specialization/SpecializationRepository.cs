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
            throw new NotImplementedException();
        }

        public BusinessObject.Specialization DeleteSpecialization(BusinessObject.Specialization specification)
        {
            throw new NotImplementedException();
        }

        public BusinessObject.Specialization FindSpeById(int speId)
        {
            throw new NotImplementedException();
        }

        public List<BusinessObject.Specialization> GetSpecByMajorId(int majorId)
        {
            return db.GetListSpecialzationByMajorID(majorId);
        }

        public BusinessObject.Specialization UpdateSpecialization(BusinessObject.Specialization specification)
        {
            throw new NotImplementedException();
        }
    }
}
