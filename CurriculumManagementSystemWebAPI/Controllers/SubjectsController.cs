using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using AutoMapper;
using Repositories.Subjects;
using DataAccess.Models.DTO.response;
using DataAccess.Models.DTO.request;
using MiniExcelLibs;
using Newtonsoft.Json.Linq;
using Repositories.PreRequisites;
using DataAccess.Models.Enums;
using Repositories.CurriculumSubjects;
using Microsoft.AspNetCore.Authorization;
using DataAccess.Models.DTO.Excel;
using Repositories.LearningResources;
using Repositories.LearningMethods;
using Repositories.AssessmentMethods;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager, Dispatcher")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISubjectRepository _subjectRepository = new SubjectRepository();
        private readonly ICurriculumSubjectRepository _curriSubjectRepository = new CurriculumSubjectRepository();
        private readonly IPreRequisiteRepository _preRequisiteRepository = new PreRequisiteRepository();
        private readonly ILearnningMethodRepository _learningMethodRepository = new LearningMethodRepository();
        private readonly IAssessmentMethodRepository _assessmentMethodRepository = new AssessmentMethodRepository();
        public SubjectsController(IMapper mapper)

        {
            _mapper = mapper;
        }


        // GET: api/Subjects
        [HttpGet("GetAllSubject")]
        public async Task<ActionResult<IEnumerable<SubjectResponse>>> GetSubject([FromQuery] string? txtSearch)
        {
            var subject = _subjectRepository.GetAllSubject(txtSearch);
            if (subject.Count == 0)
            {
                return BadRequest(new BaseResponse(true, "List Subject is Empty. Please create new subject!"));
            }
            var subjectRespone = _mapper.Map<List<SubjectResponse>>(subject);
            return Ok(new BaseResponse(false, "Success!", subjectRespone));
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public async Task<IActionResult> PaginationSubject(int page, int limit, [FromQuery] string? txtSearch)
        {
            List<Subject> subject = _subjectRepository.PaginationSubject(page, limit, txtSearch);

            var subjectResponse = _mapper.Map<List<SubjectResponse>>(subject);
            foreach (var subjectRespones in subjectResponse)
            {
                var prerequisites = _preRequisiteRepository.GetPreRequisitesBySubject(subjectRespones.subject_id);
                subjectRespones.prerequisites = _mapper.Map<List<PreRequisiteResponse>>(prerequisites);
            }

            var paginationResponse = new PaginationResponse<SubjectResponse>
            {
                Page = page,
                Limit = limit,
                TotalElements = _subjectRepository.GetTotalSubject(txtSearch),
                Data = subjectResponse
            };

            return Ok(paginationResponse);
        }

        // GET: api/Subjects/5
        [HttpGet("GetSubjectById/{id}")]
        public async Task<ActionResult<SubjectResponse>> GetSubjectById(int id)
        {
            var subject = _subjectRepository.GetSubjectById(id);

            if (subject == null)
            {
                return NotFound(new BaseResponse(true, "Can't Found this subject"));
            }
            var subjectResponse = _mapper.Map<SubjectResponse>(subject);
            return Ok(new BaseResponse(false, "Success!", subjectResponse));
        }


        // GET: api/Subjects/5
        [HttpGet("GetSubjectBySyllabus/{syllabus_id}")]
        public async Task<ActionResult<SubjectResponse>> GetSubjectBySyllabus(int syllabus_id)
        {
            var subject = _subjectRepository.GetSubjectBySyllabus(syllabus_id);

            if (subject == null)
            {
                return NotFound(new BaseResponse(true, "Can't Found this subject"));
            }
            var subjectResponse = _mapper.Map<SubjectResponse>(subject);
            return Ok(new BaseResponse(false, "Success!", subjectResponse));
        }


        [HttpPost("CreateSubjectWithPrerequisites")]
        public async Task<ActionResult<Subject>> PostSubjectWithPrerequisites([FromBody] SubjectPreRequisiteRequest subjectPreRequisitesRequest)
        {
            subjectPreRequisitesRequest.SubjectRequest.is_active = true;
            var subject = _mapper.Map<Subject>(subjectPreRequisitesRequest.SubjectRequest);
            if (CheckCodeExist(subject.subject_code))
            {
                return BadRequest(new BaseResponse(true, $"Subject {subject.subject_code} is Duplicate!"));
            }
            string createResult = _subjectRepository.CreateNewSubject(subject);

            if (!createResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }
            if (subjectPreRequisitesRequest.PreRequisiteRequest.Count != 0)
            {
                var preRequisite = _mapper.Map<List<PreRequisite>>(subjectPreRequisitesRequest.PreRequisiteRequest);

                foreach (PreRequisite prere in preRequisite)
                {
                    prere.subject_id = subject.subject_id;
                    string createPreResult = _preRequisiteRepository.CreatePreRequisite(prere);
                    if (!createPreResult.Equals(Result.createSuccessfull.ToString()))
                    {
                        return BadRequest(new BaseResponse(true, createPreResult));
                    }
                }
            }

            return Ok(new BaseResponse(false, $"Create Subject {subject.subject_code} successfull!", subjectPreRequisitesRequest));
        }


        [HttpPut("EditSubjectWithPrerequisites/{id}")]
        public async Task<ActionResult<Subject>> EditSubjectWithPrerequisites(int id, [FromBody] SubjectPreRequisiteRequest subjectPreRequisitesRequest)
        {
            if (!CheckIdExist(id))
            {
                return NotFound(new BaseResponse(true, "Cannot Found this subject"));
            }

            var subject = _subjectRepository.GetSubjectById(id);
            _mapper.Map(subjectPreRequisitesRequest.SubjectRequest, subject);
            
            if(_subjectRepository.CheckSubjectCodeUpdateDuplicate(subject.subject_id, subject.subject_code))
            {
                return BadRequest(new BaseResponse(true, $"Subject Code {subject.subject_code} is Duplicate!"));
            }

            string updateResult = _subjectRepository.UpdateSubject(subject);
            if (!updateResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            var listPreRequisite = _preRequisiteRepository.GetPreRequisitesBySubject(id);
            if (listPreRequisite.Count > 0)
            {
                foreach (var prere in listPreRequisite)
                {
                    _preRequisiteRepository.DeletePreRequisite(prere);
                }
            }

            var preRequisite = _mapper.Map<List<PreRequisite>>(subjectPreRequisitesRequest.PreRequisiteRequest);
            foreach (var prere in preRequisite)
            {
                prere.subject_id = id;
                _preRequisiteRepository.CreatePreRequisite(prere);
            }


            return Ok(new BaseResponse(false, $"Edit subject {subject.subject_code} successful!", subjectPreRequisitesRequest));
        }



        // DELETE: api/Subjects/5

        [HttpDelete("DeleteSubject/{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {

            var subject = _subjectRepository.GetSubjectById(id);
            var subjectRespone = _mapper.Map<SubjectResponse>(subject);
            // if subject not exsit
            if (subject == null)
            {
                return NotFound(new BaseResponse(true, "Subject you want delete Not Found!"));
            }

            var checkSubject = _curriSubjectRepository.GetListCurriculumBySubject(id);
            if (checkSubject.Count != 0)
            {
                return BadRequest(new BaseResponse(true, "Subject used by curriculum. Can't Delete!"));
            }

            if (CheckIdExistInSyllabus(id))
            {
                return BadRequest(new BaseResponse(true, "Subject used by Syllabus. Can't Delete!"));
            }

            // Delete foreign key of subject
            var preRequisite = _preRequisiteRepository.GetPreRequisitesBySubject(id);
            if (preRequisite != null)
            {
                foreach (var prere in preRequisite)
                {
                    string deletePreRequisiteResult = _preRequisiteRepository.DeletePreRequisite(prere);
                    // if delete foreign key fail
                    if (deletePreRequisiteResult != Result.deleteSuccessfull.ToString())
                    {
                        return BadRequest(new BaseResponse(true, deletePreRequisiteResult));
                    }
                }
            }
            // delete subject
            string deleteResult = _subjectRepository.DeleteSubject(subject);
            if (!deleteResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }


            return Ok(new BaseResponse(false, $"Delete Subject {subjectRespone.subject_code} successfull!", subjectRespone));
        }

        //Import Subject by Excel
        [HttpPost("ImportSubject")]
        public async Task<IActionResult> ImportSubject(IFormFile fileSubject)
        {
            string error = "";
            try
            {
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileSubject.CopyToAsync(stream);
                    //Get SheetName
                    var sheetNames = MiniExcel.GetSheetNames(filePath);
                    var row = MiniExcel.Query<SubjectImportExcel>(filePath, sheetName: sheetNames[0], excelType: ExcelType.XLSX);
                    foreach (var subject in row)
                    {
                        subject.credit = string.IsNullOrEmpty(subject.credit) ? "0" : subject.credit;
                        error = $"Error in Subject {subject.subject_code}: ";
                        var learningMethod = _learningMethodRepository.GetLearningMethodByName(subject.learning_method);
                        if(learningMethod == null)
                        {
                            return BadRequest(new BaseResponse(true, error + $"Learning Method '{subject.learning_method}' Not Found"));
                        }
                        var assessmentMethod = _assessmentMethodRepository.GetAsssentMethodByName(subject.assessment_method);
                        if (assessmentMethod == null)
                        {
                            return BadRequest(new BaseResponse(true, error + $"Assessment Method '{subject.assessment_method}' Not Found"));
                        }
                        var s = new Subject
                        {
                            subject_code = subject.subject_code,
                            credit = int.Parse(subject.credit),
                            exam_total = float.Parse(subject.exam_time),
                            total_time = float.Parse(subject.total_time),
                            total_time_class = float.Parse(subject.class_time),
                            is_active = true,
                            subject_name = subject.subject_name,
                            english_subject_name = subject.english_subject_name,
                            learning_method_id = learningMethod.learning_method_id,
                            assessment_method_id = assessmentMethod.assessment_method_id
                        };
                        if(s.total_time < s.total_time_class + s.exam_total)
                        {
                            return BadRequest(new BaseResponse(true, error + "Total Time must greater or equal than sum of Class Time and Exam Time"));
                        }
                        if(!CheckCodeExist(subject.subject_code))
                        {
                            string createResult = _subjectRepository.CreateNewSubject(s);

                            if (!createResult.Equals("OK"))
                            {
                                return BadRequest(new BaseResponse(true, createResult));
                            }
                        }
                    }
                }

                return Ok(new BaseResponse(false, "Import Subject Successfull!"));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, error + ex.Message + " or check table name match in Sample File!"));
            }
        }

        private bool CheckIdExistInSyllabus(int id)
        {
            return _subjectRepository.CheckIdExistInSyllabus(id);
        }

        [NonAction]
        public bool CheckIdExist(int id)
        {
            return _subjectRepository.CheckIdExist(id);

        }

        [NonAction]
        public bool CheckCodeExist(string code)
        {
            return _subjectRepository.CheckCodeExist(code);

        }

        [NonAction]
        public bool CheckSubjectExist(int subject_id)
        {
            return _subjectRepository.CheckSubjectExist(subject_id);
        }
    }
}
