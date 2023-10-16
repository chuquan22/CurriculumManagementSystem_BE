﻿using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Session;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISessionRepository repo;

        public SessionController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new SessionRepository();
        }
        [HttpGet]
        public ActionResult GetMaterial(int syllabus_id)
        {
            Session rs = new Session();
            try
            {
                rs = repo.GetSession(syllabus_id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
    } 
}