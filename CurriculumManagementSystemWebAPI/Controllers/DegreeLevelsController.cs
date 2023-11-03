﻿using AutoMapper;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DegreeLevels;
using Repositories.Roles;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DegreeLevelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IDegreeLevelRepository _degreeLevelRepository;

        public DegreeLevelsController(IMapper mapper)
        {
            _mapper = mapper;
            _degreeLevelRepository = new DegreeLevelRepository();
        }

        [HttpGet("GetAllDegreeLevel")]
        public ActionResult GetAllDegreeLevel()
        {
            var listDegreeLevel = _degreeLevelRepository.GetAllDegreeLevel();

            return Ok(new BaseResponse(false, "list Degree Level", listDegreeLevel));

        }
    }
}
