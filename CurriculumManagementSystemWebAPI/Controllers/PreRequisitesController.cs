﻿using System;
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
using Microsoft.AspNetCore.Authorization;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PreRequisitesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPreRequisiteRepository _preRequisiteRepository = new PreRequisiteRepository();

        public PreRequisitesController(IMapper mapper)
        {
            _mapper = mapper;
        }


        // GET: api/GetPreRequisiteBySubject/{subjectId}
        [HttpGet("GetPreRequisiteBySubject/{subjectId}")]
        public async Task<ActionResult<IEnumerable<PreRequisite>>> GetPreRequisiteBySubject(int subjectId)
        {
           
            var preRequisite = _preRequisiteRepository.GetPreRequisitesBySubject(subjectId);
            if (preRequisite.Count == 0)
            {
                return Ok(new BaseResponse(false, "Subject hasn't Prerequisite Subject!", preRequisite));
            }
            var preRequisiteResponse = _mapper.Map<List<PreRequisiteResponse>>(preRequisite);
            return Ok(new BaseResponse(false, "Get PreRequisite By Subject", preRequisiteResponse));
        }

        // GET: api/GetPreRequisite/{subjectId}/{preSubjectId}
        [HttpGet("GetPreRequisite/{subjectId}/{preSubjectId}")]
        public async Task<ActionResult<PreRequisite>> GetPreRequisite(int subjectId, int preSubjectId)
        {
            
            var preRequisite = _preRequisiteRepository.GetPreRequisite(subjectId, preSubjectId);

            if (preRequisite == null)
            {
                return NotFound(new BaseResponse(true, "Cannot found Prerequisite"));
            }

            return Ok(new BaseResponse(false, "success", preRequisite));
        }


        


    }
}
