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
            return Ok(new BaseResponse(false, "List Batch", listLearningMethodResponse));
        }
    }
}
