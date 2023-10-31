using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.LearningResources;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningResourcesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ILearningResourceRepository learningResourceRepository;

        public LearningResourcesController(IMapper mapper)
        {
            _mapper = mapper;
            learningResourceRepository = new LearningResourceRepository();
        }
        [HttpGet]
        public ActionResult GetLearningResource()
        {
            List<LearningResource> rs = new List<LearningResource>();
            try
            {
                rs = learningResourceRepository.GetLearningResource();
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error"));
            }
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationLearningResource(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listLearningResource = learningResourceRepository.PaginationLearningResource(page, limit, txtSearch);
            if (listLearningResource.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Learning Resource!"));
            }
            var total = learningResourceRepository.GetTotalLearningResource(txtSearch);
            return Ok(new BaseResponse(false, "List Learning Resource", new BaseListResponse(page, limit, total, listLearningResource)));
        }

        [HttpPost("CreateLearningResource")]
        public ActionResult CreateLearningResource([FromBody] LearningResourceRequest learningResourceRequest)
        {
            if(learningResourceRepository.CheckLearningResourceDuplicate(0, learningResourceRequest.learning_resource_type))
            {
                return BadRequest(new BaseResponse(true, "Learning Resource is Duplicate!"));
            }

            var learningResource = _mapper.Map<LearningResource>(learningResourceRequest); 

            string createResult = learningResourceRepository.CreateLearningResource(learningResource);
            if(!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Learning Resource Success!", learningResourceRequest));
        }

        [HttpPut("UpdateLearningResource/{id}")]
        public ActionResult UpdateLearningResource(int id,[FromBody] LearningResourceRequest learningResourceRequest)
        {
            var learningResource = learningResourceRepository.GetLearningResource(id);
            if(learningResource == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found Learning Resource!"));
            }

            if (learningResourceRepository.CheckLearningResourceDuplicate(id, learningResourceRequest.learning_resource_type))
            {
                return BadRequest(new BaseResponse(true, "Learning Resource is Duplicate!"));
            }

            _mapper.Map(learningResourceRequest, learningResource);

            string updateResult = learningResourceRepository.UpdateLearningResource(learningResource);
            if (!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update Learning Resource Success!", learningResourceRequest));
        }

        [HttpDelete("RemoveLearningResource/{id}")]
        public ActionResult RemoveLearningResource(int id)
        {
            var learningResource = learningResourceRepository.GetLearningResource(id);
            if (learningResource == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found Learning Resource!"));
            }

            if (learningResourceRepository.CheckLearningResourceExsit(id))
            {
                return BadRequest(new BaseResponse(true, "Learning Resource Used by Material. Can't Delete!"));
            }

            string deleteResult = learningResourceRepository.DeleteLearningResource(learningResource);
            if (!deleteResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, $"Remove Learning Resource {learningResource.learning_resource_type} Success!", learningResource));
        }
    }
}
