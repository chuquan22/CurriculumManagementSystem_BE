using AutoMapper;
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
        public readonly IMapper mapper;

        public List<SubjectResponse> GetAllSubjects()
        {
            var list = CMSDbContext.Subject.Include(x => x.AssessmentMethod).Include(x => x.LearningMethod).ToList();
            var listSubjectRespone = mapper.Map<List<SubjectResponse>>(list);
            return listSubjectRespone;
        }

        public SubjectResponse GetSubjectById(int id)
        {
            try
            {
                var subject = CMSDbContext.Subject.Include(x => x.AssessmentMethod).Include(x => x.LearningMethod).SingleOrDefault(x => x.subject_id == id);
                var subjectRespone = mapper.Map<SubjectResponse>(subject);
                return subjectRespone;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<SubjectResponse> GetSubjectByName(string name)
        {
            try
            {
                var list = CMSDbContext.Subject.Include(x => x.AssessmentMethod).Include(x => x.LearningMethod).ToList();
                var listSubjectRespone = mapper.Map<List<SubjectResponse>>(list);
                return listSubjectRespone;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string CreateSubject(SubjectRequest subjectRequest)
        {
            try
            {
                var subject = mapper.Map<Subject>(subjectRequest);
                CMSDbContext.Subject.Add(subject);
                CMSDbContext.SaveChanges();
                return "OK";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }

        public string UpdateSubject(SubjectRequest subjectRequest)
        {
            try
            {
                var subject = mapper.Map<Subject>(subjectRequest);
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
