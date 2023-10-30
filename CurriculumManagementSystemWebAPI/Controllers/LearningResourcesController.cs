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
        private ILearningResourceRepository repo;

        public LearningResourcesController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new LearningResourceRepository();
        }
        [HttpGet]
        public ActionResult GetLearningResource()
        {
            List<LearningResource> rs = new List<LearningResource>();
            try
            {
                rs = repo.GetLearningResource();
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
            var listLearningResource = repo.PaginationLearningResource(page, limit, txtSearch);
            if (listLearningResource.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Learning Resource!"));
            }
            return Ok(new BaseResponse(false, "List Learning Resource", listLearningResource));
        }

        [HttpPost("CreateLearningResource")]
        public ActionResult CreateLearningResource([FromBody] LearningResourceRequest learningResourceRequest)
        {
            if(repo.CheckLearningResourceDuplicate(learningResourceRequest.learning_resource_type))
            {
                return BadRequest(new BaseResponse(true, "Learning Resource is Duplicate!"));
            }

            var learningResource = _mapper.Map<LearningResource>(learningResourceRequest); 

            string createResult = repo.CreateLearningResource(learningResource);
            if(!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Learning Resource Success!", learningResourceRequest));
        }



    }
}
