using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using AutoMapper;
using Repositories.Curriculums;
using Repositories.CurriculumSubjects;
using CurriculumManagementSystemWebAPI.Models.DTO.response;
using CurriculumManagementSystemWebAPI.Models.DTO.request;
using DataAccess.Models.Enums;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumSubjectsController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurriculumSubjectRepository _curriculumSubjectRepository = new CurriculumSubjectRepository();

        public CurriculumSubjectsController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        // GET: api/CurriculumSubjects
        [HttpGet("GetAllCurriculumSubject")]
        public async Task<ActionResult<IEnumerable<CurriculumSubjectResponse>>> GetCurriculumSubject()
        {
            if (_context.CurriculumSubject == null)
            {
                return NotFound();
            }
            var curriculumSubject = _curriculumSubjectRepository.GetAllCurriculumSubject();

            if(curriculumSubject == null)
            {
                return BadRequest(new BaseResponse(true, "List Curriculum Subject is empty. Please Create Curriculum Subject"));
            }
            var curriculumSubjectResponse = _mapper.Map<List<CurriculumSubjectResponse>>(curriculumSubject);
            return Ok(new BaseResponse(false, "success!", curriculumSubjectResponse));
        }

        // GET: api/CurriculumSubjects/5
        [HttpGet("GetCurriculumBySubject/{subjectId}")]
        public async Task<ActionResult<CurriculumSubjectResponse>> GetCurriculumBySubject(int subjectId)
        {
            if (_context.CurriculumSubject == null)
            {
                return NotFound();
            }
            var curriculumSubject = _curriculumSubjectRepository.GetListCurriculumBySubject(subjectId);

            if (curriculumSubject == null)
            {
                return NotFound();
            }
            var curriculumSubjectResponse = _mapper.Map<List<CurriculumSubjectResponse>>(curriculumSubject);

            return Ok(new BaseResponse(false, "Success!", curriculumSubjectResponse));
        }

        // GET: api/CurriculumSubjects/5
        [HttpGet("GetSubjectByCurriculum/{curriculumId}")]
        public async Task<ActionResult<CurriculumSubjectResponse>> GetSubjectByCurriculum(int curriculumId)
        {
            if (_context.CurriculumSubject == null)
            {
                return NotFound();
            }
            var curriculumSubject = _curriculumSubjectRepository.GetListSubjectByCurriculum(curriculumId);

            if (curriculumSubject == null)
            {
                return NotFound();
            }
            var curriculumSubjectResponse = _mapper.Map<List<CurriculumSubjectResponse>>(curriculumSubject);

            return Ok(new BaseResponse(false, "Success!", curriculumSubjectResponse));
        }

        // PUT: api/CurriculumSubjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCurriculumSubject/{curriculumId}/{subjectId}")]
        public async Task<IActionResult> PutCurriculumSubject(int curriculumId, int subjectId, [FromForm]CurriculumSubjectRequest curriculumSubjectRequest)
        {
            if(!CurriculumSubjectExists(curriculumId, subjectId))
            {
                return NotFound(new BaseResponse(true, "Not found this Curriculum Subject"));
            }
            var curriculumSubject = _curriculumSubjectRepository.GetCurriculumSubjectById(curriculumId, subjectId);
            _mapper.Map(curriculumSubjectRequest, curriculumSubject);
            string updateResult = _curriculumSubjectRepository.UpdateCurriculumSubject(curriculumSubject);
            if(!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }
            return Ok(new BaseResponse(false, "Update success!", curriculumSubjectRequest));
        }

        // POST: api/CurriculumSubjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateCurriculumSubject")]
        public async Task<ActionResult<CurriculumSubject>> PostCurriculumSubject(CurriculumSubject curriculumSubject)
        {
            if (_context.CurriculumSubject == null)
            {
                return Problem("Entity set 'CMSDbContext.CurriculumSubject'  is null.");
            }
            

            return CreatedAtAction("GetCurriculumSubject", new { id = curriculumSubject.curriculum_id }, curriculumSubject);
        }

        // DELETE: api/CurriculumSubjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurriculumSubject(int id)
        {
            if (_context.CurriculumSubject == null)
            {
                return NotFound();
            }
            var curriculumSubject = await _context.CurriculumSubject.FindAsync(id);
            if (curriculumSubject == null)
            {
                return NotFound();
            }

            _context.CurriculumSubject.Remove(curriculumSubject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CurriculumSubjectExists(int curriId, int subId)
        {
            return (_context.CurriculumSubject?.Any(e => e.curriculum_id == curriId && e.subject_id==subId)).GetValueOrDefault();
        }
    }
}
