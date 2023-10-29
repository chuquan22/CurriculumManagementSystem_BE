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

        public bool CheckAssmentTypeDuplicate(string name)
        {
            return db.CheckAssmentTypeDuplicate(name);
        }

        public string CreateAssessmentType(AssessmentType type)
        {
            return db.CreateAssessmentType(type);
        }

        public string DeleteAssessmentType(AssessmentType type)
        {
            return db.DeleteAssessmentType(type);
        }

        public List<AssessmentType> GetAllAssessmentType()
        {
            return db.GetAllAssessmentMethod();
        }

        public AssessmentType GetAssessmentTypeByName(string name)
        {
            return db.GetAssessmentTypeByName(name);
        }

        public AssessmentType GetAsssentTypeById(int id)
        {
            return db.GetAsssentTypeById(id);
        }

        public string UpdateAssessmentType(AssessmentType type)
        {
            return db.UpdateAssessmentType(type);
        }
    }
}
