﻿using BusinessObject;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SyllabusDAO
    {
        public List<Syllabus> GetListSyllabus(int page, int limit, string txtSearch, string subjectCode)
        {
            List<Syllabus> rs = new List<Syllabus>();
            using (var context = new CMSDbContext())
            {
                rs = context.Syllabus
                                   .Include(s => s.Subject)
                                   .Include(s => s.Subject.LearningMethod)
                                   .Where(s => s.syllabus_status == true)
                                   .OrderByDescending(x => x.approved_date).ToList();
                if (!string.IsNullOrEmpty(txtSearch))
                {
                    rs = rs.Where(sy => sy.Subject.subject_name.Contains(txtSearch)
                    || sy.Subject.subject_code.Contains(txtSearch)
                    || sy.Subject.english_subject_name.Contains(txtSearch)

                    ).OrderByDescending(x => x.approved_date).ToList();                
                }
                if (!string.IsNullOrEmpty(subjectCode))
                {
                    rs = rs.Where(sy => sy.Subject.subject_code.Contains(subjectCode)).OrderByDescending(x => x.approved_date).ToList();
                }
                rs = rs
                .Skip((page - 1)* limit).Take(limit).ToList();
            }
            return rs;
        }

        public Syllabus CreateSyllabus(Syllabus rs)
        {
            using (var context = new CMSDbContext())
            {
                rs.syllabus_status = true;
                if(rs.approved_date != null)
                {
                    rs.syllabus_approved = true;
                }
                context.Syllabus.Add(rs);
                context.SaveChanges();
            }
            return rs;
        }

        public List<PreRequisite> GetListPre(int id)
        {
            List<PreRequisite> rs = new List<PreRequisite>();
            using (var context = new CMSDbContext())
            {
                rs = context.PreRequisite
                    .Include(x => x.Subject)
                    .Include(x => x.PreRequisiteType)
                    .Where(x => x.subject_id == id )
                    .ToList();  
            }
            return rs;
        }

        public int GetTotalSyllabus(string txtSearch, string subjectCode)
        {
            int rs = 0;
            using (var context = new CMSDbContext())
            {
                rs = context.Syllabus.Include(s => s.Subject)
                                   .Include(s => s.Subject.LearningMethod)
                                   .ToList()
                                   .Count();
                if (!string.IsNullOrEmpty(txtSearch))
                {
                    rs = context.Syllabus.Include(s => s.Subject).Where(sy => sy.Subject.subject_name.Contains(txtSearch)
                    || sy.Subject.subject_code.Contains(txtSearch)
                    || sy.Subject.english_subject_name.Contains(txtSearch)
                    ).ToList().Count();
                }
                if (!string.IsNullOrEmpty(subjectCode))
                {
                    rs = context.Syllabus.Include(s => s.Subject).Where(sy => sy.Subject.subject_code.Contains(subjectCode)).ToList().Count();
                }
            }
            return rs;
        }

        public Syllabus GetSyllabusById(int id)
        {
            Syllabus rs = new Syllabus();
            using (var context = new CMSDbContext())
            {
                rs = context.Syllabus.Include(s => s.Subject)
                                   .Include(s => s.Subject.LearningMethod)
                                   .Where(s => s.syllabus_id == id)
                                   .FirstOrDefault();
            }
            return rs;
        }
        public bool SetStatus(int id)
        {
            Syllabus rs = new Syllabus();
            using (var context = new CMSDbContext())
            {
                rs = context.Syllabus.Include(s => s.Subject)
                                   .Include(s => s.Subject.LearningMethod)
                                   .Where(s => s.syllabus_id == id)
                                   .FirstOrDefault();
                if(rs.syllabus_status == true)
                {
                    rs.syllabus_status = false;
                    context.Syllabus.Update(rs);
                    context.SaveChanges();
                    return true;
                }
                
            }
            return false;
        }
        public bool SetApproved(int id)
        {
            Syllabus rs = new Syllabus();
            using (var context = new CMSDbContext())
            {
                rs = context.Syllabus.Include(s => s.Subject)
                                   .Include(s => s.Subject.LearningMethod)
                                   .Where(s => s.syllabus_id == id)
                                   .FirstOrDefault();
                rs.syllabus_approved = true;
                rs.approved_date = DateTime.Now;
                context.Syllabus.Update(rs);
                context.SaveChanges();
                return true;
               

            }
            return false;
        }

        public string UpdatePatchSyllabus(Syllabus syllabus)
        {
            using (var context = new CMSDbContext())
            {
                var oldRs = context.Syllabus
               .Include(s => s.Subject)
               .Include(s => s.Subject.LearningMethod)
               .Where(s => s.syllabus_id == syllabus.syllabus_id)
               .FirstOrDefault();

                if (oldRs != null)
                {
                    if (syllabus.syllabus_description != null)
                        oldRs.syllabus_description = syllabus.syllabus_description;

                    if (syllabus.degree_level != null)
                        oldRs.degree_level = syllabus.degree_level;

                    if (syllabus.syllabus_tool != null )
                        oldRs.syllabus_tool = syllabus.syllabus_tool;



                    context.Syllabus.Update(oldRs);
                    context.SaveChanges();
                }
            }
            return Result.updateSuccessfull.ToString();
        }
    }
}
