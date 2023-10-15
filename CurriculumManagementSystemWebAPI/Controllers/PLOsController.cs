using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using AutoMapper;
using Repositories.CurriculumCombo;
using Repositories.PLOS;
using DataAccess.Models.DTO.response;
using DataAccess.Models.DTO.request;
using DataAccess.Models.Enums;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PLOsController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPLOsRepository _plosRepository = new PLOsRepository();

        public PLOsController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/PLOs/GetListPLOs/5
        [HttpGet("GetListPLOs/{curriculumId}")]
        public async Task<ActionResult<IEnumerable<PLOs>>> GetListPLOs(int curriculumId)
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
        public async Task<ActionResult<PLOs>> PostPLOs(PLOs pLOs)
        {
          if (_context.PLOs == null)
          {
              return Problem("Entity set 'CMSDbContext.PLOs'  is null.");
          }
            _context.PLOs.Add(pLOs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPLOs", new { id = pLOs.PLO_id }, pLOs);
        }

        // DELETE: api/PLOs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePLOs(int id)
        {
            if (_context.PLOs == null)
            {
                return NotFound();
            }
            var pLOs = await _context.PLOs.FindAsync(id);
            if (pLOs == null)
            {
                return NotFound();
            }

            _context.PLOs.Remove(pLOs);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PLOsExists(int id)
        {
            return (_context.PLOs?.Any(e => e.PLO_id == id)).GetValueOrDefault();
        }
    }
}
