using BusinessObject;
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
    public class SemesterPlanDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<SemesterPlan> GetAllSemesterPlan()
        {
            var listSemesterPlan = _cmsDbContext.SemesterPlan.Include(x => x.Semester).Include(x => x.Curriculum).ToList();
            return listSemesterPlan;
        }
        //public List<SemesterPlan> GetAllSemesterPlan(int semester_id, string degree_level)
        //{
        //    var listSemesterPlan = _cmsDbContext.SemesterPlan
        //        .Include(x => x.Semester)
        //        .Include(x => x.Curriculum)
        //        .Include(x => x.Curriculum.Specialization)
        //        .Include(x => x.Semester.Batch.SemesterBatches)
        //        .Where(s => s.semester_id == semester_id && s.degree_level.EndsWith(degree_level))
        //        .ToList();



        //    return listSemesterPlan;
        //}
        public List<SemesterPlanResponse> GetAllSemesterPlan(int semester_id, string degree_level)
        {
            var listSemesterPlan = _cmsDbContext.SemesterPlan
    .Include(x => x.Curriculum)
    .Include(x => x.Curriculum.Specialization)
    .Include(x => x.Semester.SemesterBatches) // Include SemesterBatches
    .Where(s => s.semester_id == semester_id && s.degree_level.Equals(degree_level))
    .ToList();

            var responseList = new List<SemesterPlanResponse>();

            foreach (var semesterPlan in listSemesterPlan)
            {
                var spe = semesterPlan.Curriculum.Specialization.specialization_english_name;
                var totalSemester = semesterPlan.Curriculum.total_semester;
                var semester = semesterPlan.Semester.semester_name;

                // Get the list of SemesterBatch with Batch data
                var semesterBatches = _cmsDbContext.SemesterBatch
                    .Include(sb => sb.Batch) // Include Batch data
                    .Where(sb => sb.semester_id == semester_id && sb.degree_level.Equals(degree_level))
                    .ToList();

                // Convert the list of SemesterBatch to SemesterBatchResponse
                var batchResponses = semesterBatches.Select(sb => new SemesterBatchResponse
                {
                    semester_batch_id = sb.semester_batch_id,
                    semester_id = sb.semester_id,
                    batch_id = sb.batch_id,
                    batch_name = sb.Batch.batch_name, // Access Batch data
                    term_no = sb.term_no,
                    degree_level = sb.degree_level
                }).ToList();

                var semesterPlanResponse = new SemesterPlanResponse
                {
                    spe = spe,
                    totalSemester = totalSemester,
                    semester = semester,
                    batch = batchResponses
                };

                responseList.Add(semesterPlanResponse);
            }

            return responseList;

            //public List<SemesterBatch> GetAllSemesterPlan(int semester_id, string degree_level)
            //{
            //    // Lấy SemesterPlan dựa trên semester_id và degree_level
            //    var semesterPlan = _cmsDbContext.SemesterPlan
            //        .Include(sp => sp.Curriculum)
            //            .ThenInclude(c => c.Specialization)
            //        .Single(sp => sp.semester_id == semester_id && sp.degree_level == degree_level);

            //    // Lấy Specialization từ Curriculum
            //    var specialization = semesterPlan.Curriculum.Specialization;

            //    // Lấy danh sách các SemesterBatch dựa trên điều kiện
            //    var relatedSemesterBatches = _cmsDbContext.SemesterBatch
            //        .Where(sb => sb.semester_id == specialization.semester_id
            //                    && sb.degree_level == degree_level
            //                    && sb.batch_id < specialization.semester_id)
            //        .ToList();

            //    return relatedSemesterBatches;
            //}

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
