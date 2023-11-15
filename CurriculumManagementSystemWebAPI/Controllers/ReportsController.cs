using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.Report;
using DataAccess.Models.DTO.response;
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

        [HttpGet("ReportTKOLChart/{SpeId}")]
        public IActionResult ReportTKOLChart(int SpeId)
        {
            var spe = _specializationRepository.GetSpeById(SpeId);
            var subject = _subjectRepository.GetSubjectBySpecialization(spe.specialization_id);
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
                foreach (var m in major)
                {
                    var spe = _specializationRepository.GetSpeByMajorId(m.major_id);
                    var tkolDTOReport = new TKOL_DTOReport { batch_name = batch.batch_name, major_name = m.major_english_name, tkol = new List<TKOLReport>() };

                    foreach (var s in spe)
                    {
                        var subject = _subjectRepository.GetSubjectBySpecialization(s.specialization_id);
                        var tkolReport = new TKOLReport { specialization_name = s.specialization_english_name, total_subject = subject.Count() };

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

            foreach (var major in listMajor)
            {
                var spe = _specializationRepository.GetSpeByMajorId(major.major_id);
                var textbook = new TextBookDTOReport
                {
                    batch_name = batch.batch_name,
                    major_name = major.major_name,
                    textBookReports = new List<TextBookReport>()
                };

                foreach (var s in spe)
                {
                    var listSubject = _subjectRepository.GetSubjectBySpecialization(s.specialization_id);
                    var textBookReport = new TextBookReport
                    {
                        specialization_name = s.specialization_english_name,
                        total_subject = listSubject.Count(),
                    };

                    var listMaterial = _materialRepository.GetMaterialListBysubject(listSubject);
                    var listLearningResource = _learningResourceRepository.GetLearningResource();

                    foreach (var learningResource in listLearningResource)
                    {
                        if (learningResource.learning_resouce_code.Equals("T01"))
                        {
                            textBookReport.learning_resource_T01_name = learningResource.learning_resource_type;
                            textBookReport.number_subject_T01 = listMaterial.Where(x => x.material_purpose.Equals("Textbook") && x.learning_resource_id == learningResource.learning_resource_id).Count();
                        }
                        else if (learningResource.learning_resouce_code.Equals("T02"))
                        {
                            textBookReport.learning_resource_T02_name = learningResource.learning_resource_type;
                            textBookReport.number_subject_T02 = listMaterial.Where(x => x.material_purpose.Equals("Textbook") && x.learning_resource_id == learningResource.learning_resource_id).Count();
                        }
                        else if (learningResource.learning_resouce_code.Equals("T03"))
                        {
                            textBookReport.learning_resource_T03_name = learningResource.learning_resource_type;
                            textBookReport.number_subject_T03 = listMaterial.Where(x => x.material_purpose.Equals("Textbook") && x.learning_resource_id == learningResource.learning_resource_id).Count();
                        }
                        else if (learningResource.learning_resouce_code.Equals("T04"))
                        {
                            textBookReport.learning_resource_T04_name = learningResource.learning_resource_type;
                            textBookReport.number_subject_T04 = listMaterial.Where(x => x.material_purpose.Equals("Textbook") && x.learning_resource_id == learningResource.learning_resource_id).Count();
                        }
                        else if (learningResource.learning_resouce_code.Equals("T05"))
                        {
                            textBookReport.learning_resource_T05_name = learningResource.learning_resource_type;
                            textBookReport.number_subject_T05 = listMaterial.Where(x => x.material_purpose.Equals("Textbook") && x.learning_resource_id == learningResource.learning_resource_id).Count();
                        }
                    }
                    textBookReport.learning_resource_T06_name = "No Syllabus";
                    textBookReport.number_subject_T06 = _subjectRepository.GetNumberSubjectNoSyllabus(listSubject);
                    textbook.textBookReports.Add(textBookReport);
                }

                listTextBookReport.Add(textbook);
            }

            return Ok(new BaseResponse(false, "Text Book Table Report", listTextBookReport));
        }

        [HttpGet("ReportTextBookChart/{speId}")]
        public IActionResult ReportTextBookChart(int speId)
        {
            var spe = _specializationRepository.GetSpeById(speId);

            var listSubject = _subjectRepository.GetSubjectBySpecialization(spe.specialization_id);
            var textBookReport = new TextBookReport
            {
                specialization_name = spe.specialization_english_name,
                total_subject = listSubject.Count(),
            };

            var listMaterial = _materialRepository.GetMaterialListBysubject(listSubject);
            var listLearningResource = _learningResourceRepository.GetLearningResource();

            foreach (var learningResource in listLearningResource)
            {
                if (learningResource.learning_resouce_code.Equals("T01"))
                {
                    textBookReport.learning_resource_T01_name = learningResource.learning_resource_type;
                    textBookReport.number_subject_T01 = listMaterial.Where(x => x.material_purpose.Equals("Textbook") && x.learning_resource_id == learningResource.learning_resource_id).Count();
                }
                else if (learningResource.learning_resouce_code.Equals("T02"))
                {
                    textBookReport.learning_resource_T02_name = learningResource.learning_resource_type;
                    textBookReport.number_subject_T02 = listMaterial.Where(x => x.material_purpose.Equals("Textbook") && x.learning_resource_id == learningResource.learning_resource_id).Count();
                }
                else if (learningResource.learning_resouce_code.Equals("T03"))
                {
                    textBookReport.learning_resource_T03_name = learningResource.learning_resource_type;
                    textBookReport.number_subject_T03 = listMaterial.Where(x => x.material_purpose.Equals("Textbook") && x.learning_resource_id == learningResource.learning_resource_id).Count();
                }
                else if (learningResource.learning_resouce_code.Equals("T04"))
                {
                    textBookReport.learning_resource_T04_name = learningResource.learning_resource_type;
                    textBookReport.number_subject_T04 = listMaterial.Where(x => x.material_purpose.Equals("Textbook") && x.learning_resource_id == learningResource.learning_resource_id).Count();
                }
                else if (learningResource.learning_resouce_code.Equals("T05"))
                {
                    textBookReport.learning_resource_T05_name = learningResource.learning_resource_type;
                    textBookReport.number_subject_T05 = listMaterial.Where(x => x.material_purpose.Equals("Textbook") && x.learning_resource_id == learningResource.learning_resource_id).Count();
                }
            }
            textBookReport.learning_resource_T06_name = "no syllabus";
            textBookReport.number_subject_T06 = _subjectRepository.GetNumberSubjectNoSyllabus(listSubject);

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
