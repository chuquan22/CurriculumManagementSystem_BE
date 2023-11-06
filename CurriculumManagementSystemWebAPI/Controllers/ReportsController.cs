using AutoMapper;
using DataAccess.Models.DTO.Report;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.DegreeLevels;
using Repositories.LearningMethods;
using Repositories.Major;
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

        public ReportsController()
        {
            _subjectRepository = new SubjectRepository();
            _learningMethodRepository = new LearningMethodRepository();
            _majorRepository = new MajorRepository();
            _specializationRepository = new SpecializationRepository();
            _degreeLevelRepository = new DegreeLevelRepository();
            _batchRepository = new BatchRepository();
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
                        var tkolReport = new TKOLReport { specialization_name = s.specialization_english_name, total_subject = subject.Count(), subjects = new List<SubjectDTOReport>() };

                        var learningMethod = _learningMethodRepository.GetAllLearningMethods();
                        foreach (var item in learningMethod)
                        {
                            var subjectReport = new SubjectDTOReport { learning_method_name = item.learning_method_name };
                            subjectReport.total_subject = subject.Where(x => x.learning_method_id == item.learning_method_id).Count();

                            if (!double.IsInfinity(subjectReport.total_subject) && !double.IsNaN(subjectReport.total_subject) && subject.Count() != 0)
                            {
                                subjectReport.ratio = ((double)subjectReport.total_subject / (double)subject.Count()) * 100;
                                subjectReport.ratio = Math.Round(subjectReport.ratio, 2);
                            }

                            tkolReport.subjects.Add(subjectReport);
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

        [HttpGet("ReportTextBook/{batchId}")]
        public IActionResult ReportTextBook(int batchId)
        {
            var ListTextBookReport = new List<TextBookDTOReport>();
            var batch = _batchRepository.GetBatchById(batchId);
            var degreeId = _degreeLevelRepository.GetDegreeIdByBatch(batchId);
            var listMajor = _majorRepository.GetMajorByDegreeLevel(degreeId);
            foreach (var major in listMajor)
            {
                var spe = _specializationRepository.GetSpeByMajorId(major.major_id);
                var textbook = new TextBookDTOReport { batch_name = batch.batch_name, major_name = major.major_name, textBookReports = new List<TextBookReport>() };


            }

            return Ok();
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
