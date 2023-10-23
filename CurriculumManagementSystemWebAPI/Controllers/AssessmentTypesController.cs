using AutoMapper;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.AssessmentTypes;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentTypesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IAssessmentTypeRepository _repo;

        public AssessmentTypesController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new AssessmentTypeRepository();
        }

        [HttpGet("GetAllAssessmentType")]
        public ActionResult GetAllAssessmentType()
        {
            var listAssessmentType = _repo.GetAllAssessmentType();
            var listAssessmentTypeResponse = _mapper.Map<List<AssessmentTypeResponse>>(listAssessmentType);
            return Ok(new BaseResponse(false, "Sucessfully", listAssessmentTypeResponse));
        }
    }
}
