using AutoMapper;
using DataAccess.Models.DTO.response;
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
        private IClassSessionTypeRepository _repo;

        public ClassSessionTypesController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new ClassSessionTypeRepository();
        }

        [HttpGet]
        public ActionResult GetListClassSessionType()
        {
            var listClassSessionType = _repo.GetListClassSessionType();
            var listAssessmentTypeResponse = _mapper.Map<List<ClassSessionTypeResponse>>(listClassSessionType);
            return Ok(new BaseResponse(false, "Sucessfully", listAssessmentTypeResponse));
        }
    }
}
