﻿using DataAccess.Specialization;
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
            return db.CreateSpecialization(specification);
        }

        public string DeleteSpecialization(int id)
        {
            return db.DeleteSpecialization(id);
        }

        public BusinessObject.Specialization GetSpeById(int speId)
        {

            return db.GetSpeById(speId);

           // return db.GetSpeById(speId);
        }

        public List<BusinessObject.Specialization> GetListSpecialization(int degree_id, int page, int limit, string? txtSearch, string? major_id)
        {
            return db.GetSpecByPagging(degree_id, page, limit, txtSearch, major_id);

        }

        public List<BusinessObject.Specialization> GetSpecialization()
        {
            return db.GetSpec();
        }


        public int GetSpecializationIdByCode(string spe_code)
        {
            return db.GetSpecializationIdByCode(spe_code);
        } 

        public int GetTotalSpecialization(int degree_id, string? txtSearch, string? major_id)
        {
            return db.GetTotalSpecialization(degree_id, txtSearch, major_id);

        }

        public BusinessObject.Specialization UpdateSpecialization(BusinessObject.Specialization specification)
        {
            return db.UpdateSpecialization(specification);
        }

        public bool IsCodeExist(string code)
        {
            return db.IsCodeExist(code);
        }

        public List<BusinessObject.Specialization> GetSpeByMajorId(int majorId, string batch_name)
        {
            return db.GetSpeByMajorId(majorId, batch_name);
        }

        public List<BusinessObject.Specialization> GetSpeByBatchId(int batchId, int majorId)
        {
            return db.GetSpeByBatchId(batchId, majorId);
        }
    }
}
