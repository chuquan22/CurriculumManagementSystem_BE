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
        public List<SemesterPlanResponse> GetAllSemesterPlan(int semester_id, int degree_level)
        {
            var listSemesterPlan = _cmsDbContext.SemesterPlan
                .Include(x => x.Curriculum)
                .Include(x => x.Curriculum.Specialization)
                .Include(x => x.Semester.SemesterBatches)
                .Where(s => s.semester_id == semester_id && s.Semester.DegreeLevel.degree_level_id == degree_level)
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
                        .Where(sb => sb.semester_id == semester_id && sb.degree_level_id == degree_level)
                        .ToList();

                    var batchResponses = semesterBatches.Select(sb => new SemesterBatchResponse
                    {
                        semester_batch_id = sb.semester_batch_id,
                        semester_id = sb.semester_id,
                        batch_id = sb.batch_id,
                        batch_name = sb.Batch.batch_name,
                        term_no = sb.term_no,
                        degree_level_id = sb.degree_level_id
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

        public SemesterPlanDetailsResponse GetSemesterPlanDetails(int semester_id, int degree_level)
        {

            var curriculumIds = _cmsDbContext.SemesterPlan
                    .Where(sp => sp.semester_id == semester_id && sp.Semester.degree_level_id == degree_level)
                    .Select(sp => sp.curriculum_id)
                    .Distinct()
                    .ToList();
            var responseList = new SemesterPlanDetailsResponse
            {
                spe = new List<SemesterPlanDetailsTermResponse>(),
            };
            foreach (var curriculumId in curriculumIds)
            {
                var semesterBatches = _cmsDbContext.SemesterBatch
               .Include(sb => sb.Semester)
               .Where(sb => sb.semester_id == semester_id && sb.degree_level_id == degree_level)
               .ToList();


                var semesterPlanDetails = new SemesterPlanDetailsTermResponse
                {
                    specialization_name = "",
                    major_name = "",
                    courses = new List<DataTermNoResponse>(),
                };
                var dataTermNo = new DataTermNoResponse();
                foreach (var semesterBatch in semesterBatches)
                {
                    responseList.semesterName = semesterBatch.Semester.semester_name;

                    var curriculum = _cmsDbContext.Curriculum
                        .Include(c => c.Specialization)
                        .ThenInclude(s => s.Major)
                        .Include(c => c.CurriculumSubjects)
                        .ThenInclude(cs => cs.Subject)
                        .ThenInclude(subject => subject.AssessmentMethod)
                        .ThenInclude(assessmentMethod => assessmentMethod.AssessmentType)
                        .FirstOrDefault(c => c.curriculum_id == curriculumId);


                    semesterPlanDetails.specialization_name = curriculum.Specialization.specialization_english_name;
                    semesterPlanDetails.major_name = curriculum.Specialization.Major.major_english_name;

                    dataTermNo = new DataTermNoResponse
                    {
                        term_no = semesterBatch.term_no,

                        subjectData = curriculum.CurriculumSubjects
                            .Where(cs => cs.term_no == semesterBatch.term_no && cs.Subject.subject_code != null)
                            .Select(cs => new DataSubjectReponse
                            {
                                subject_code = cs.Subject.subject_code,
                                subject_name = cs.Subject.subject_name,
                                credit = cs.Subject.credit,
                                total = cs.Subject.total_time,
                                @class = cs.Subject.total_time_class,
                                @exam = cs.Subject.exam_total,
                                method = cs.Subject.AssessmentMethod.assessment_method_component,
                                assessment = cs.Subject.AssessmentMethod.AssessmentType.assessment_type_name
                            })
                            .ToList(),
                    };

                    semesterPlanDetails.courses.Add(dataTermNo);

                }

                responseList.spe.Add(semesterPlanDetails);
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
