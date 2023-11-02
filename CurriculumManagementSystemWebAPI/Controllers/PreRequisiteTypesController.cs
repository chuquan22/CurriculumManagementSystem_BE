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
using Repositories.PreRequisiteTypes;
using DataAccess.Models.DTO.response;
using DataAccess.Models.DTO.request;
using DataAccess.Models.Enums;
using Repositories.LearningMethods;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreRequisiteTypesController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPreRequisiteTypeRepository _preRequisiteType = new PreRequisiteRepository();

        public PreRequisiteTypesController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/PreRequisiteTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreRequisiteTypeResponse>>> GetPreRequisiteType()
        {
            if (_context.PreRequisiteType == null)
            {
                return NotFound();
            }
            var preRequisite = _preRequisiteType.GetAllPreRequisiteTypes();
            if (preRequisite == null)
            {
                return NotFound(new BaseResponse(true, "Pre-Requisite-Type is Null. Please Create Pre-Requisite-Type"));
            }
            var preRequisiteType = _mapper.Map<List<PreRequisiteTypeResponse>>(preRequisite);
            return Ok(new BaseResponse(false, "List Pre-Requisite-Type", preRequisiteType));
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationPreRequisiteType(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listPreRequisiteType = _preRequisiteType.PaginationPreRequisiteType(page, limit, txtSearch);
            if (listPreRequisiteType.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Learning Method!"));
            }
            var total = _preRequisiteType.GetTotalPreRequisite(txtSearch);
            return Ok(new BaseResponse(false, "List Learning Method", new BaseListResponse(page, limit, total, listPreRequisiteType)));
        }

        // GET: api/PreRequisiteTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PreRequisiteTypeResponse>> GetPreRequisiteType(int id)
        {
            if (_context.PreRequisiteType == null)
            {
                return NotFound();
            }
            var preRequisiteType = _preRequisiteType.GetPreRequisiteType(id);

            if (preRequisiteType == null)
            {
                return NotFound();
            }
            var preRequisiteTypes = _mapper.Map<PreRequisiteTypeResponse>(preRequisiteType);
            return Ok(new BaseResponse(false, "Pre-Requisite-Type", preRequisiteTypes)); ;
        }

        // PUT: api/PreRequisiteTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreRequisiteType(int id, [FromBody] PreRequisiteTypeRequest preRequisiteTypeRequest)
        {
            if (!PreRequisiteTypeExists(id))
            {
                return NotFound(new BaseResponse(true, "Pre-Requisite-Type Not Found"));
            }
            var preRequisiteType = _preRequisiteType.GetPreRequisiteType(id);
            _mapper.Map(preRequisiteTypeRequest, preRequisiteType);

            string updateResult = _preRequisiteType.UpdatePreRequisiteType(preRequisiteType);

            if(updateResult != Result.updateSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }
            return Ok(new BaseResponse(false, "Update Successfull!", preRequisiteType));
        }

        // POST: api/PreRequisiteTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PreRequisiteType>> PostPreRequisiteType([FromBody]PreRequisiteTypeRequest preRequisiteTypeRequest)
        {
            
            var preRequisiteType = _mapper.Map<PreRequisiteType>(preRequisiteTypeRequest);

            string createResult = _preRequisiteType.CreatePreRequisiteType(preRequisiteType);

            if(createResult != Result.createSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Successfull", preRequisiteType));
        }

        // DELETE: api/PreRequisiteTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreRequisiteType(int id)
        {
            var preRequisiteType = _preRequisiteType.GetPreRequisiteType(id);
            if (preRequisiteType == null)
            {
                return NotFound(new BaseResponse(true, "Not Found PreRequisite Type"));
            }

            if (_preRequisiteType.CheckPreRequisiteTypeExsit(id))
            {
                return BadRequest(new BaseResponse(true, "PreRequisite Type Used by PreRequisite. Can't Delete!"));
            }

            string deleteResult = _preRequisiteType.DeletePreRequisiteType(preRequisiteType);
            
            if(deleteResult != Result.createSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, "Delete Successfull", preRequisiteType));
        }

        private bool PreRequisiteTypeExists(int id)
        {
            return (_context.PreRequisiteType?.Any(e => e.pre_requisite_type_id == id)).GetValueOrDefault();
        }
    }
}
