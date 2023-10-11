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
    public class PreRequisiteDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<PreRequisite> GetPreRequisiteBySubject(int subjectId)
        {
            try
            {
                var preRequisite = _cmsDbContext.PreRequisite
                    .Include(x => x.Subject)
                    .Include(x => x.PreRequisiteType)
                    .Include(x => x.PreSubject)
                    .Where(x => x.subject_id == subjectId).ToList();
                return preRequisite;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public PreRequisite GetPreRequisite(int subjectId, int preSubjectId)
        {
            var preRequisite = _cmsDbContext.PreRequisite
                .Include(x => x.Subject)
                .Include(x => x.PreRequisiteType)
                .Include(x => x.PreSubject)
                .Where(x => x.pre_subject_id == preSubjectId && x.subject_id == subjectId).FirstOrDefault();
            return preRequisite;
        }

        public string CreatePreRequisite(PreRequisite preRequisite)
        {
            try
            {
                _cmsDbContext.PreRequisite.Add(preRequisite);
                _cmsDbContext.SaveChanges();
                return Result.createSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdatePreRequisite(PreRequisite preRequisite)
        {
            try
            {
                _cmsDbContext.PreRequisite.Update(preRequisite);
                _cmsDbContext.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeletePreRequisite(PreRequisite preRequisite)
        {
            try
            {
                _cmsDbContext.PreRequisite.Remove(preRequisite);
                _cmsDbContext.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
