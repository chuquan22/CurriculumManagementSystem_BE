using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess.DTO;
using AutoMapper;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;

        public SubjectsController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        // GET: api/Subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDTO>>> GetSubject()
        {
          if (_context.Subject == null)
          {
              return NotFound();
          }
            var subject = await _context.Subject.Include(x => x.AssessmentMethod).Include(x => x.LearningMethod).ToListAsync();
            var subjectDTO = _mapper.Map<List<SubjectDTO>>(subject);
            return subjectDTO;
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDTO>> GetSubject(int id)
        {
          if (_context.Subject == null)
          {
              return NotFound();
          }
            var subject =  _context.Subject.Include(x => x.AssessmentMethod).Include(x => x.LearningMethod).Where(x => x.subject_id == id).FirstOrDefault();
            var subjectDTO = _mapper.Map<SubjectDTO>(subject);
            if (subject == null)
            {
                return NotFound();
            }

            return subjectDTO;
        }

        // PUT: api/Subjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(int id, Subject subject)
        {
            if (id != subject.subject_id)
            {
                return BadRequest();
            }

            _context.Entry(subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
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

        // POST: api/Subjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subject>> PostSubject(Subject subject)
        {
          if (_context.Subject == null)
          {
              return Problem("Entity set 'CMSDbContext.Subject'  is null.");
          }
            _context.Subject.Add(subject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubject", new { id = subject.subject_id }, subject);
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            if (_context.Subject == null)
            {
                return NotFound();
            }
            var subject = await _context.Subject.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            _context.Subject.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubjectExists(int id)
        {
            return (_context.Subject?.Any(e => e.subject_id == id)).GetValueOrDefault();
        }
    }
}
