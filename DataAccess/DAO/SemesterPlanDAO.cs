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
    public class SemesterPlanDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<SemesterPlan> GetAllSemesterPlan()
        {
            var listSemesterPlan = _cmsDbContext.SemesterPlan.Include(x => x.Semester).Include(x => x.Curriculum).ToList();
            return listSemesterPlan;
        }
        public List<SemesterPlan> GetAllSemesterPlan(int semester_id, string degree_level)
        {
            var listSemesterPlan = _cmsDbContext.SemesterPlan
                                    .Include(x => x.Semester)
                                    .Include(x => x.Curriculum)
                                    .Include(x => x.Curriculum.Specialization)
                                    .Include(x => x.Semester.SemesterBatches)
                                    .Include(x => x.Semester.Batch)

                .Where(s => s.semester_id == semester_id && s.degree_level.EndsWith(degree_level))
                .ToList();
            return listSemesterPlan;
        }
        public SemesterPlan GetSemesterPlan(int curriId, int semestId)
        {
            var semesterPlan = _cmsDbContext.SemesterPlan.Include(x => x.Semester).Include(x => x.Curriculum).FirstOrDefault(x => x.curriculum_id == curriId && x.semester_id == semestId);
            return semesterPlan;
        }

        public string CreateSemesterPlan(SemesterPlan semesterPlan)
        {
            try
            {
                _cmsDbContext.SemesterPlan.Add(semesterPlan);
                _cmsDbContext.SaveChanges();
                return Result.createSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateSemesterPlan(SemesterPlan semesterPlan)
        {
            try
            {
                _cmsDbContext.SemesterPlan.Update(semesterPlan);
                _cmsDbContext.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }

        public string DeleteSemesterPlan(SemesterPlan semesterPlan)
        {
            try
            {
                _cmsDbContext.SemesterPlan.Remove(semesterPlan);
                _cmsDbContext.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }
    }
}
