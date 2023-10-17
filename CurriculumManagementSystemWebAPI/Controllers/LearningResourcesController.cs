using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
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

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }
    }
}
