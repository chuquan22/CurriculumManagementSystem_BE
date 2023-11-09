using BusinessObject;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
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

        public List<SemesterPlanDTO> GetSemesterPlan(int semester_id)
        {
            var Semester = _cmsDbContext.Semester.Where(x => x.semester_id == semester_id).FirstOrDefault();
            int degreeLevel = Semester.degree_level_id;
            var StartBatch = _cmsDbContext.Batch.Where(x => x.batch_id == Semester.start_batch_id).FirstOrDefault();
            var ListCurri = _cmsDbContext.CurriculumBatch.Where(x => x.batch_id == Semester.start_batch_id).ToList();
            List<SemesterPlanDTO> listResponse = new List<SemesterPlanDTO>();
            foreach (var item in ListCurri)
            {
                var Curriculum = _cmsDbContext.Curriculum.Include(x => x.Specialization).Where(x => x.curriculum_id == item.curriculum_id).FirstOrDefault();
                int validOrder = StartBatch.batch_order - Curriculum.total_semester;
                if (validOrder <= 1)
                {
                    validOrder = 1;
                }
                var listValidSemester = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.degree_level_id == degreeLevel)
                    .Where(x => x.Batch.batch_order >= validOrder && x.Batch.batch_order <= StartBatch.batch_order)
                    .OrderByDescending(x => x.Batch.batch_order)
                    .ToList();
                int specializationId = Curriculum.specialization_id;
                var validBatchIds = listValidSemester.Select(validSemester => validSemester.Batch.batch_id).ToList();
                string speName = Curriculum.Specialization.specialization_english_name;
                var listCurriValid = _cmsDbContext.CurriculumBatch.Include(x => x.Curriculum)
                    .Where(x => validBatchIds.Contains(x.batch_id))
                    .Where(x => x.Curriculum.specialization_id == specializationId)
                    .OrderByDescending(x => x.Batch.batch_order)
                    .ToList();
                int termNo = 1;
                List<SemesterPlan> list = new List<SemesterPlan>();
                foreach (var curri in listCurriValid)
                {
                    foreach (var batch in listValidSemester)
                    {
                        if (curri.batch_id == batch.start_batch_id)
                        {
                            SemesterPlan s = new SemesterPlan();
                            s.curriculum_id = curri.Curriculum.curriculum_id;
                            s.semester_id = semester_id;
                            s.term_no = termNo;
                            list.Add(s);
                            termNo++;
                        }
                    }
                }
                SemesterPlanDTO sDTO = new SemesterPlanDTO { listSemester = new List<SemesterPlan>() };
                sDTO.speName = speName;
                sDTO.listSemester = list;
                listResponse.Add(sDTO);

            }
            foreach (var item in listResponse)
            {
                foreach (var semester in item.listSemester)
                {
                    SemesterPlan sw = new SemesterPlan();
                    sw.semester_id = semester.semester_id;
                    sw.curriculum_id = semester.curriculum_id;
                    sw.term_no = semester.term_no;
                    _cmsDbContext.SemesterPlan.Add(sw);
                    _cmsDbContext.SaveChanges();
                }
            }
            return listResponse;
        }

        public SemesterPlanDetailsResponse GetSemesterPlanDetails(int semester_id)
        {
            var Semester = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.semester_id == semester_id).FirstOrDefault();
            int order = Semester.Batch.batch_order;
            var responseList = new SemesterPlanDetailsResponse
            {
                spe = new List<SemesterPlanDetailsTermResponse>(),
            };
            responseList.semesterName = Semester.semester_name + " - " + Semester.school_year;
            var SemesterPlan = _cmsDbContext.SemesterPlan.Include(x => x.Curriculum).Include(x => x.Semester).Where(x => x.semester_id == Semester.semester_id).ToList();
            List<SemesterPlanCount> listCount = new List<SemesterPlanCount>();

            List<SemesterPlanDetailsTermResponse> SemesterPlanDetails = new List<SemesterPlanDetailsTermResponse>();
            foreach (var item in SemesterPlan)
            {
                var existingCount = listCount.FirstOrDefault(x => x.curriculumId == item.curriculum_id);

                if (existingCount != null)
                {
                    existingCount.count++;
                }
                else
                {
                    listCount.Add(new SemesterPlanCount
                    {
                        curriculumId = item.curriculum_id,
                        count = 1,
                    });
                }
            }
            foreach (var item in listCount)
            {
                var semesterPlanDetails = new SemesterPlanDetailsTermResponse
                {
                    specialization_name = "",
                    major_name = "",
                    courses = new List<DataTermNoResponse>(),
                };
                var dataTermNo = new DataTermNoResponse();
                for (int i = 1; i <= item.count; i++)
                {
                    var curriculum = _cmsDbContext.Curriculum
                        .Include(c => c.Specialization)
                        .ThenInclude(s => s.Major)
                        .Include(c => c.CurriculumSubjects)
                        .ThenInclude(cs => cs.Subject)
                        .ThenInclude(subject => subject.AssessmentMethod)
                        .ThenInclude(assessmentMethod => assessmentMethod.AssessmentType)
                        .FirstOrDefault(c => c.curriculum_id == item.curriculumId);
                    var curriBatch = _cmsDbContext.CurriculumBatch.Include(x => x.Batch)
                        .Where(x => x.curriculum_id == item.curriculumId)
                        .OrderByDescending(x => x.Batch.batch_order)
                        .ToList();
                    semesterPlanDetails.specialization_name = curriculum.Specialization.specialization_english_name;
                    semesterPlanDetails.major_name = curriculum.Specialization.Major.major_english_name;
                    semesterPlanDetails.specializationId = curriculum.Specialization.specialization_id;
                    dataTermNo = new DataTermNoResponse
                    {
                        term_no = i,
                        batch = curriBatch[i - 1].Batch.batch_name,
                        batch_order = curriBatch[i - 1].Batch.batch_order,
                        batch_check = curriBatch[i - 1].Batch.batch_name,
                        curriculum_code = curriculum.curriculum_code,
                        subjectData = curriculum.CurriculumSubjects
                            .Where(cs => cs.term_no == (order - curriBatch[i - 1].Batch.batch_order + 1) && cs.Subject.subject_code != null)
                            .Select(cs => new DataSubjectReponse
                            {
                                subject_code = cs.Subject.subject_code,
                                subject_name = cs.Subject.english_subject_name,
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

        //    public List<SemesterPlan> GetAllSemesterPlan()
        //    {
        //        var listSemesterPlan = _cmsDbContext.SemesterPlan.Include(x => x.Semester).Include(x => x.Curriculum).ToList();
        //        return listSemesterPlan;
        //    }

        //    public List<SemesterPlanResponse> GetAllSemesterPlan(int semester_id, string degree_level)
        //    {
        //        var listSemesterPlan = _cmsDbContext.SemesterPlan
        //            .Include(x => x.Curriculum)
        //            .Include(x => x.Curriculum.Specialization)
        //            .Include(x => x.Semester.SemesterBatches)
        //            .Where(s => s.semester_id == semester_id && s.degree_level.Equals(degree_level))
        //            .ToList();

        //        var responseList = new List<SemesterPlanResponse>();

        //        var uniqueCurriculumIds = listSemesterPlan.Select(sp => sp.Curriculum.curriculum_id).ToList();

        //        foreach (var curriculumId in uniqueCurriculumIds)
        //        {
        //            var semesterPlanForCurriculum = listSemesterPlan.FirstOrDefault(sp => sp.Curriculum.curriculum_id == curriculumId);

        //            if (semesterPlanForCurriculum != null)
        //            {
        //                var spe_name = semesterPlanForCurriculum.Curriculum.Specialization.specialization_english_name;
        //                var spe_id = semesterPlanForCurriculum.Curriculum.Specialization.specialization_id;
        //                var totalSemester = semesterPlanForCurriculum.Curriculum.total_semester;
        //                var semester = semesterPlanForCurriculum.Semester;
        //                var semester_name = semester.semester_name;

        //                var semester_filter = _cmsDbContext.Semester.Where(x => x.semester_id == semesterPlanForCurriculum.Curriculum.Specialization.semester_id).FirstOrDefault();

        //                var batch = _cmsDbContext.Batch.Where(x => x.batch_id == semester_filter.batch_id).FirstOrDefault();
        //                var semesterBatches = _cmsDbContext.SemesterBatch
        //                    .Include(sb => sb.Batch)
        //                    .Where(sb => sb.semester_id == semester_id && sb.degree_level.Equals(degree_level) && string.Compare(sb.Batch.batch_name, batch.batch_name) >= 0)
        //                    .ToList();

        //                var batchResponses = semesterBatches.Select(sb => new SemesterBatchResponse
        //                {
        //                    semester_batch_id = sb.semester_batch_id,
        //                    semester_id = sb.semester_id,
        //                    batch_id = sb.batch_id,
        //                    batch_name = sb.Batch.batch_name,
        //                    term_no = sb.term_no,
        //                    degree_level = sb.degree_level
        //                }).ToList();

        //                var semesterPlanResponse = new SemesterPlanResponse
        //                {
        //                    spe = spe_name,
        //                    specialization_id = spe_id,
        //                    totalSemester = totalSemester,
        //                    semester = semester_name,
        //                    batch = batchResponses
        //                };

        //                responseList.Add(semesterPlanResponse);
        //            }
        //        }

        //        return responseList;
        //    }

        //    public SemesterPlanDetailsResponse GetSemesterPlanDetails(int semester_id, string degree_level)
        //    {

        //        var curriculumIds = _cmsDbContext.SemesterPlan
        //                .Where(sp => sp.semester_id == semester_id && sp.degree_level.Equals(degree_level))
        //                .Select(sp => sp.curriculum_id)
        //                .Distinct()
        //                .ToList();
        //        var responseList = new SemesterPlanDetailsResponse
        //        {
        //            spe = new List<SemesterPlanDetailsTermResponse>(),
        //        };
        //        foreach (var curriculumId in curriculumIds)
        //        {
        //            var curri = _cmsDbContext.Curriculum.Include(x => x.Specialization).Where(x => x.curriculum_id == curriculumId).FirstOrDefault();
        //            var semester_filter = _cmsDbContext.Semester.Where(x => x.semester_id == curri.Specialization.semester_id).FirstOrDefault();
        //            var batch = _cmsDbContext.Batch.Where(x => x.batch_id == semester_filter.batch_id).FirstOrDefault();

        //            var semesterBatches = _cmsDbContext.SemesterBatch
        //                .Include(sb => sb.Semester)
        //                .Include(sb => sb.Batch)
        //                .Where(sb => sb.semester_id == semester_id && sb.degree_level.Equals(degree_level) && string.Compare(sb.Batch.batch_name, batch.batch_name) >= 0)
        //                .ToList();

        //            var semesterPlanDetails = new SemesterPlanDetailsTermResponse
        //            {
        //                specialization_name = "",
        //                major_name = "",
        //                specialization_id = 0,
        //                courses = new List<DataTermNoResponse>(),
        //            };
        //            var dataTermNo = new DataTermNoResponse();
        //            foreach (var semesterBatch in semesterBatches)
        //            {
        //                responseList.semesterName = semesterBatch.Semester.semester_name;

        //                var curriculum = _cmsDbContext.Curriculum
        //                    .Include(c => c.Specialization)
        //                    .ThenInclude(s => s.Major)
        //                    .Include(c => c.CurriculumSubjects)
        //                    .ThenInclude(cs => cs.Subject)
        //                    .ThenInclude(subject => subject.AssessmentMethod)
        //                    .ThenInclude(assessmentMethod => assessmentMethod.AssessmentType)
        //                    .FirstOrDefault(c => c.curriculum_id == curriculumId);

        //                semesterPlanDetails.specialization_name = curriculum.Specialization.specialization_english_name;
        //                semesterPlanDetails.major_name = curriculum.Specialization.Major.major_english_name;
        //                semesterPlanDetails.specialization_id = curriculum.specialization_id;
        //                if (curriculum.batch_id == semesterBatch.batch_id)
        //                {
        //                    dataTermNo = new DataTermNoResponse
        //                    {
        //                        term_no = semesterBatch.term_no,
        //                        batch = semesterBatch.Batch.batch_name,
        //                        batch_check = curriculum.Batch.batch_name,
        //                        curriculum_code = curriculum.curriculum_code,
        //                        subjectData = curriculum.CurriculumSubjects
        //                        .Where(cs => cs.term_no == semesterBatch.term_no && cs.Subject.subject_code != null)
        //                        .Select(cs => new DataSubjectReponse
        //                        {
        //                            subject_code = cs.Subject.subject_code,
        //                            subject_name = cs.Subject.english_subject_name,
        //                            credit = cs.Subject.credit,
        //                            total = cs.Subject.total_time,
        //                            @class = cs.Subject.total_time_class,
        //                            @exam = cs.Subject.exam_total,
        //                            method = cs.Subject.AssessmentMethod.assessment_method_component,
        //                            assessment = cs.Subject.AssessmentMethod.AssessmentType.assessment_type_name
        //                        })
        //                        .ToList(),
        //                    };
        //                }
        //                else
        //                {
        //                    dataTermNo = new DataTermNoResponse
        //                    {
        //                        term_no = semesterBatch.term_no,
        //                        batch = semesterBatch.Batch.batch_name,
        //                        subjectData = null,
        //                    };
        //                }

        //                semesterPlanDetails.courses.Add(dataTermNo);

        //            }

        //            responseList.spe.Add(semesterPlanDetails);


        //        }

        //        return responseList;
        //    }
        //    public SemesterPlan GetSemesterPlan(int curriId, int semestId)
        //    {
        //        var semesterPlan = _cmsDbContext.SemesterPlan.Include(x => x.Semester).Include(x => x.Curriculum).FirstOrDefault(x => x.curriculum_id == curriId && x.semester_id == semestId);
        //        return semesterPlan;
        //    }

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