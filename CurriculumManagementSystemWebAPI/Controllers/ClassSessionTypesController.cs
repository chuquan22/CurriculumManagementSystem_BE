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
        private IClassSessionTypeRepository classSessionTypeRepository;

        public ClassSessionTypesController(IMapper mapper)
        {
            _mapper = mapper;
            classSessionTypeRepository = new ClassSessionTypeRepository();
        }

        [HttpGet]
        public ActionResult GetListClassSessionType()
        {
            var listClassSessionType = classSessionTypeRepository.GetListClassSessionType();
            var listAssessmentTypeResponse = _mapper.Map<List<ClassSessionTypeResponse>>(listClassSessionType);
            return Ok(new BaseResponse(false, "Sucessfully", listAssessmentTypeResponse));
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationClassSessionType(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listClassSessionType = classSessionTypeRepository.PaginationClassSessionType(page, limit, txtSearch);
            if (listClassSessionType.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Class Session Type!"));
            }
            var total = _repo.GetTotalClassSessionType(txtSearch);
            var listAssessmentTypeResponse = _mapper.Map<List<ClassSessionTypeResponse>>(listClassSessionType);
            return Ok(new BaseResponse(false, "List Class Session Type", new BaseListResponse(page, limit, total, listAssessmentTypeResponse)));
        }


        [HttpPost("CreateClassSessionType")]
        public ActionResult CreateClassSessionType([FromBody] ClassSessionTypeRequest classSessionTypeRequest)
        {
            if(classSessionTypeRepository.CheckClassSessionTypeDuplicate(classSessionTypeRequest.class_session_type_name))
            {
                return BadRequest(new BaseResponse(true, "Class Session Type is Duplicate!"));
            }

            var classSessionType = _mapper.Map<ClassSessionType>(classSessionTypeRequest);

            string createResult = classSessionTypeRepository.CreateClassSessionType(classSessionType);
            if(!createResult.Equals(Result.createSuccessfull.ToString())) {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", classSessionTypeRequest));
        }

        [HttpPut("UpdateClassSessionType/{id}")]
        public ActionResult UpdateClassSessionType(int id,[FromBody] ClassSessionTypeRequest classSessionTypeRequest)
        {
            var classSessionType = classSessionTypeRepository.GetClassSessionType(id);
            if(classSessionType == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found Class Session Type !"));
            }

            if (classSessionTypeRepository.CheckClassSessionTypeDuplicate(classSessionTypeRequest.class_session_type_name))
            {
                return BadRequest(new BaseResponse(true, "Class Session Type is Duplicate!"));
            }

            _mapper.Map(classSessionTypeRequest, classSessionType);

            string updateResult = classSessionTypeRepository.UpdateClassSessionType(classSessionType);
            if (!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update Success!", classSessionTypeRequest));
        }

        [HttpDelete("DeleteClassSessionType/{id}")]
        public ActionResult DeleteClassSessionType(int id)
        {
            var classSessionType = classSessionTypeRepository.GetClassSessionType(id);
            if (classSessionType == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found Class Session Type !"));
            }

            string deleteResult = classSessionTypeRepository.DeleteClassSessionType(classSessionType);
            if (!deleteResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, "Remove Success!", classSessionType));
        }
    }
}
