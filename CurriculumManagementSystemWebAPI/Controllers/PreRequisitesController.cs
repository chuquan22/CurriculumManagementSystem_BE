using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Repositories.PreRequisites;
using DataAccess.Models.DTO.response;
using AutoMapper;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreRequisitesController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPreRequisiteRepository _preRequisiteRepository = new PreRequisiteRepository();

        public PreRequisitesController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/PreRequisites
        [HttpGet("GetPreRequisiteBySubject/{subjectId}")]
        public async Task<ActionResult<IEnumerable<PreRequisite>>> GetPreRequisiteBySubject(int subjectId)
        {
            if (_context.PreRequisite == null)
            {
                return NotFound();
            }
            var preRequisite = _preRequisiteRepository.GetPreRequisitesBySubject(subjectId);
            if (preRequisite.Count == 0)
            {
                return NotFound(new BaseResponse(true, "Subject hasn't Prerequisite Subject!"));
            }
            var preRequisiteResponse = _mapper.Map<List<PreRequisiteTypeResponse>>(preRequisite);
            return Ok(new BaseResponse(false, "Get PreRequisite By Subject", preRequisiteResponse));
        }

        // GET: api/PreRequisites/5
        [HttpGet("GetPreRequisite/{subjectId}/{preSubjectId}")]
        public async Task<ActionResult<PreRequisite>> GetPreRequisite(int subjectId, int preSubjectId)
        {
            if (_context.PreRequisite == null)
            {
                return NotFound();
            }
            var preRequisite = _preRequisiteRepository.GetPreRequisite(subjectId, preSubjectId);

            if (preRequisite == null)
            {
                return NotFound(new BaseResponse(true, "Cannot found Prerequisite"));
            }

            return Ok(new BaseResponse(false, "success", preRequisite));
        }

        // PUT: api/PreRequisites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreRequisite(int id, PreRequisite preRequisite)
        {
            if (id != preRequisite.subject_id)
            {
                return BadRequest();
            }

            _context.Entry(preRequisite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreRequisiteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PreRequisites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PreRequisite>> PostPreRequisite(PreRequisite preRequisite)
        {
            if (_context.PreRequisite == null)
            {
                return Problem("Entity set 'CMSDbContext.PreRequisite'  is null.");
            }
            _context.PreRequisite.Add(preRequisite);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PreRequisiteExists(preRequisite.subject_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPreRequisite", new { id = preRequisite.subject_id }, preRequisite);
        }

        // DELETE: api/PreRequisites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreRequisite(int id)
        {
            if (_context.PreRequisite == null)
            {
                return NotFound();
            }
            var preRequisite = await _context.PreRequisite.FindAsync(id);
            if (preRequisite == null)
            {
                return NotFound();
            }

            _context.PreRequisite.Remove(preRequisite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PreRequisiteExists(int id)
        {
            return (_context.PreRequisite?.Any(e => e.subject_id == id)).GetValueOrDefault();
        }
    }
}
