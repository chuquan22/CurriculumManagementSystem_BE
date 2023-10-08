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
using CurriculumManagementSystemWebAPI.Models.DTO.response;
using CurriculumManagementSystemWebAPI.Models.DTO.request;
using MiniExcelLibs;
using Newtonsoft.Json.Linq;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISubjectRepository _subjectRepository = new SubjectRepository();

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

        [HttpGet("Pagination/{page}/{limit}/{txtSearch}")]
        public async Task<ActionResult<IEnumerable<SubjectResponse>>> PaginationSubject(int page, int limit, string txtSearch)
        {
            if (_context.Subject == null)
            {
                return NotFound();
            }

            var subject = _context.Subject.Where(x => x.subject_code.Contains(txtSearch) || x.subject_name.Contains(txtSearch) || x.english_subject_name.Contains(txtSearch))
                .Skip((page - 1) * limit).Take(limit)
                .Include(x => x.AssessmentMethod)
                .Include(x => x.LearningMethod)
                .ToList();

            if (subject == null)
            {
                return BadRequest(new BaseResponse(true, "List Subject is Empty. Please create new subject!"));
            }
            var subjectRespone = _mapper.Map<List<SubjectResponse>>(subject);
            return Ok(new BaseListResponse(page, limit, subjectRespone));
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


        // GET: api/Subjects/abc
        [HttpGet("SearchSubjectByName/{subjectName}")]
        public async Task<ActionResult<SubjectResponse>> SearchSubject(string subjectName)
        {
            if (_context.Subject == null)
            {
                return NotFound();
            }
            var subject = _subjectRepository.GetSubjectByName(subjectName);

            if (subject == null)
            {
                return NotFound(new BaseResponse(true, "Can't Found this subject"));
            }
            var subjectResponse = _mapper.Map<List<SubjectResponse>>(subject);
            return Ok(new BaseResponse(false, "Success!", subjectResponse));
        }


        // PUT: api/Subjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateSubject/{id}")]
        public async Task<IActionResult> PutSubject(int id, [FromForm] SubjectRequest subjectRespone)
        {
            if (!CheckIdExist(id))
            {
                return BadRequest(new BaseResponse(true, "Subject Not Found. Can't Update"));
            }
            var subject = _subjectRepository.GetSubjectById(id);
            _mapper.Map(subjectRespone, subject);

            string updateResult = _subjectRepository.UpdateSubject(subject);
            if (!updateResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "update subject successfull!", subjectRespone));
        }

        // POST: api/Subjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateSubject")]
        public async Task<ActionResult<Subject>> PostSubject([FromForm] SubjectRequest subjectRequest)
        {
            if (_context.Subject == null)
            {
                return Problem("Entity set 'CMSDbContext.Subject'  is null.");
            }
            subjectRequest.is_active = true;
            var subject = _mapper.Map<Subject>(subjectRequest);

            string createResult = _subjectRepository.CreateNewSubject(subject);
            if (!createResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }
            return Ok(new BaseResponse(false, "create new subject successfull!", subjectRequest));
        }

        // DELETE: api/Subjects/5
        [HttpDelete("DeleteSubject/{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            if (_context.Subject == null)
            {
                return NotFound();
            }
            var subject = _subjectRepository.GetSubjectById(id);
            if (subject == null)
            {
                return NotFound(new BaseResponse(true, "Subject you want delete Not Found!"));
            }
            string deleteResult = _subjectRepository.DeleteSubject(subject);
            if (!deleteResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, "Delete successfull!", id));
        }


        // GET: Export Execl Subject
        [HttpGet("ExportSubjectToExecl")]
        public async Task<ActionResult<SubjectExeclResponse>> ExportExeclSubject()
        {
            var subject = _subjectRepository.GetAllSubject();
            var reader = _mapper.Map<List<SubjectExeclResponse>>(subject);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.SaveAs(reader);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Subjects");
            }
        }

        [NonAction]
        public bool CheckIdExist(int id)
        {
            if (_context.Subject.Find(id) == null) return false;
            return true;
        }
    }
}
