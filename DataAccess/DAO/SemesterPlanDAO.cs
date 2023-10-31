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
        public List<SemesterPlanResponse> GetAllSemesterPlan(int semester_id, string degree_level)
        {
            var listSemesterPlan = _cmsDbContext.SemesterPlan
                .Include(x => x.Curriculum)
                .Include(x => x.Curriculum.Specialization)
                .Include(x => x.Semester.SemesterBatches)
                .Where(s => s.semester_id == semester_id && s.degree_level.Equals(degree_level))
                .ToList();

            var responseList = new List<SemesterPlanResponse>();

            var uniqueCurriculumIds = listSemesterPlan.Select(sp => sp.Curriculum.specialization_id).Distinct().ToList();

            foreach (var curriculumId in uniqueCurriculumIds)
            {
                var semesterPlanForCurriculum = listSemesterPlan.FirstOrDefault(sp => sp.Curriculum.specialization_id == curriculumId);

                if (semesterPlanForCurriculum != null)
                {
                    var spe = semesterPlanForCurriculum.Curriculum.Specialization.specialization_english_name;
                    var totalSemester = semesterPlanForCurriculum.Curriculum.total_semester;
                    var semester = semesterPlanForCurriculum.Semester.semester_name;

                    var semesterBatches = _cmsDbContext.SemesterBatch
                        .Include(sb => sb.Batch)
                        .Where(sb => sb.semester_id == semester_id && sb.degree_level.Equals(degree_level))
                        .ToList();

                    var batchResponses = semesterBatches.Select(sb => new SemesterBatchResponse
                    {
                        semester_batch_id = sb.semester_batch_id,
                        semester_id = sb.semester_id,
                        batch_id = sb.batch_id,
                        batch_name = sb.Batch.batch_name,
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
            }

            return responseList;
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
