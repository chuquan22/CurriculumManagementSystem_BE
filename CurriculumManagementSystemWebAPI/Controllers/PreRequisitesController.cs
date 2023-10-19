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
using DataAccess.Models.DTO.request;

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


        // GET: api/GetPreRequisiteBySubject/{subjectId}
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
            var preRequisiteResponse = _mapper.Map<List<PreRequisiteResponse>>(preRequisite);
            return Ok(new BaseResponse(false, "Get PreRequisite By Subject", preRequisiteResponse));
        }

        // GET: api/GetPreRequisite/{subjectId}/{preSubjectId}
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


        


    }
}
