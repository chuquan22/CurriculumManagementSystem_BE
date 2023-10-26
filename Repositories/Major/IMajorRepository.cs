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

        public BusinessObject.Major AddMajor(BusinessObject.Major major);

        public BusinessObject.Major EditMajor(BusinessObject.Major major);
        public void DeleteMajor(int id);
        BusinessObject.Major CheckMajorbyMajorCode(string code);
        BusinessObject.Major CheckMajorbyMajorName(string name);
        BusinessObject.Major CheckMajorbyMajorEnglishName(string eng_name);
        public BusinessObject.Major FindMajorById(int majorId);

    }
}
