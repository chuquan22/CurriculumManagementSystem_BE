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
using Repositories.Curriculums;
using DataAccess.Models.DTO.response;
using DataAccess.Models.DTO.request;
using System.Diagnostics.CodeAnalysis;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumsController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurriculumRepository _curriculumRepository = new CurriculumRepository();

        public CurriculumsController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET All Curriculum
        [HttpGet("GetAllCurriculum")]
        public async Task<ActionResult<IEnumerable<CurriculumResponse>>> GetCurriculum()
        {
            if (_context.Curriculum == null)
            {
                return NotFound();
            }
            var listCurriculum = _curriculumRepository.GetAllCurriculum();
            if (listCurriculum == null)
            {
                return BadRequest(new BaseResponse(true, "List Curriculum is Empty. Please Add Curriculum!"));
            }
            var listCurriculumRespone = _mapper.Map<List<CurriculumResponse>>(listCurriculum);
            return Ok(new BaseResponse(false, "list Curriculums", listCurriculumRespone));
        }



        [HttpGet("Pagination/{page}/{limit}/{txtSearch}/{specializationId}")]
        public async Task<ActionResult<IEnumerable<SubjectResponse>>> PaginationCurriculum(int page, int limit, string txtSearch, int specializationId)
        {
            if (_context.Subject == null)
            {
                return NotFound();
            }

            var subject = _context.Curriculum.Where(x => x.curriculum_code.Equals(txtSearch) || x.curriculum_name.Equals(txtSearch) && x.specialization_id == specializationId)
                .Skip((page - 1) * limit).Take(limit)
                .Include(x => x.Batch)
                .Include(x => x.Specialization)
                .ToList();

            if (subject == null)
            {
                return BadRequest(new BaseResponse(true, "List Subject is Empty. Please create new subject!"));
            }
            var subjectRespone = _mapper.Map<List<SubjectResponse>>(subject);
            return Ok(new BaseResponse(false, "success",  new BaseListResponse(page, limit, subjectRespone)));
        }

        // Get List Curriculum Have Status Can Remove
        [HttpGet("GetListCurriculumCanDelete")]
        public async Task<ActionResult<IEnumerable<CurriculumResponse>>> Curriculum()
        {
            if (_context.Curriculum == null)
            {
                return NotFound();
            }
            var listCurriculum = _context.Curriculum.Where(x => x.curriculum_status.Equals("Active") == null).ToList();
            var listCurriculumRespone = _mapper.Map<List<CurriculumResponse>>(listCurriculum);
            return listCurriculumRespone;
        }

        // GET: api/Curriculums/5
        [HttpGet("GetCurriculum/{code}")]
        public async Task<ActionResult<CurriculumResponse>> GetCurriculum(string code, int? batchId)
        {
            if (batchId == null)
            {
                batchId = 1;
            }
            var curriculum = _curriculumRepository.GetCurriculum(code, (int)batchId);

            if (curriculum == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found This Curriculum!"));
            }
            var curriculumResponse = _mapper.Map<CurriculumResponse>(curriculum);
            return curriculumResponse;
        }

        // PUT: api/Curriculums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCurriculum/{id}")]
        public async Task<IActionResult> PutCurriculum(int id, [FromForm]CurriculumRequest curriculumRequest)
        {
            if (!CurriculumExists(id))
            {
                return NotFound(new BaseResponse(true, "Can't Update because not found curriculum"));
            }
            var curriculum = _curriculumRepository.GetCurriculumById(id);
            _mapper.Map(curriculumRequest, curriculum);
            string updateResult = _curriculumRepository.UpdateCurriculum(curriculum);
            
            if(!updateResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update Curriculum Success!", curriculumRequest));
        }

        // POST: api/Curriculums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateCurriculum")]
        public async Task<ActionResult<Curriculum>> PostCurriculum([FromForm]CurriculumRequest curriculumRequest)
        {
            if (CheckCurriculumExists(curriculumRequest.curriculum_code, curriculumRequest.batch_id))
            {
                return BadRequest(new BaseResponse(true, "Curriculum Existed. Please Create other curriculum!"));
            }
            var curriculum = _mapper.Map<Curriculum>(curriculumRequest);
            string createResult = _curriculumRepository.CreateCurriculum(curriculum);
            if(!createResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Curriculum Success!", curriculumRequest));
        }

        // DELETE: api/Curriculums/5
        [HttpDelete("DeleteCurriculum/{id}")]
        public async Task<IActionResult> DeleteCurriculum(int id)
        {
            if (_context.Curriculum == null)
            {
                return NotFound();
            }
            var curriculum = _curriculumRepository.GetCurriculumById(id);
            if (curriculum == null)
            {
                return NotFound(new BaseResponse(true, "Not found this curriculum"));
            }
            string deleteResult = _curriculumRepository.RemoveCurriculum(curriculum);
            if (!deleteResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }
            return Ok(new BaseResponse(false, "Delete Curriculum Successfull!"));
        }



        private bool CurriculumExists(int id)
        {
            return (_context.Curriculum?.Any(e => e.curriculum_id == id)).GetValueOrDefault();
        }

        private bool CheckCurriculumExists(string code, int batchId)
        {
            return (_context.Curriculum?.Any(e => e.curriculum_code.Equals(code) && e.batch_id == batchId)).GetValueOrDefault();
        }    
    }
}
