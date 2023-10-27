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

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISubjectRepository _subjectRepository = new SubjectRepository();
        private readonly ICurriculumSubjectRepository _curriSubjectRepository = new CurriculumSubjectRepository();
        private readonly IPreRequisiteRepository _preRequisiteRepository = new PreRequisiteRepository();

        public SubjectsController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Subjects
        [HttpGet("GetAllSubject")]
        public async Task<ActionResult<IEnumerable<SubjectResponse>>> GetSubject()
        {
            if (_context.Subject == null)
            {
                return NotFound();
            }
            var subject = _subjectRepository.GetAllSubject();
            if (subject == null)
            {
                return BadRequest(new BaseResponse(true, "List Subject is Empty. Please create new subject!"));
            }
            var subjectRespone = _mapper.Map<List<SubjectResponse>>(subject);
            return Ok(new BaseResponse(false, "Success!", subjectRespone));
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public async Task<IActionResult> PaginationSubject(int page, int limit, [FromQuery] string? txtSearch)
        {
            IQueryable<Subject> subjectQuery = _context.Subject;

            if (!string.IsNullOrWhiteSpace(txtSearch))
            {
                subjectQuery = subjectQuery.Where(x => x.subject_code.Contains(txtSearch) || x.subject_name.Contains(txtSearch) || x.english_subject_name.Contains(txtSearch));
            }

            var totalElements = subjectQuery.Count(); 
            var subject = subjectQuery.Skip((page - 1) * limit).Take(limit)
                .Include(x => x.PreRequisite)
                .Include(x => x.AssessmentMethod.AssessmentType)
                .Include(x => x.LearningMethod)
                .ToList();


            var subjectResponse = _mapper.Map<List<SubjectResponse>>(subject);
            foreach(var subjectRespones in subjectResponse)
            {
                var prerequisites = _preRequisiteRepository.GetPreRequisitesBySubject(subjectRespones.subject_id);
                subjectRespones.prerequisites = _mapper.Map<List<PreRequisiteResponse>>(prerequisites);
            }

            var paginationResponse = new PaginationResponse<SubjectResponse>
            {
                Page = page,
                Limit = limit,
                TotalElements = totalElements,
                Data = subjectResponse
            };

            return Ok(paginationResponse);
        }

        // GET: api/Subjects/5
        [HttpGet("GetSubjectById/{id}")]
        public async Task<ActionResult<SubjectResponse>> GetSubject(int id)
        {
            if (_context.Subject == null)
            {
                return NotFound();
            }
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
                return BadRequest(new BaseResponse(true, "Subject had Exsited!"));
            }
            string createResult = _subjectRepository.CreateNewSubject(subject);

            if (!createResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }
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

            return Ok(new BaseResponse(false, "create subject with preRequisite successfull!", subjectPreRequisitesRequest));
        }


        [HttpPut("EditSubjectWithPrerequisites/{id}")]
        public async Task<ActionResult<Subject>> EditSubjectWithPrerequisites(int id, [FromBody] SubjectPreRequisiteRequest subjectPreRequisitesRequest)
        {
            if(!CheckIdExist(id))
            {
                return NotFound(new BaseResponse(true, "Cannot Found this subject"));
            }
           
            var subject = _subjectRepository.GetSubjectById(id);
            _mapper.Map(subjectPreRequisitesRequest.SubjectRequest, subject);

            string updateResult = _subjectRepository.UpdateSubject(subject);
            if (!updateResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }
            
            var listPreRequisite = _preRequisiteRepository.GetPreRequisitesBySubject(id);
            if(listPreRequisite.Count > 0)
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


            return Ok(new BaseResponse(false, "Edit subject with prerequisites successful!", subjectPreRequisitesRequest));
        }



        // DELETE: api/Subjects/5

        [HttpDelete("DeleteSubject/{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            
            var subject = _subjectRepository.GetSubjectById(id);
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
            

            return Ok(new BaseResponse(false, "Delete successfull!", id));
        }


        [NonAction]
        public bool CheckIdExist(int id)
        {
            if (_context.Subject.Find(id) == null) return false;
            return true;
        }

        [NonAction]
        public bool CheckCodeExist(string code)
        {
            var subject = _context.Subject.FirstOrDefault(x => x.subject_code.Equals(code));
            if (subject == null) return false;
            return true;
        }
    }
}
