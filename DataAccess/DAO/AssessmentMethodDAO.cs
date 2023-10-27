using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AssessmentMethodDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<AssessmentMethod> GetAllAssessmentMethod()
        {
            var listAssessmentMethod = _context.AssessmentMethod
                .Include(x => x.AssessmentType)
                .ToList();
            return listAssessmentMethod;
        }

        public AssessmentMethod GetAsssentMethodByName(string name)
        {
            var rs = _context.AssessmentMethod.Include(a => a.AssessmentType).Where(x => x.assessment_method_component.Contains(name)).FirstOrDefault();
            return rs;
        }
    }
}
