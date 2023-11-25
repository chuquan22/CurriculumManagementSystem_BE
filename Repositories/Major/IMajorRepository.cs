using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repositories.Major
{
    public interface IMajorRepository
    {
        public List<BusinessObject.Major> GetAllMajor();
        List<BusinessObject.Major> GetMajorByDegreeLevel(int degreeId);
        BusinessObject.Major GetMajorBySubjectId(int subjectId);
        public BusinessObject.Major AddMajor(BusinessObject.Major major);
        List<BusinessObject.Major> GetMajorByBatch(int batchId);
        public BusinessObject.Major EditMajor(BusinessObject.Major major);
        public void DeleteMajor(int id);
        BusinessObject.Major CheckMajorbyMajorCode(string code, int degree_level_id);
        BusinessObject.Major CheckMajorbyMajorCode(string code);

        BusinessObject.Major CheckMajorbyMajorName(string name);
        BusinessObject.Major CheckMajorbyMajorEnglishName(string eng_name);
        public BusinessObject.Major FindMajorById(int majorId);

    }
}
