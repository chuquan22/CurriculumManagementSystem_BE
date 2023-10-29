using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
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

        [HttpPost("CreateClassSessionType")]
        public ActionResult CreateClassSessionType([FromBody] ClassSessionTypeRequest classSessionTypeRequest)
        {
            if(_repo.CheckClassSessionTypeDuplicate(classSessionTypeRequest.class_session_type_name))
            {
                return BadRequest(new BaseResponse(true, "Class Session Type is Duplicate!"));
            }

            var classSessionType = _mapper.Map<ClassSessionType>(classSessionTypeRequest);

            string createResult = _repo.CreateClassSessionType(classSessionType);
            if(!createResult.Equals(Result.createSuccessfull.ToString())) {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", classSessionTypeRequest));
        }
    }
}
