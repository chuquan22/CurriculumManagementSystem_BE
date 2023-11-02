﻿using BusinessObject;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SemesterDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<Semester> GetSemesters()
        {
            var listSemesters = _cmsDbContext.Semester.Include(x => x.Batch).ToList();
            return listSemesters;
        }

        public List<Semester> PaginationSemester(int page, int limit, string? txtSearch)
        {
            IQueryable<Semester> query = _cmsDbContext.Semester
                .Include(x => x.Batch);

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.semester_name.ToLower().Contains(txtSearch.ToLower().Trim()) || x.school_year.ToString().Contains(txtSearch.Trim()));
            }

            var listSemester = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return listSemester;
        }

        public int GetTotalSemester(string? txtSearch)
        {
            IQueryable<Semester> query = _cmsDbContext.Semester
                .Include(x => x.Batch);

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.semester_name.ToLower().Contains(txtSearch.ToLower().Trim()) || x.school_year.ToString().Contains(txtSearch.Trim()));
            }

            var listSemester = query
                .ToList();
            return listSemester.Count;
        }

        public Semester GetSemester(int id)
        {
            var semester = _cmsDbContext.Semester.Include(x => x.Batch).FirstOrDefault(x => x.semester_id == id);
            return semester;
        }

        public bool CheckSemesterDuplicate(int id, string name, int schoolYear)
        {
            return (_cmsDbContext.Semester?.Any(x => x.semester_name.Equals(name) && x.school_year == schoolYear && x.semester_id != id)).GetValueOrDefault();
        }

        public bool CheckSemesterExsit(int id)
        {
            var exsitSemesterPlan = _cmsDbContext.SemesterPlan.FirstOrDefault(x => x.semester_id == id);
            var exsitSemesterBatch = _cmsDbContext.SemesterBatch.FirstOrDefault(x => x.semester_id == id);
            var exsitSpecialization = _cmsDbContext.Specialization.FirstOrDefault(x => x.semester_id == id);
            if(exsitSemesterPlan == null && exsitSemesterBatch == null && exsitSpecialization == null)
            {
                return false;
            }
            return true;
        }

        public string CreateSemester(Semester semester)
        {
            try
            {
                _cmsDbContext.Semester.Add(semester);
                _cmsDbContext.SaveChanges();
                return Result.createSuccessfull.ToString();
            }catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateSemester(Semester semester)
        {
            try
            {
                _cmsDbContext.Semester.Update(semester);
                _cmsDbContext.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteSemester(Semester semester)
        {
            try
            {
                _cmsDbContext.Semester.Remove(semester);
                _cmsDbContext.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

    }
}
