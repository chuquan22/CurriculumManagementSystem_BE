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

        [HttpGet("ReportTKOLChart/{batchId}/{SpeId}")]
        public IActionResult ReportTKOLChart(int batchId, int SpeId)
        {
            var spe = _specializationRepository.GetSpeById(SpeId);
            var subject = _subjectRepository.GetSubjectBySpecialization(spe.specialization_id, batchId);
            var tkolReport = new TKOLReport { specialization_name = spe.specialization_english_name, total_subject = subject.Count() };

            var learningMethod = _learningMethodRepository.GetAllLearningMethods();
            foreach (var item in learningMethod)
            {
                if (item.learning_method_name.ToLower().Contains("online"))
                {
                    tkolReport.learning_method_onl_name = item.learning_method_name;
                    tkolReport.total_subject_onl = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                    if (!double.IsInfinity(tkolReport.total_subject_onl) && !double.IsNaN(tkolReport.total_subject_onl) && subject.Count() != 0)
                    {
                        tkolReport.ratio_onl = ((double)tkolReport.total_subject_onl / (double)subject.Count()) * 100;
                        tkolReport.ratio_onl = Math.Round(tkolReport.ratio_onl, 2);
                    }
                }
                else if (item.learning_method_name.ToLower().Equals("blended"))
                {
                    tkolReport.learning_method_blended_name = item.learning_method_name;
                    tkolReport.total_subject_blended = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                    if (!double.IsInfinity(tkolReport.total_subject_blended) && !double.IsNaN(tkolReport.total_subject_blended) && subject.Count() != 0)
                    {
                        tkolReport.ratio_blended = ((double)tkolReport.total_subject_blended / (double)subject.Count()) * 100;
                        tkolReport.ratio_blended = Math.Round(tkolReport.ratio_blended, 2);
                    }
                }
                else if (item.learning_method_name.ToLower().Equals("traditional"))
                {
                    tkolReport.learning_method_traditional_name = item.learning_method_name;
                    tkolReport.total_subject_traditional = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                    if (!double.IsInfinity(tkolReport.total_subject_traditional) && !double.IsNaN(tkolReport.total_subject_traditional) && subject.Count() != 0)
                    {
                        tkolReport.ratio_traditional = ((double)tkolReport.total_subject_traditional / (double)subject.Count()) * 100;
                        tkolReport.ratio_traditional = Math.Round(tkolReport.ratio_traditional, 2);
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
                        var subject = _subjectRepository.GetSubjectBySpecialization(s.specialization_id, batchId);
                        var tkolReport = new TKOLReport { specialization_name = s.specialization_english_name, total_subject = subject.Count() };

                        var learningMethod = _learningMethodRepository.GetAllLearningMethods();
                        foreach (var item in learningMethod)
                        {
                            if (item.learning_method_name.ToLower().Contains("online"))
                            {
                                tkolReport.learning_method_onl_name = item.learning_method_name;
                                tkolReport.total_subject_onl = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                                if (!double.IsInfinity(tkolReport.total_subject_onl) && !double.IsNaN(tkolReport.total_subject_onl) && subject.Count() != 0)
                                {
                                    tkolReport.ratio_onl = ((double)tkolReport.total_subject_onl / (double)subject.Count()) * 100;
                                    tkolReport.ratio_onl = Math.Round(tkolReport.ratio_onl, 2);
                                }
                            }
                            else if (item.learning_method_name.ToLower().Equals("blended"))
                            {
                                tkolReport.learning_method_blended_name = item.learning_method_name;
                                tkolReport.total_subject_blended = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                                if (!double.IsInfinity(tkolReport.total_subject_blended) && !double.IsNaN(tkolReport.total_subject_blended) && subject.Count() != 0)
                                {
                                    tkolReport.ratio_blended = ((double)tkolReport.total_subject_blended / (double)subject.Count()) * 100;
                                    tkolReport.ratio_blended = Math.Round(tkolReport.ratio_blended, 2);
                                }
                            }
                            else if (item.learning_method_name.ToLower().Equals("traditional"))
                            {
                                tkolReport.learning_method_traditional_name = item.learning_method_name;
                                tkolReport.total_subject_traditional = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                                if (!double.IsInfinity(tkolReport.total_subject_traditional) && !double.IsNaN(tkolReport.total_subject_traditional) && subject.Count() != 0)
                                {
                                    tkolReport.ratio_traditional = ((double)tkolReport.total_subject_traditional / (double)subject.Count()) * 100;
                                    tkolReport.ratio_traditional = Math.Round(tkolReport.ratio_traditional, 2);
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
                    var listSubject = _subjectRepository.GetSubjectBySpecialization(s.specialization_id, batchId);
                    var textBookReport = new TextBookReport
                    {
                        specialization_name = s.specialization_english_name,
                        total_subject = listSubject.Count(),
                        LearningResource = new List<LearningResourceReport>()
                    };

                    foreach (var subject in listSubject)
                    {
                        var listMaterial = _materialRepository.GetMaterialListBysubject(subject.subject_id);
                        var listLearningResource = _learningResourceRepository.GetLearningResource();

                        foreach (var learningResource in listLearningResource)
                        {
                            var learningResourceReport = new LearningResourceReport();
                            var number = listMaterial.Where(x => x.learning_resource_id == learningResource.learning_resource_id).Count();
                            learningResourceReport.learning_resouce_name = learningResource.learning_resource_type;
                            if (textBookReport.LearningResource.FirstOrDefault(x => x.learning_resouce_name.Equals(learningResourceReport.learning_resouce_name)) == null)
                            {
                                learningResourceReport.number_subject = number;
                                textBookReport.LearningResource.Add(learningResourceReport);
                            }
                            else
                            {
                                textBookReport.LearningResource.FirstOrDefault(x => x.learning_resouce_name.Equals(learningResourceReport.learning_resouce_name)).number_subject += number;
                            }
                        }
                    }

                    textbook.textBookReports.Add(textBookReport);
                }

                listTextBookReport.Add(textbook);
            }

            return Ok(new BaseResponse(false, "Text Book Table Report", listTextBookReport));
        }


        [HttpGet("ReportSubject")]
        public IActionResult ReportSubject()
        {
            int totalSubject = _subjectRepository.GetAllSubject().Count;
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
