using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.AssessmentTypes
{
    public class AssessmentTypeRepository : IAssessmentTypeRepository
    {
        private readonly AssessmentTypeDAO db = new AssessmentTypeDAO();

        public List<AssessmentType> GetAllAssessmentType()
        {
            return db.GetAllAssessmentMethod();
        }
    }
}
