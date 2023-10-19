using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Specialization
{
    public interface ISpecializationRepository
    {
        public List<BusinessObject.Specialization> GetSpecialization();

        public BusinessObject.Specialization CreateSpecialization(BusinessObject.Specialization specification);

        public BusinessObject.Specialization UpdateSpecialization(BusinessObject.Specialization specification);

        public BusinessObject.Specialization DeleteSpecialization(int id);

        public BusinessObject.Specialization FindSpeById(int speId);
        int GetTotalSpecialization(string? txtSearch, string? major_id);
        List<BusinessObject.Specialization> GetListSpecialization(int page, int limit, string? txtSearch, string? major_id);
    }
}
