using AutoMapper;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.AssessmentMethods;
using Repositories.Batchs;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentMethodsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IAssessmentMethodRepository _repo;

        public AssessmentMethodsController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new AssessmentMethodRepository();
        }

        [HttpGet("GetAllAssessmentMethod")]
        public ActionResult GetAllAssessmentMethod()
        {
            var listAssessmentMethod = _repo.GetAllAssessmentMethod();
            var listAssessmentMethodResponse = _mapper.Map<List<AssessmentMethodDTOResponse>>(listAssessmentMethod);
            return Ok(new BaseResponse(false, "List Batch", listAssessmentMethodResponse));
        }
    }
}
