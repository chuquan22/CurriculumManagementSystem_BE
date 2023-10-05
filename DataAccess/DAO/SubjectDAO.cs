
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SubjectDAO
    {
        public readonly CMSDbContext CMSDbContext = new CMSDbContext();
        

        public List<Subject> GetAllSubjects()
        {
            var list = CMSDbContext.Subject.Include(x => x.AssessmentMethod).Include(x => x.LearningMethod).ToList();
            return list;
        }

        public Subject GetSubjectById(int id)
        {
            try
            {
                var subject = CMSDbContext.Subject.Include(x => x.AssessmentMethod).Include(x => x.LearningMethod).SingleOrDefault(x => x.subject_id == id);
                return subject;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Subject> GetSubjectByName(string name)
        {
            try
            {
                var list = CMSDbContext.Subject.Include(x => x.AssessmentMethod).Include(x => x.LearningMethod).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string CreateSubject(Subject subject)
        {
            try
            {
                CMSDbContext.Subject.Add(subject);
                CMSDbContext.SaveChanges();
                return "OK";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }

        public string UpdateSubject(Subject subject)
        {
            try
            {
                CMSDbContext.Subject.Update(subject);
                CMSDbContext.SaveChanges();
                return "OK";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }

        public string DeleteSubject(Subject subject)
        {
            try
            {
                CMSDbContext.Subject.Remove(subject);
                CMSDbContext.SaveChanges();
                return "OK";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }
    }
}
