using AutoMapper;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.PLOMappings;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PLOMappingsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IPLOMappingRepository _repo;

        public PLOMappingsController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new PLOMappingRepository();
        }

        [HttpGet("GetAllPLOMapping/{curriculumId}")]
        public ActionResult GetAllPLOMapping(int curriculumId)
        {
            var listPLOMapping = _repo.GetPLOMappingsInCurriculum(curriculumId);
            var listPLOMappingResponse = _mapper.Map<PLOMappingDTOResponse>(listPLOMapping);
            return Ok(new BaseResponse(false, "List PLO Mapping", listPLOMappingResponse));
        }

        [HttpPost("CreatePLOMapping")]
        public ActionResult CreatePLOMapping(int curriculumId)
        {
            var listPLOMapping = _repo.GetPLOMappingsInCurriculum(curriculumId);
            var listPLOMappingResponse = _mapper.Map<PLOMappingDTOResponse>(listPLOMapping);
            return Ok(new BaseResponse(false, "List PLO Mapping", listPLOMappingResponse));
        }
    }
}
