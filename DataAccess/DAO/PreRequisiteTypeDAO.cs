using BusinessObject;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class PreRequisiteTypeDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<PreRequisiteType> ListPreRequisiteTypes()
        {
            var listPreRequisiteType = _cmsDbContext.PreRequisiteType.ToList();
            return listPreRequisiteType;
        }

        public PreRequisiteType GetPreRequisiteType(int id)
        {
            var preRequisiteType = _cmsDbContext.PreRequisiteType.Find(id);
            return preRequisiteType;
        }

        public string CreatePreRequisiteType(PreRequisiteType preRequisiteType)
        {
            try
            {
                _cmsDbContext.PreRequisiteType.Add(preRequisiteType);
                _cmsDbContext.SaveChanges();
                return Result.createSuccessfull.ToString(); 
            }catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdatePreRequisiteType(PreRequisiteType preRequisiteType)
        {
            try
            {
                _cmsDbContext.PreRequisiteType.Update(preRequisiteType);
                _cmsDbContext.SaveChanges();
                return Result.createSuccessfull.ToString();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }

        public string DeletePreRequisiteType(PreRequisiteType preRequisiteType)
        {
            try
            {
                _cmsDbContext.PreRequisiteType.Remove(preRequisiteType);
                _cmsDbContext.SaveChanges();
                return Result.createSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
