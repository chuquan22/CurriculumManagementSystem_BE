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

        public bool CheckAssmentTypeDuplicate(int id, string name)
        {
            return db.CheckAssmentTypeDuplicate(id,name);
        }

        public bool CheckAssmentTypeExsit(int id)
        {
            return db.CheckAssmentTypeExsit(id);
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

        public int GetTotalAssessmentType(string? txtSearch)
        {
            return db.GetTotalAssessmentType(txtSearch);
        }

        public List<AssessmentType> PaginationAssessmentType(int page, int limit, string? txtSearch)
        {
            return db.PaginationAssessmentType(page, limit, txtSearch);
        }

        public string UpdateAssessmentType(AssessmentType type)
        {
            return db.UpdateAssessmentType(type);
        }
    }
}
