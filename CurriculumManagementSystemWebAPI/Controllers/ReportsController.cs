﻿using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.Report;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.DegreeLevels;
using Repositories.LearningMethods;
using Repositories.LearningResources;
using Repositories.Major;
using Repositories.Materials;
using Repositories.Specialization;
using Repositories.Subjects;
using Repositories.Users;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private ISubjectRepository _subjectRepository;
        private ILearnningMethodRepository _learningMethodRepository;
        private IMajorRepository _majorRepository;
        private ISpecializationRepository _specializationRepository;
        private IDegreeLevelRepository _degreeLevelRepository;
        private IBatchRepository _batchRepository;
        private IMaterialRepository _materialRepository;
        private ILearningResourceRepository _learningResourceRepository;

        public ReportsController()
        {
            _subjectRepository = new SubjectRepository();
            _learningMethodRepository = new LearningMethodRepository();
            _majorRepository = new MajorRepository();
            _specializationRepository = new SpecializationRepository();
            _degreeLevelRepository = new DegreeLevelRepository();
            _batchRepository = new BatchRepository();
            _materialRepository = new MaterialRepository();
            _learningResourceRepository = new LearningResourceRepository();
        }

        [HttpGet("ReportTKOLChart/{batchId}/{SpeId}")]
        public IActionResult ReportTKOLChart(int batchId, int SpeId)
        {
            var spe = _specializationRepository.GetSpeById(SpeId);
            var batch = _batchRepository.GetBatchById(batchId);
            var subject = _subjectRepository.GetSubjectBySpecialization(spe.specialization_id, batch.batch_name);
            var tkolReport = new TKOLReport { specialization_name = spe.specialization_english_name, total_subject = subject.Count() };

            var learningMethod = _learningMethodRepository.GetAllLearningMethods();
            foreach (var item in learningMethod)
            {
                if (item.learning_method_code.Equals("T01"))
                {
                    tkolReport.learning_method_T01_name = item.learning_method_name;
                    tkolReport.total_subject_T01 = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                    if (!double.IsInfinity(tkolReport.total_subject_T01) && !double.IsNaN(tkolReport.total_subject_T01) && subject.Count() != 0)
                    {
                        tkolReport.ratio_T01 = ((double)tkolReport.total_subject_T01 / (double)subject.Count()) * 100;
                        tkolReport.ratio_T01 = Math.Round(tkolReport.ratio_T01, 2);
                    }
                }
                else if (item.learning_method_code.Equals("T02"))
                {
                    tkolReport.learning_method_T02_name = item.learning_method_name;
                    tkolReport.total_subject_T02 = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                    if (!double.IsInfinity(tkolReport.total_subject_T02) && !double.IsNaN(tkolReport.total_subject_T02) && subject.Count() != 0)
                    {
                        tkolReport.ratio_T02 = ((double)tkolReport.total_subject_T02 / (double)subject.Count()) * 100;
                        tkolReport.ratio_T02 = Math.Round(tkolReport.ratio_T02, 2);
                    }
                }
                else if (item.learning_method_code.Equals("T03"))
                {
                    tkolReport.learning_method_T03_name = item.learning_method_name;
                    tkolReport.total_subject_T03 = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                    if (!double.IsInfinity(tkolReport.total_subject_T03) && !double.IsNaN(tkolReport.total_subject_T03) && subject.Count() != 0)
                    {
                        tkolReport.ratio_T03 = ((double)tkolReport.total_subject_T03 / (double)subject.Count()) * 100;
                        tkolReport.ratio_T03 = Math.Round(tkolReport.ratio_T03, 2);
                    }
                }

            }
            return Ok(new BaseResponse(false, "TKOL Chart Report", tkolReport));
        }

        [HttpGet("ReportTKOLTable/{batchId}")]
        public IActionResult ReportTKOL(int batchId)
        {
            try
            {
                var listTKOLReport = new List<TKOL_DTOReport>();
                var batch = _batchRepository.GetBatchById(batchId);
                var degreeId = _degreeLevelRepository.GetDegreeIdByBatch(batchId);
                var major = _majorRepository.GetMajorByDegreeLevel(degreeId);
                var learningMethod = _learningMethodRepository.GetAllLearningMethods();
                foreach (var m in major)
                {
                    var spe = _specializationRepository.GetSpeByMajorId(m.major_id, batch.batch_name);
                    var tkolDTOReport = new TKOL_DTOReport
                    {
                        batch_name = batch.batch_name,
                        major_name = m.major_english_name,
                        learning_method_T01_name = learningMethod.FirstOrDefault(x => x.learning_method_code.Equals("T01")).learning_method_name,
                        learning_method_T02_name = learningMethod.FirstOrDefault(x => x.learning_method_code.Equals("T02")).learning_method_name,
                        learning_method_T03_name = learningMethod.FirstOrDefault(x => x.learning_method_code.Equals("T03")).learning_method_name,
                        tkol = new List<TKOLReport>()
                    };

                    foreach (var s in spe)
                    {
                        var subject = _subjectRepository.GetSubjectBySpecialization(s.specialization_id, batch.batch_name);
                        var tkolReport = new TKOLReport { specialization_name = s.specialization_english_name, total_subject = subject.Count() };


                        foreach (var item in learningMethod)
                        {
                            if (item.learning_method_code.Equals("T01"))
                            {
                                tkolReport.learning_method_T01_name = item.learning_method_name;
                                tkolReport.total_subject_T01 = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                                if (!double.IsInfinity(tkolReport.total_subject_T01) && !double.IsNaN(tkolReport.total_subject_T01) && subject.Count() != 0)
                                {
                                    tkolReport.ratio_T01 = ((double)tkolReport.total_subject_T01 / (double)subject.Count()) * 100;
                                    tkolReport.ratio_T01 = Math.Round(tkolReport.ratio_T01, 2);
                                }
                            }
                            else if (item.learning_method_code.Equals("T02"))
                            {
                                tkolReport.learning_method_T02_name = item.learning_method_name;
                                tkolReport.total_subject_T02 = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                                if (!double.IsInfinity(tkolReport.total_subject_T02) && !double.IsNaN(tkolReport.total_subject_T02) && subject.Count() != 0)
                                {
                                    tkolReport.ratio_T02 = ((double)tkolReport.total_subject_T02 / (double)subject.Count()) * 100;
                                    tkolReport.ratio_T02 = Math.Round(tkolReport.ratio_T02, 2);
                                }
                            }
                            else if (item.learning_method_code.Equals("T03"))
                            {
                                tkolReport.learning_method_T03_name = item.learning_method_name;
                                tkolReport.total_subject_T03 = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                                if (!double.IsInfinity(tkolReport.total_subject_T03) && !double.IsNaN(tkolReport.total_subject_T03) && subject.Count() != 0)
                                {
                                    tkolReport.ratio_T03 = ((double)tkolReport.total_subject_T03 / (double)subject.Count()) * 100;
                                    tkolReport.ratio_T03 = Math.Round(tkolReport.ratio_T03, 2);
                                }
                            }

                        }
                        tkolDTOReport.tkol.Add(tkolReport);
                    }
                    listTKOLReport.Add(tkolDTOReport);
                }

                return Ok(new BaseResponse(false, "TKOL Table Report", listTKOLReport));

            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, ex.InnerException.Message));
            }
        }

        [HttpGet("ReportTextBookTable/{batchId}")]
        public IActionResult ReportTextBook(int batchId)
        {
            var listTextBookReport = new List<TextBookDTOReport>();
            var batch = _batchRepository.GetBatchById(batchId);
            var degreeId = _degreeLevelRepository.GetDegreeIdByBatch(batchId);
            var listMajor = _majorRepository.GetMajorByDegreeLevel(degreeId);
            var listLearningResource = _learningResourceRepository.GetLearningResource();

            foreach (var major in listMajor)
            {
                var spe = _specializationRepository.GetSpeByMajorId(major.major_id, batch.batch_name);
                var textbook = new TextBookDTOReport
                {
                    batch_name = batch.batch_name,
                    major_name = major.major_english_name,
                    learning_resource_T01_name = listLearningResource.FirstOrDefault(x => x.learning_resouce_code.Equals("T01")).learning_resource_type,
                    learning_resource_T02_name = listLearningResource.FirstOrDefault(x => x.learning_resouce_code.Equals("T02")).learning_resource_type,
                    learning_resource_T03_name = listLearningResource.FirstOrDefault(x => x.learning_resouce_code.Equals("T03")).learning_resource_type,
                    learning_resource_T04_name = listLearningResource.FirstOrDefault(x => x.learning_resouce_code.Equals("T04")).learning_resource_type,
                    learning_resource_T05_name = listLearningResource.FirstOrDefault(x => x.learning_resouce_code.Equals("T05")).learning_resource_type,
                    learning_resource_T06_name = listLearningResource.FirstOrDefault(x => x.learning_resouce_code.Equals("T06")).learning_resource_type,
                    learning_resource_T07_name = "No Syllabus",
                    textBookReports = new List<TextBookReport>()
                };

                foreach (var s in spe)
                {
                    var listSubject = _subjectRepository.GetSubjectBySpecialization(s.specialization_id, batch.batch_name);
                    var textBookReport = new TextBookReport
                    {
                        specialization_name = s.specialization_english_name,
                        total_subject = listSubject.Count(),
                    };

                    var listMaterial = _materialRepository.GetMaterialListBysubject(listSubject);

                    foreach (var learningResource in listLearningResource)
                    {
                        string resourceCode = learningResource.learning_resouce_code;

                        switch (resourceCode)
                        {
                            case "T01":
                                textBookReport.learning_resource_T01_name = learningResource.learning_resource_type;
                                textBookReport.number_subject_T01 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                                break;

                            case "T02":
                                textBookReport.learning_resource_T02_name = learningResource.learning_resource_type;
                                textBookReport.number_subject_T02 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                                break;

                            case "T03":
                                textBookReport.learning_resource_T03_name = learningResource.learning_resource_type;
                                textBookReport.number_subject_T03 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                                break;

                            case "T04":
                                textBookReport.learning_resource_T04_name = learningResource.learning_resource_type;
                                textBookReport.number_subject_T04 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                                break;

                            case "T05":
                                textBookReport.learning_resource_T05_name = learningResource.learning_resource_type;
                                textBookReport.number_subject_T05 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                                break;

                            case "T06":
                                textBookReport.learning_resource_T06_name = learningResource.learning_resource_type;
                                textBookReport.number_subject_T06 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                                break;
                        }
                    }
                    textBookReport.learning_resource_T07_name = "No Syllabus";
                    textBookReport.number_subject_T07 = _subjectRepository.GetNumberSubjectNoSyllabus(listSubject);
                    textbook.textBookReports.Add(textBookReport);
                }

                listTextBookReport.Add(textbook);
            }

            return Ok(new BaseResponse(false, "Text Book Table Report", listTextBookReport));
        }

        private int GetMaterialCount(int id, List<Material> listMaterial)
        {
            return listMaterial.Count(x => !string.IsNullOrEmpty(x.material_purpose) && x.material_purpose.ToLower().Equals("textbook") && x.learning_resource_id == id);
        }

        [HttpGet("ReportTextBookChart/{batchId}/{speId}")]
        public IActionResult ReportTextBookChart(int batchId,int speId)
        {
            var spe = _specializationRepository.GetSpeById(speId);
            var batch = _batchRepository.GetBatchById(batchId);
            var listSubject = _subjectRepository.GetSubjectBySpecialization(spe.specialization_id, batch.batch_name);
            var textBookReport = new TextBookReport
            {
                specialization_name = spe.specialization_english_name,
                total_subject = listSubject.Count(),
            };

            var listMaterial = _materialRepository.GetMaterialListBysubject(listSubject);
            var listLearningResource = _learningResourceRepository.GetLearningResource();

            foreach (var learningResource in listLearningResource)
            {
                string resourceCode = learningResource.learning_resouce_code;

                switch (resourceCode)
                {
                    case "T01":
                        textBookReport.learning_resource_T01_name = learningResource.learning_resource_type;
                        textBookReport.number_subject_T01 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                        break;

                    case "T02":
                        textBookReport.learning_resource_T02_name = learningResource.learning_resource_type;
                        textBookReport.number_subject_T02 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                        break;

                    case "T03":
                        textBookReport.learning_resource_T03_name = learningResource.learning_resource_type;
                        textBookReport.number_subject_T03 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                        break;

                    case "T04":
                        textBookReport.learning_resource_T04_name = learningResource.learning_resource_type;
                        textBookReport.number_subject_T04 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                        break;

                    case "T05":
                        textBookReport.learning_resource_T05_name = learningResource.learning_resource_type;
                        textBookReport.number_subject_T05 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                        break;

                    case "T06":
                        textBookReport.learning_resource_T06_name = learningResource.learning_resource_type;
                        textBookReport.number_subject_T06 = GetMaterialCount(learningResource.learning_resource_id, listMaterial);
                        break;
                }
            }
            textBookReport.learning_resource_T07_name = "No Syllabus";
            textBookReport.number_subject_T07 = _subjectRepository.GetNumberSubjectNoSyllabus(listSubject);

            return Ok(new BaseResponse(false, "Text Book Table Report", textBookReport));
        }



        [HttpGet("ReportSubject")]
        public IActionResult ReportSubject()
        {
            int totalSubject = _subjectRepository.GetAllSubject("").Count;
            var listSubjectReport = new List<SubjectDTOReport>();
            var learningMethod = _learningMethodRepository.GetAllLearningMethods();
            foreach (var item in learningMethod)
            {
                var subjectReport = new SubjectDTOReport();
                subjectReport.learning_method_name = item.learning_method_name;
                subjectReport.total_subject = _subjectRepository.GetSubjectByLearningMethod(item.learning_method_id).Count;
                subjectReport.ratio = ((double)subjectReport.total_subject / (double)totalSubject) * 100;
                subjectReport.ratio = Math.Round(subjectReport.ratio, 2);

                listSubjectReport.Add(subjectReport);
            }

            return Ok(new BaseResponse(false, "Subject Report", listSubjectReport));
        }


    }
}
