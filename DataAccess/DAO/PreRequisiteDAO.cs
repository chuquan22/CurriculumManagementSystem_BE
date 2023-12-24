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

        public List<PreRequisite> GetAllPreRequisite()
        {
                var preRequisite = _cmsDbContext.PreRequisite
                    .Include(x => x.Subject)
                    .Include(x => x.PreRequisiteType)
                    .Include(x => x.PreSubject)
                    .ToList();
                return preRequisite;
        }

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
                //_cmsDbContext.PreRequisite.Add(preRequisite);
                //int number = _cmsDbContext.SaveChanges();
                //if (number > 0)
                //{
                    return Result.createSuccessfull.ToString();
                //}
                //else
                //{
                //    return "Create PreRequisite Fail";
                //}
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
                int number = _cmsDbContext.SaveChanges();
                if (number > 0)
                {
                    return Result.updateSuccessfull.ToString();
                }
                else
                {
                    return "Update PreRequisite Fail";
                }
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
                int number = _cmsDbContext.SaveChanges();
                if (number > 0)
                {
                    return Result.deleteSuccessfull.ToString();
                }
                else
                {
                    return "Delete PreRequisite Fail";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
