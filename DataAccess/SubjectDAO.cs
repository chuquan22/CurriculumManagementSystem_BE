using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SubjectDAO
    {
        public readonly CMSDbContext CMSDbContext;

        public SubjectDAO(CMSDbContext cMSDbContext)
        {
            CMSDbContext = cMSDbContext;
        }

        public List<Subject> GetAllSubjects()
        {
            List<Subject> list = new List<Subject>();
            list = CMSDbContext.Subject.ToList();
            return list;
        }

        public Subject GetSubjectById(int id)
        {
            try
            {
                Subject subject = new Subject();
                subject = CMSDbContext.Subject.FirstOrDefault(x => x.subject_id == id);
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
                var listSubject = CMSDbContext.Subject.Where(x => x.subject_name.Contains(name)).ToList();
                return listSubject;
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
            }catch (DbUpdateConcurrencyException ex)
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
