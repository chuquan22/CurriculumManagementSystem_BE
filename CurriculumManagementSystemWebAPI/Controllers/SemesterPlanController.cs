﻿using AutoMapper;
using BusinessObject;
using DataAccess.DAO;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.SemesterPlans;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.Xml;
using static Google.Apis.Requests.BatchRequest;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager, Dispatcher")]
    [ApiController]
    public class SemesterPlanController : ControllerBase
    {
        private ISemesterPlanRepository _repo;
        public SemesterPlanController()
        {
            _repo = new SemesterPlanRepository();

        }
        [HttpGet("CreateSemesterPlan/{semester_id}")]
        public ActionResult CreateSemesterPlan(int semester_id)
        {
            try
            {
                var list = _repo.GetSemesterPlanDetails(semester_id);
                List<CreateSemesterPlanResponse> rs = null;
                if(list.spe.Count == 0)
                {
                    var response = _repo.GetSemesterPlan(semester_id);
                    rs = _repo.GetSemesterPlanOverView(semester_id);
                    return Ok(new BaseResponse(false, "Create new semester plan successfully!", rs));
                }
                else
                {
                    return BadRequest(new BaseResponse(false, "Semester plan is exist!", rs));
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(false, "Error: " + ex.Message, null));

            }
        }
        [HttpGet("GetSemesterPlanOverView/{semester_id}")]
        public ActionResult GetSemesterPlanOverView(int semester_id)
        {
            try
            {
                var response = _repo.GetSemesterPlanOverView(semester_id);
                return Ok(new BaseResponse(false, "Successfully!", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpGet("GetSemesterPlanSubject/{semester_id}")]
        public ActionResult GetSemesterPlanSubject(int semester_id)
        {
            try
            {
                var response = _repo.GetSemesterPlanDetails(semester_id);
                return Ok(new BaseResponse(false, "Successfully!", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpGet("GetSemesterPlanDetails/{semester_id}")]
        public ActionResult GetSemesterPlanDetails(int semester_id)
        {
            try
            {
                var response = _repo.GetSemesterPlanOverViewDetails(semester_id);
                return Ok(new BaseResponse(false, "Successfully!", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpDelete("DeleteSemesterPlan/{semester_id}")]
        public ActionResult DeleteSemesterPlan(int semester_id)
        {
            try
            {
                var response = _repo.DeleteSemesterPlan(semester_id);
                return Ok(new BaseResponse(false, "Successfully!", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
    }
}
