﻿using BusinessObject;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SemesterPlanDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();
        private readonly SubjectDAO _dao = new SubjectDAO();
        public string errorSemesterPlan = "";

        public List<SemesterPlanDTO> GetSemesterPlan(int semester_id)
        {
            var Semester = _cmsDbContext.Semester.Where(x => x.semester_id == semester_id).FirstOrDefault();
            int degreeLevel = Semester.Batch.degree_level_id;
            if (degreeLevel == 1)
            {
                var StartBatch = _cmsDbContext.Batch.Where(x => x.batch_id == Semester.start_batch_id).FirstOrDefault();
                int count = 7;
                if (StartBatch.batch_order >= 10)
                {
                    count = 6;
                }
                int validBatchOrder = StartBatch.batch_order - count;
                if (validBatchOrder <= 0)
                {
                    validBatchOrder = 0;
                }
                var listValidSemesterCurri = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.Batch.degree_level_id == degreeLevel)
                        .Where(x => x.Batch.batch_order > validBatchOrder && x.Batch.batch_order <= StartBatch.batch_order)
                        .OrderByDescending(x => x.Batch.batch_order)
                        .ToList();
                var ListCurri = getListValidCurri(listValidSemesterCurri);
                List<SemesterPlanDTO> listResponse = new List<SemesterPlanDTO>();
                foreach (var item in ListCurri)
                {
                    var Curriculum = _cmsDbContext.Curriculum.Include(x => x.Specialization).Where(x => x.curriculum_id == item.curriculum_id).FirstOrDefault();
                    var listValidSemester = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.Batch.degree_level_id == degreeLevel)
                        .Where(x => x.Batch.batch_order > validBatchOrder && x.Batch.batch_order <= StartBatch.batch_order)
                        .OrderByDescending(x => x.Batch.batch_order)
                        .ToList();
                    int specializationId = Curriculum.specialization_id;
                    var validBatchIds = listValidSemester.Select(validSemester => validSemester.Batch.batch_id).ToList();
                    string speName = Curriculum.Specialization.specialization_english_name;
                    var listCurriValid = _cmsDbContext.CurriculumBatch.Include(x => x.Curriculum).ThenInclude(x => x.Specialization)
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
                            }
                            termNo++;
                        }
                        termNo = 1;
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
            else if (degreeLevel == 3)
            {
                int count = 2;
                var StartBatch = _cmsDbContext.Batch.Where(x => x.batch_id == Semester.start_batch_id).FirstOrDefault();
                if (StartBatch.batch_order < 4)
                {
                    count = 3;
                }
                int validBatchOrder = StartBatch.batch_order - count;
                if (validBatchOrder <= 0)
                {
                    validBatchOrder = 0;
                }
                var listValidSemesterCurri = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.Batch.degree_level_id == degreeLevel)
                        .Where(x => x.Batch.batch_order > validBatchOrder && x.Batch.batch_order <= StartBatch.batch_order)
                        .Where(x => x.no == 1)
                        .OrderByDescending(x => x.Batch.batch_order)
                        .ToList();
                var ListCurri = getListValidCurri(listValidSemesterCurri);
                List<SemesterPlanDTO> listResponse = new List<SemesterPlanDTO>();
                foreach (var item in ListCurri)
                {
                    var Curriculum = _cmsDbContext.Curriculum.Include(x => x.Specialization).Where(x => x.curriculum_id == item.curriculum_id).FirstOrDefault();
                    int validOrder = StartBatch.batch_order - count;
                    if (validOrder <= 0)
                    {
                        validOrder = 0;
                    }
                    var listValidSemester = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.Batch.degree_level_id == degreeLevel)
                        .Where(x => x.Batch.batch_order > validOrder && x.Batch.batch_order <= StartBatch.batch_order)
                        .Where(x => x.no == 1)
                        .OrderByDescending(x => x.Batch.batch_order)
                        .ToList();
                    int specializationId = Curriculum.specialization_id;
                    var validBatchIds = listValidSemester.Select(validSemester => validSemester.Batch.batch_id).ToList();
                    string speName = Curriculum.Specialization.specialization_english_name;
                    var listCurriValid = _cmsDbContext.CurriculumBatch.Include(x => x.Curriculum).ThenInclude(x => x.Specialization)
                        .Where(x => validBatchIds.Contains(x.batch_id))
                        .Where(x => x.Curriculum.specialization_id == specializationId)
                        .OrderByDescending(x => x.Batch.batch_order)
                        .ToList();
                    int termNo = 1 + (int)Semester.no - 1;
                    List<SemesterPlan> list = new List<SemesterPlan>();
                    foreach (var curri in listCurriValid)
                    {
                        termNo = 1 + (int)Semester.no - 1;
                        foreach (var batch in listValidSemester)
                        {
                            if (curri.batch_id == batch.start_batch_id)
                            {
                                if (termNo > 8)
                                {
                                    break;
                                }
                                SemesterPlan s = new SemesterPlan();
                                s.curriculum_id = curri.Curriculum.curriculum_id;
                                s.semester_id = semester_id;
                                s.term_no = termNo;
                                list.Add(s);
                                termNo += 3;
                            }
                            else
                            {
                                termNo += 3;
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
            else
            {
                return null;
            }
        }

        private List<CurriculumBatch> getListValidCurri(List<Semester> semesters)
        {
            var result = new List<CurriculumBatch>();

            foreach (var item in semesters)
            {
                var tempList = _cmsDbContext.CurriculumBatch
                    .Include(x => x.Curriculum)
                    .ThenInclude(x => x.Specialization)
                    .Where(x => x.batch_id == item.start_batch_id)
                    .ToList();

                result.AddRange(tempList);
            }
            var filteredResult = result
            .DistinctBy(x => new { x.curriculum_id })
            .DistinctBy(x => new { x.Curriculum.Specialization })
            .ToList();
            return filteredResult;
        }
        public List<SemesterPlanResponse> GetSemesterPlanOverViewDetails(int semester_id)
        {
            var semester_root = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.semester_id == semester_id).FirstOrDefault();
            int degreeLv = semester_root.Batch.degree_level_id;
            var listSemesterPlan = _cmsDbContext.SemesterPlan
               .Include(x => x.Curriculum)
               .Include(x => x.Curriculum.Specialization)
               .Include(x => x.Semester)
               .Where(s => s.semester_id == semester_id)
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
                    var semester = semesterPlanForCurriculum.Semester.semester_name + " " + semesterPlanForCurriculum.Semester.school_year;
                    int order = semester_root.Batch.batch_order - semesterPlanForCurriculum.Curriculum.total_semester;
                    var validSemester = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.Batch.batch_order > order && x.Batch.batch_order <= semester_root.Batch.batch_order).Where(x => x.Batch.degree_level_id == degreeLv).OrderByDescending(x => x.Batch.batch_order).ToList();
                    List<SemesterBatchResponse> batchResponses = new List<SemesterBatchResponse>();
                    int i = 1;
                    foreach (var item in validSemester)
                    {
                        SemesterBatchResponse batch = new SemesterBatchResponse();
                        batch.semester_batch_id = item.semester_id;
                        batch.semester_id = item.semester_id;
                        batch.batch_id = item.Batch.batch_id;
                        batch.batch_name = "K" + item.Batch.batch_name;
                        batch.term_no = i;
                        batch.degree_level = 1;
                        i++;
                        batchResponses.Add(batch);
                    }
                    i = 1;
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

        public List<CreateSemesterPlanResponse> GetSemesterPlanOverView(int semester_id)
        {

            var semesterPlan = _cmsDbContext.SemesterPlan.Where(x => x.semester_id == semester_id).ToList();
            if (semesterPlan.Count == 0)
            {
                return null;
            }
            var result = new List<CreateSemesterPlanResponse>();

            var semester = _cmsDbContext.Semester.Include(x => x.Batch.DegreeLevel).FirstOrDefault(x => x.semester_id == semester_id);
            if (semester.Batch.degree_level_id == 1)
            {
                string degreeLevel = semester.Batch.DegreeLevel.degree_level_english_name;
                var startBatch = _cmsDbContext.Batch.FirstOrDefault(x => x.batch_id == semester.start_batch_id);
                int order = startBatch.batch_order;
                int count = 7;
                if (order >= 10)
                {
                    count = 6;
                }
                int validOrder = order - count;
                List<Semester> semesterValid = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.Batch.degree_level_id == semester.Batch.degree_level_id).Where(x => x.Batch.batch_order > validOrder && x.Batch.batch_order <= order).OrderByDescending(x => x.Batch.batch_order).ToList();
                int termNO = 1;
                foreach (var batch in semesterValid)
                {
                    var response = new CreateSemesterPlanResponse();
                    response.semester_batch_id = startBatch.batch_id;
                    response.semester_id = semester.semester_id;
                    response.batch_id = batch.Batch.batch_id;
                    response.batch_name = batch.Batch.batch_name;
                    response.term_no = termNO;
                    response.degree_level = degreeLevel;
                    result.Add(response);
                    termNO++;
                }
            }
            else if (semester.Batch.degree_level_id == 3)
            {
                int count = 6;
                string degreeLevel = semester.Batch.DegreeLevel.degree_level_english_name;
                var startBatch = _cmsDbContext.Batch.FirstOrDefault(x => x.batch_id == semester.start_batch_id);
                int order = startBatch.batch_order;
                if (order < 5)
                {
                    count = 8;
                }
                int validOrder = order - count;
                List<Semester> semesterValid = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.Batch.degree_level_id == semester.Batch.degree_level_id).Where(x => x.no == 1).Where(x => x.Batch.batch_order > validOrder && x.Batch.batch_order <= order).OrderByDescending(x => x.Batch.batch_order).ToList();
                int termNO = 1 + (int)semester.no - 1;
                foreach (var batch in semesterValid)
                {
                    var response = new CreateSemesterPlanResponse();
                    response.semester_batch_id = startBatch.batch_id;
                    response.semester_id = semester.semester_id;
                    response.batch_id = batch.Batch.batch_id;
                    response.batch_name = batch.Batch.batch_name;
                    response.term_no = termNO;
                    response.degree_level = degreeLevel;
                    if (termNO <= count)
                    {
                        result.Add(response);
                    }
                    termNO += 3;
                }
            }
            return result;
        }
        public SemesterPlanDetailsResponse GetSemesterPlanDetails(int semester_id)
        {
            var Semester = _cmsDbContext.Semester.Include(x => x.Batch).ThenInclude(x => x.DegreeLevel).Where(x => x.semester_id == semester_id).FirstOrDefault();
            if (Semester == null)
            {
                throw new Exception("Invalid Semester!");
            }
            int order = Semester.Batch.batch_order;
            if (Semester.Batch.degree_level_id == 1)
            {
                int count = 7;
                if (order >= 10)
                {
                    count = 6;
                }
                int validOrder = order - count;
                if (validOrder <= 0)
                {
                    validOrder = 0;
                }
                var responseList = new SemesterPlanDetailsResponse
                {
                    spe = new List<SemesterPlanDetailsTermResponse>(),
                };
                responseList.semesterName = Semester.semester_name + " " + Semester.school_year;
                responseList.degreeLevel = Semester.Batch.DegreeLevel.degree_level_english_name;
                responseList.startDate = Semester.semester_start_date;
                responseList.endDate = Semester.semester_end_date;
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
                        no = 1,
                        courses = new List<DataTermNoResponse>(),
                    };
                    var dataTermNo = new DataTermNoResponse();
                    for (int i = 1; i <= item.count; i++)
                    {
                        var semesterPlan = _cmsDbContext.SemesterPlan
                            .Include(x => x.Curriculum)
                            .ThenInclude(x => x.Specialization)
                            .ThenInclude(x => x.Major)
                            .Include(x => x.Curriculum)
                            .ThenInclude(x => x.CurriculumSubjects)
                            .ThenInclude(x => x.Subject)
                            .ThenInclude(x => x.LearningMethod)
                            .Include(x => x.Curriculum)
                            .ThenInclude(x => x.CurriculumSubjects)
                            .ThenInclude(x => x.Subject)
                            .ThenInclude(x => x.PreRequisite)
                            .ThenInclude(x => x.PreSubject)
                            .Where(x => x.semester_id == semester_id)
                            .Where(x => x.curriculum_id == item.curriculumId)
                            .ToList();
                        var curriBatch = _cmsDbContext.CurriculumBatch.Include(x => x.Batch)
                            .Where(x => x.curriculum_id == item.curriculumId)
                            .Where(x => x.Batch.batch_order <= order && x.Batch.batch_order > validOrder)
                            .OrderByDescending(x => x.Batch.batch_order)
                            .ToList();
                        List<Semester> listSemesterValid = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.Batch.batch_order > validOrder && x.Batch.batch_order <= order && x.Batch.degree_level_id == Semester.Batch.degree_level_id).ToList();
                        semesterPlanDetails.specialization_name = semesterPlan[i - 1].Curriculum.Specialization.specialization_english_name;
                        semesterPlanDetails.major_name = semesterPlan[i - 1].Curriculum.Specialization.Major.major_english_name;
                        semesterPlanDetails.specializationId = semesterPlan[i - 1].Curriculum.Specialization.specialization_id;
                        semesterPlanDetails.validBatch = listSemesterValid.Select(x => new SemesterPlanBatchResponse() { batchName = x.Batch.batch_name, batchOrder = x.Batch.batch_order }).ToList();
                        dataTermNo = new DataTermNoResponse
                        {
                            term_no = semesterPlan[i - 1].term_no,
                            batch = curriBatch[i - 1].Batch.batch_name,
                            batch_order = curriBatch[i - 1].Batch.batch_order,
                            batch_check = curriBatch[i - 1].Batch.batch_name,
                            curriculum_code = semesterPlan[i - 1].Curriculum.curriculum_code,
                            subjectData = semesterPlan[i - 1].Curriculum.CurriculumSubjects
                                .Where(cs => cs.term_no == (order - curriBatch[i - 1].Batch.batch_order + 1) && cs.Subject.subject_code != null)
                                .Select(cs => new DataSubjectReponse
                                {
                                    subject_code = cs.Subject.subject_code,
                                    subject_name = cs.Subject.english_subject_name,
                                    credit = cs.Subject.credit,
                                    total = cs.Subject.total_time,
                                    @class = cs.Subject.total_time_class,
                                    @exam = cs.Subject.exam_total,
                                    method = cs.Subject.LearningMethod.learning_method_name,
                                    optional = cs.option.ToString(),
                                    combo = _cmsDbContext.Combo.Where(x => x.combo_id == cs.combo_id).Select(x => x.combo_english_name).FirstOrDefault(),
                                    pre = getPre(cs.Subject.subject_id)
                                })
                                .ToList(),
                        };
                        semesterPlanDetails.courses.Add(dataTermNo);
                    }

                    responseList.spe.Add(semesterPlanDetails);
                }
                return responseList;
            }
            else if (Semester.Batch.degree_level_id == 3)
            {
                int count = 2;
                if (Semester.Batch.batch_order < 5)
                {
                    count = 3;
                }
                int validOrder = order - count;
                if (validOrder <= 0)
                {
                    validOrder = 0;
                }
                var responseList = new SemesterPlanDetailsResponse
                {
                    spe = new List<SemesterPlanDetailsTermResponse>(),
                };
                responseList.semesterName = Semester.semester_name + " " + Semester.school_year;
                responseList.degreeLevel = Semester.Batch.DegreeLevel.degree_level_english_name;
                responseList.startDate = Semester.semester_start_date;
                responseList.endDate = Semester.semester_end_date;
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
                        no = (int)Semester.no,
                        courses = new List<DataTermNoResponse>(),
                    };
                    var dataTermNo = new DataTermNoResponse();
                    for (int i = 1; i <= item.count; i++)
                    {
                        var semesterPlan = _cmsDbContext.SemesterPlan
                            .Include(x => x.Curriculum)
                            .ThenInclude(x => x.Specialization)
                            .ThenInclude(x => x.Major)
                            .Include(x => x.Curriculum)
                            .ThenInclude(x => x.CurriculumSubjects)
                            .ThenInclude(x => x.Subject)
                            .ThenInclude(x => x.LearningMethod)
                            .Where(x => x.semester_id == semester_id)
                            .Where(x => x.curriculum_id == item.curriculumId)
                            .ToList();
                        var curriBatch = _cmsDbContext.CurriculumBatch.Include(x => x.Batch)
                            .Where(x => x.curriculum_id == item.curriculumId)
                            .Where(x => x.Batch.batch_order <= order && x.Batch.batch_order > validOrder)
                            .OrderByDescending(x => x.Batch.batch_order)
                            .ToList();
                        List<Semester> listSemesterValid = getValidSemester(Semester);
                        semesterPlanDetails.specialization_name = semesterPlan[i - 1].Curriculum.Specialization.specialization_english_name;
                        semesterPlanDetails.major_name = semesterPlan[i - 1].Curriculum.Specialization.Major.major_english_name;
                        semesterPlanDetails.specializationId = semesterPlan[i - 1].Curriculum.Specialization.specialization_id;
                        //(order - curriBatch[i - 1].Batch.batch_order + 1 + (int)Semester.no)
                        semesterPlanDetails.validBatch = listSemesterValid.Select(x => new SemesterPlanBatchResponse() { batchName = x.Batch.batch_name, batchOrder = x.Batch.batch_order }).ToList();
                        dataTermNo = new DataTermNoResponse
                        {
                            term_no = semesterPlan[i - 1].term_no,
                            batch = curriBatch[i - 1].Batch.batch_name,
                            batch_order = curriBatch[i - 1].Batch.batch_order,
                            batch_check = curriBatch[i - 1].Batch.batch_name,
                            curriculum_code = semesterPlan[i - 1].Curriculum.curriculum_code,
                            subjectData = semesterPlan[i - 1].Curriculum.CurriculumSubjects
                                .Where(cs => cs.term_no == semesterPlan[i - 1].term_no && cs.Subject.subject_code != null)
                                .Select(cs => new DataSubjectReponse
                                {
                                    subject_code = cs.Subject.subject_code,
                                    subject_name = cs.Subject.english_subject_name,
                                    credit = cs.Subject.credit,
                                    total = cs.Subject.total_time,
                                    @class = cs.Subject.total_time_class,
                                    @exam = cs.Subject.exam_total,
                                    method = cs.Subject.LearningMethod.learning_method_name,
                                    optional = cs.option.ToString(),
                                    combo = _cmsDbContext.Combo.Where(x => x.combo_id == cs.combo_id).Select(x => x.combo_english_name).FirstOrDefault(),
                                    pre = getPre(cs.Subject.subject_id),
                                })
                                .ToList(),
                        };
                        semesterPlanDetails.courses.Add(dataTermNo);
                    }

                    responseList.spe.Add(semesterPlanDetails);
                }
                return responseList;
            }
            return null;
        }

        public List<PreRequisiteResponse> getPre(int id)
        {
            var listPre = _cmsDbContext.PreRequisite.Include(x => x.PreSubject).Include(x => x.PreRequisiteType).Where(x => x.subject_id == id).ToList();
            List<PreRequisiteResponse> result = new List<PreRequisiteResponse>();
            if (listPre != null)
            {
                foreach (var item in listPre)
                {
                    PreRequisiteResponse pre = new PreRequisiteResponse();
                    pre.subject_id = item.subject_id;
                    pre.pre_subject_id = item.pre_subject_id;
                    pre.subject_code = item.PreSubject.subject_code;
                    pre.subject_name = item.PreSubject.english_subject_name;
                    pre.pre_requisite_type_id = item.pre_requisite_type_id;
                    pre.pre_requisite_type_name = item.PreRequisiteType.pre_requisite_type_name;
                    result.Add(pre);
                }

                return result;
            }
            return null;
        }
        public List<Semester> getValidSemester(Semester semester)
        {
            int count = 6;
            var startBatch = _cmsDbContext.Batch.FirstOrDefault(x => x.batch_id == semester.start_batch_id);
            int order = startBatch.batch_order;
            if (order < 5)
            {
                count = 8;
            }
            int validOrder = order - count;
            List<Semester> result = new List<Semester>();
            List<Semester> semesterValid = _cmsDbContext.Semester.Include(x => x.Batch).Where(x => x.Batch.degree_level_id == semester.Batch.degree_level_id).Where(x => x.no == 1).Where(x => x.Batch.batch_order > validOrder && x.Batch.batch_order <= order).OrderByDescending(x => x.Batch.batch_order).ToList();
            int termNO = 1 + (int)semester.no - 1;
            foreach (var batch in semesterValid)
            {
                if (termNO <= count)
                {
                    result.Add(batch);
                }
                termNO += 3;
            }
            return result;
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

        public string DeleteSemesterPlan(int semester_id)
        {
            try
            {
                List<SemesterPlan> semesterPlan = _cmsDbContext.SemesterPlan.Where(s => s.semester_id == semester_id).ToList();
                foreach (var item in semesterPlan)
                {

                    _cmsDbContext.SemesterPlan.Remove(item);
                    _cmsDbContext.SaveChanges();
                }
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





