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

        public List<PreRequisiteType> PaginationPreRequisiteType(int page, int limit, string? txtSearch)
        {
            IQueryable<PreRequisiteType> query = _cmsDbContext.PreRequisiteType;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.pre_requisite_type_name.ToLower().Contains(txtSearch.ToLower().Trim()));
            }

            var listPreRequisiteType = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return listPreRequisiteType;
        }

        public int GetTotalPreRequisite(string? txtSearch)
        {
            IQueryable<PreRequisiteType> query = _cmsDbContext.PreRequisiteType;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.pre_requisite_type_name.ToLower().Contains(txtSearch.ToLower().Trim()));
            }

            var listPreRequisiteType = query
                .ToList();
            return listPreRequisiteType.Count;
        }
        public bool PreRequisiteTypeExist(int id)
        {
            return (_cmsDbContext.PreRequisiteType?.Any(e => e.pre_requisite_type_id == id)).GetValueOrDefault(); 
        }

        public bool CheckPreRequisiteTypeDuplicate(int id,string name)
        {
            return (_cmsDbContext.PreRequisiteType?.Any(x => x.pre_requisite_type_name.Equals(name) && x.pre_requisite_type_id != id)).GetValueOrDefault();
        }

        public bool CheckPreRequisiteTypeExsit(int id)
        {
            return (_cmsDbContext.PreRequisite?.Any(x => x.pre_requisite_type_id == id)).GetValueOrDefault();
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
                return ex.InnerException.Message;
            }
        }

        public string UpdatePreRequisiteType(PreRequisiteType preRequisiteType)
        {
            try
            {
                _cmsDbContext.PreRequisiteType.Update(preRequisiteType);
                _cmsDbContext.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.InnerException.Message;
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
                return ex.InnerException.Message;
            }
        }
    }
}
