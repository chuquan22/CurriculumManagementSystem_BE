using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AssessmentTypeDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<AssessmentType> GetAllAssessmentMethod()
        {
            var listAssessmentType= _context.AssessmentType
          
                .ToList();
            return listAssessmentType;
        }

        public AssessmentType GetAssessmentTypeByName(string name)
        {
            var ass = _context.AssessmentType.Where(x => x.assessment_type_name.Equals(name.Trim())).FirstOrDefault();
            return ass;
        }
    }
}
