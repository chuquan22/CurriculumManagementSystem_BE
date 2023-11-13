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

        public string DeleteSpecialization(int id);
        List<BusinessObject.Specialization> GetSpeByBatchId(int batchId, int majorId);
        public BusinessObject.Specialization GetSpeById(int speId);
        int GetTotalSpecialization(string? txtSearch, string? major_id);
        List<BusinessObject.Specialization> GetListSpecialization(int page, int limit, string? txtSearch, string? major_id);
        List<BusinessObject.Specialization> GetSpeByMajorId(int majorId);

        int GetSpecializationIdByCode(string spe_code);

        public bool IsCodeExist(string code);

    }
}
