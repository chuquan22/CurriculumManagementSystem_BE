using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using AutoMapper;
using Repositories.PLOS;
using DataAccess.Models.DTO.response;
using DataAccess.Models.DTO.request;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Authorize(Roles = "Manager, Dispatcher")]
    [Route("api/[controller]")]
    [ApiController]
    public class PLOsController : ControllerBase
    {
        private readonly CMSDbContext _context = new CMSDbContext();
        private readonly IMapper _mapper;
        private readonly IPLOsRepository _plosRepository = new PLOsRepository();

        public PLOsController( IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/PLOs/GetListPLOs/5
        [HttpGet("GetListPLOs/{curriculumId}")]
        public async Task<ActionResult<IEnumerable<PLOs>>> GetListPLOsByCurriculum(int curriculumId)
        {
            var plos = _plosRepository.GetListPLOsByCurriculum(curriculumId);
            if (plos == null)
            {
                return NotFound();
            }
            var listPlosResponse = _mapper.Map<List<PLOsDTOResponse>>(plos);
            return Ok(new BaseResponse(false, "List PLO", listPlosResponse));
        }

        // GET: api/PLOs/GetPLOsById/5
        [HttpGet("GetPLOsById/{id}")]
        public async Task<ActionResult<PLOs>> GetPLOs(int id)
        {
            var pLOs = _plosRepository.GetPLOsById(id);

            if (pLOs == null)
            {
                return NotFound(new BaseResponse(true, "Cannot found PLO"));
            }
            var PlosResponse = _mapper.Map<PLOsDTOResponse>(pLOs);
            return Ok(new BaseResponse(false, "List PLO", PlosResponse));
        }

        // PUT: api/PLOs/UpdatePLOs/5
        [HttpPut("UpdatePLOs/{id}")]
        public async Task<IActionResult> PutPLOs(int id, [FromBody]PLOsDTORequest pLOsDTORequest)
        {
            if (_plosRepository.CheckPLONameUpdateDuplicate(id, pLOsDTORequest.PLO_name, pLOsDTORequest.curriculum_id))
            {
                return BadRequest(new BaseResponse(true, $"{pLOsDTORequest.PLO_name} is Duplicate!"));
            }
            var plos = _plosRepository.GetPLOsById(id);
            if (plos == null)
            {
                return NotFound(new BaseResponse(true, "Cannot found PLOs"));
            }
            _mapper.Map(pLOsDTORequest, plos);
            
            string result = _plosRepository.UpdatePLOs(plos);
            if(!result.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, result));
            }

            return Ok(new BaseResponse(false, "Update success", pLOsDTORequest));
        }

        // POST: api/PLOs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostPLOs([FromBody] PLOsDTORequest pLOsDTORequest)
        {
            if (PLOsExists(pLOsDTORequest.PLO_name, pLOsDTORequest.curriculum_id))
            {
                return BadRequest(new BaseResponse(true, $"{pLOsDTORequest.PLO_name} is Duplicate!"));
            }
            var PLOs = _mapper.Map<PLOs>(pLOsDTORequest);
            
            string createResult = _plosRepository.CreatePLOs(PLOs);
            if (!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", pLOsDTORequest));
        }

        // DELETE: api/PLOs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePLOs(int id)
        {
           
            var pLOs = _plosRepository.GetPLOsById(id);
            if (pLOs == null)
            {
                return NotFound();
            }

            string deleteResult = _plosRepository.DeletePLOs(pLOs);
            if (!deleteResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, $"Delete PLO {pLOs.PLO_name} Success!"));
        }

        private bool PLOsExists(string plosName, int curriId)
        {
            return (_context.PLOs?.Any(e => e.PLO_name.Equals(plosName) && e.curriculum_id == curriId)).GetValueOrDefault();
        }
    }
}
