﻿using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.ClassSessionTypes;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSessionTypesController : ControllerBase
    {

        private readonly IMapper _mapper;
        private IClassSessionTypeRepository classSessionTypeRepository;

        public ClassSessionTypesController(IMapper mapper)
        {
            _mapper = mapper;
            classSessionTypeRepository = new ClassSessionTypeRepository();
        }

        [Authorize(Roles = "Dispatcher, Manager")]
        [HttpGet]
        public ActionResult GetListClassSessionType()
        {
            var listClassSessionType = classSessionTypeRepository.GetListClassSessionType();
            var listClassTypeResponse = _mapper.Map<List<ClassSessionTypeResponse>>(listClassSessionType);
            return Ok(new BaseResponse(false, "Sucessfully", listClassTypeResponse));
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("{id}")]
        public ActionResult GetClassSessionTypeById(int id)
        {
            var classSessionType = classSessionTypeRepository.GetClassSessionType(id);
            if (classSessionType == null)
            {
                return NotFound(new BaseResponse(true, "Not Found Class Session Type!"));
            }
            var classSessionTypeResponse = _mapper.Map<ClassSessionTypeResponse>(classSessionType);
            return Ok(new BaseResponse(false, "Sucessfully", classSessionTypeResponse));
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationClassSessionType(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listClassSessionType = classSessionTypeRepository.PaginationClassSessionType(page, limit, txtSearch);
            if (listClassSessionType.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Class Session Type!"));
            }
            var total = classSessionTypeRepository.GetTotalClassSessionType(txtSearch);
            var listAssessmentTypeResponse = _mapper.Map<List<ClassSessionTypeResponse>>(listClassSessionType);
            return Ok(new BaseResponse(false, "List Class Session Type", new BaseListResponse(page, limit, total, listAssessmentTypeResponse)));
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("CreateClassSessionType")]
        public ActionResult CreateClassSessionType([FromBody] ClassSessionTypeRequest classSessionTypeRequest)
        {
            if(classSessionTypeRepository.CheckClassSessionTypeDuplicate(0, classSessionTypeRequest.class_session_type_name))
            {
                return BadRequest(new BaseResponse(true, "Class Session Type is Duplicate!"));
            }

            var classSessionType = _mapper.Map<ClassSessionType>(classSessionTypeRequest);

            string createResult = classSessionTypeRepository.CreateClassSessionType(classSessionType);
            if(!createResult.Equals(Result.createSuccessfull.ToString())) {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", classSessionTypeRequest));
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("UpdateClassSessionType/{id}")]
        public ActionResult UpdateClassSessionType(int id,[FromBody] ClassSessionTypeRequest classSessionTypeRequest)
        {
            var classSessionType = classSessionTypeRepository.GetClassSessionType(id);
            if(classSessionType == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found Class Session Type !"));
            }

            if (classSessionTypeRepository.CheckClassSessionTypeDuplicate(id, classSessionTypeRequest.class_session_type_name))
            {
                return BadRequest(new BaseResponse(true, "Class Session Type is Duplicate!"));
            }

            _mapper.Map(classSessionTypeRequest, classSessionType);

            string updateResult = classSessionTypeRepository.UpdateClassSessionType(classSessionType);
            if (!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update Success!", classSessionTypeRequest));
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("DeleteClassSessionType/{id}")]
        public ActionResult DeleteClassSessionType(int id)
        {
            var classSessionType = classSessionTypeRepository.GetClassSessionType(id);
            if (classSessionType == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found Class Session Type !"));
            }

            if (classSessionTypeRepository.CheckClassSessionTypeExsit(id))
            {
                return BadRequest(new BaseResponse(true, "Class Session Type Used by Session. Can't Delete!"));
            }

            string deleteResult = classSessionTypeRepository.DeleteClassSessionType(classSessionType);
            if (!deleteResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, "Remove Success!", classSessionType));
        }
    }
}
