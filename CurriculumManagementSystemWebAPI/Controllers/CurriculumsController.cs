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
using DataAccess.Models.Enums;
using Repositories.CurriculumSubjects;


namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumsController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurriculumRepository _curriculumRepository = new CurriculumRepository();
        private readonly ICurriculumSubjectRepository _curriculumsubjectRepository = new CurriculumSubjectRepository();
        

        public CurriculumsController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Curriculums/GetCurriculumByBatch/code/5
        [HttpGet("GetCurriculumByBatch/{curriculumCode}/{batchId}")]
        public async Task<ActionResult<IEnumerable<CurriculumResponse>>> GetCurriculumByBatch(string curriculumCode, int batchId)
        {
            if (_context.Curriculum == null)
            {
                return NotFound();
            }
            var listCurriculum = _curriculumRepository.GetCurriculum(curriculumCode, batchId);
            if (listCurriculum == null)
            {
                return Ok(new BaseResponse(true, "Not Found Curriculum"));
            }
            var listCurriculumRespone = _mapper.Map<List<CurriculumResponse>>(listCurriculum);
            return Ok(new BaseResponse(false, "list Curriculums", listCurriculumRespone));
        }

        // GET: api/Curriculums/GetListBatchByCurriculumCode/code
        [HttpGet("GetListBatchByCurriculumCode/{curriculumCode}")]
        public async Task<ActionResult<IEnumerable<Batch>>> GetListBatch(string curriculumCode)
        {
           
            var listBatch = _curriculumRepository.GetBatchByCurriculumCode(curriculumCode);
            if (listBatch.Count == 0)
            {
                return BadRequest(new BaseResponse(true, "Cannot Found Batch By this curriculum"));
            }
            return Ok(new BaseResponse(false, "list Batch", listBatch));
        }


        // GET: api/Curriculums/Pagination/5/6/search/1
        [HttpGet("Pagination/{page}/{limit}")]
        public async Task<ActionResult<IEnumerable<SubjectResponse>>> PaginationCurriculum(int page, int limit,[FromQuery] string? txtSearch, [FromQuery]int? specializationId)
        {
            var listCurriculum = _curriculumRepository.PanigationCurriculum(page, limit, txtSearch, specializationId);
            
            if (listCurriculum.Count == 0)
            {
                return Ok(new BaseResponse(true, "Not Found Subject"));
            }
            var totalElement = _curriculumRepository.GetAllCurriculum(txtSearch, specializationId).Count();

            var subjectRespone = _mapper.Map<List<CurriculumResponse>>(listCurriculum);
            
            foreach (var curriculum in subjectRespone)
            {
               curriculum.total_credit = _curriculumRepository.GetTotalCredit(curriculum.curriculum_id);
            }

            return Ok(new BaseResponse(false,"Get List Curriculum Sucessfully", new BaseListResponse(page, limit, totalElement, subjectRespone)));

        }

        // GET: api/Curriculums/GetListCurriculumCanDelete
        // Get List Curriculum Have Status Can Remove
        [HttpGet("GetListCurriculumCanDelete")]
        public async Task<ActionResult<IEnumerable<CurriculumResponse>>> Curriculum()
        {
            if (_context.Curriculum == null)
            {
                return NotFound();
            }
            var listCurriculum = _context.Curriculum.Where(x => x.is_active == true).ToList();
            var listCurriculumRespone = _mapper.Map<List<CurriculumResponse>>(listCurriculum);
            return listCurriculumRespone;
        }

        // GET: api/Curriculums/GetCurriculum/5
        [HttpGet("GetCurriculum/{id}")]
        public async Task<ActionResult<CurriculumResponse>> GetCurriculum(int id)
        {
            var curriculum = _curriculumRepository.GetCurriculumById(id);

            if (curriculum == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found This Curriculum!"));
            }
            var curriculumResponse = _mapper.Map<CurriculumResponse>(curriculum);
            return Ok(new BaseResponse(false, "Curriculum", curriculumResponse));
        }

        // GET: api/Curriculums/GetListBatchNotInCurriculum/code
        [HttpGet("GetListBatchNotInCurriculum/{curriculumCode}")]
        public async Task<ActionResult<Batch>> GetlistBatch(string curriculumCode)
        {
            var batch = _curriculumRepository.GetListBatchNotExsitInCurriculum(curriculumCode);

            if (batch.Count == 0)
            {
                return BadRequest(new BaseResponse(true, "Not Found Batch Not Exsit in Curriculum!"));
            }
            return Ok(new BaseResponse(false, "Curriculum", batch));
        }


        // PUT: api/Curriculums/UpdateCurriculum/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCurriculum/{id}")]
        public async Task<IActionResult> PutCurriculum(int id, [FromBody]CurriculumUpdateRequest curriculumRequest)
        {
            if (!CurriculumExists(id))
            {
                return NotFound(new BaseResponse(true, "Can't Update because not found curriculum"));
            }
            var curriculum = _curriculumRepository.GetCurriculumById(id);
            _mapper.Map(curriculumRequest, curriculum);
            curriculum.updated_date = DateTime.Today;
            string updateResult = _curriculumRepository.UpdateCurriculum(curriculum);
            
            if(!updateResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update Curriculum Success!", curriculumRequest));
        }

        // POST: api/Curriculums/CreateCurriculum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateCurriculum")]
        public async Task<ActionResult<Curriculum>> PostCurriculum([FromBody]CurriculumRequest curriculumRequest)
        {
            var curriculum = _mapper.Map<Curriculum>(curriculumRequest);

            curriculum.curriculum_code = _curriculumRepository.GetCurriculumCode(curriculum.batch_id, curriculum.specialization_id);
            curriculum.total_semester = 7;
            
            if (CheckCurriculumExists(curriculum.curriculum_code, curriculum.batch_id))
            {
                return BadRequest(new BaseResponse(true, "Curriculum Existed. Please Create other curriculum!"));
            }
            string createResult = _curriculumRepository.CreateCurriculum(curriculum);
            if(!createResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Curriculum Success!", curriculumRequest));
        }

        // DELETE: api/Curriculums/DeleteCurriculum/5
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
