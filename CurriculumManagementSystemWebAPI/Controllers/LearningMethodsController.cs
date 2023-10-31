using AutoMapper;
using DataAccess.Models.DTO.response;
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
        private ILearnningMethodRepository _repo;

        public LearningMethodsController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new LearningMethodRepository();
        }

        [HttpGet("GetAllLearningMethod")]
        public ActionResult GetAllLearningMethod()
        {
            var listLearningMethod = _repo.GetAllLearningMethods();
            var listLearningMethodResponse = _mapper.Map<List<LearningMethodDTOResponse>>(listLearningMethod);
            return Ok(new BaseResponse(false, "List Batch", listLearningMethodResponse));
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationLearningMethod(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listLearningMethod = _repo.PaginationLearningMethod(page, limit, txtSearch);
            if (listLearningMethod.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Learning Method!"));
            }
            return Ok(new BaseResponse(false, "List Learning Method", listLearningMethod));
        }
    }
}
