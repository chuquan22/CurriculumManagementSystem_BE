﻿using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.LearningMethods;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningMethodsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ILearnningMethodRepository learningMethodRepository;

        public LearningMethodsController(IMapper mapper)
        {
            _mapper = mapper;
            learningMethodRepository = new LearningMethodRepository();
        }

        [HttpGet("GetAllLearningMethod")]
        public ActionResult GetAllLearningMethod()
        {
            var listLearningMethod = learningMethodRepository.GetAllLearningMethods();
            var listLearningMethodResponse = _mapper.Map<List<LearningMethodDTOResponse>>(listLearningMethod);
            return Ok(new BaseResponse(false, "List Learning Method", listLearningMethodResponse));
        }

        [HttpGet("GetLearningMethodByID/{id}")]
        public ActionResult GetLearningMethod(int id)
        {
            var learningMethod = _repo.GetLearningMethodById(id);
            var learningMethodResponse = _mapper.Map<LearningMethodDTOResponse>(learningMethod);
            return Ok(new BaseResponse(false, "Success!", learningMethodResponse));
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationLearningMethod(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listLearningMethod = _repo.PaginationLearningMethod(page, limit, txtSearch);
            if (listLearningMethod.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Learning Method!"));
            }
            var total = _repo.GetTotalLearningMethod(txtSearch);
            return Ok(new BaseResponse(false, "List Learning Method", new BaseListResponse(page, limit, total, listLearningMethod)));
        }


        [HttpPost("CreateLearningMethod")]
        public ActionResult CreateLearningMethod([FromBody] LearningMethodRequest learningMethodRequest)
        {
            if (_repo.CheckLearningMethodDuplicate(0, learningMethodRequest.learning_method_name))
            {
                return BadRequest(new BaseResponse(true, "Learning Method is Duplicate!"));
            }

            var learningMethod = _mapper.Map<LearningMethod>(learningMethodRequest);

            string createResult = _repo.CreateLearningMethod(learningMethod);
            if (!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Learning Method Success!", learningMethodRequest));
        }

        [HttpPut("UpdateLearningMethod/{id}")]
        public ActionResult UpdateLearningMethod(int id,[FromBody] LearningMethodRequest learningMethodRequest)
        {
            var learningMethod = _repo.GetLearningMethodById(id);
            if(learningMethod == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found Learning Method!"));
            }

            if (_repo.CheckLearningMethodDuplicate(id, learningMethodRequest.learning_method_name))
            {
                return BadRequest(new BaseResponse(true, "Learning Method is Duplicate!"));
            }

             _mapper.Map(learningMethodRequest, learningMethod);

            string updateResult = _repo.UpdateLearningMethod(learningMethod);
            if (!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update Learning Method Success!", learningMethodRequest));
        }

        [HttpDelete("DeleteLearningMethod/{id}")]
        public ActionResult DeleteLearningMethod(int id)
        {
            var learningMethod = _repo.GetLearningMethodById(id);
            if (learningMethod == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found Learning Method!"));
            }

            if (_repo.CheckLearningMethodExsit(id))
            {
                return BadRequest(new BaseResponse(true, $"Learning Method {learningMethod.learning_method_name} used by Subject. Can't Delete!"));
            }

            string deleteResult = _repo.DeleteLearningMethod(learningMethod);
            if (!deleteResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, "Delete Learning Method Success!", learningMethod));
        }
    }
}
