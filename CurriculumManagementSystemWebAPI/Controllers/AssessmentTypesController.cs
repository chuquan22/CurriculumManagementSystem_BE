using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
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
        private IAssessmentTypeRepository assessmentTyoeRepository;

        public AssessmentTypesController(IMapper mapper)
        {
            _mapper = mapper;
            assessmentTyoeRepository = new AssessmentTypeRepository();
        }

        [HttpGet("GetAllAssessmentType")]
        public ActionResult GetAllAssessmentType()
        {
            var listAssessmentType = assessmentTyoeRepository.GetAllAssessmentType();
            var listAssessmentTypeResponse = _mapper.Map<List<AssessmentTypeResponse>>(listAssessmentType);
            return Ok(new BaseResponse(false, "Sucessfully", listAssessmentTypeResponse));
        }

        [HttpPost("CreateAssessmentType")]
        public ActionResult CreateAssessmentType([FromBody] AssessmentTypeRequest assessmentTypeRequest)
        {
            if (assessmentTyoeRepository.CheckAssmentTypeDuplicate(assessmentTypeRequest.assessment_type_name))
            {
                return BadRequest(new BaseResponse(true, "Assessment Type is duplicate!"));
            }

            var assessmentType = _mapper.Map<AssessmentType>(assessmentTypeRequest);

            string createResult = assessmentTyoeRepository.CreateAssessmentType(assessmentType);
            if(!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", assessmentTypeRequest));
        }

        [HttpDelete("DeleteAssessmentType/{id}")]
        public ActionResult DeleteAssessmentType(int id)
        {
            var assessmentType = assessmentTyoeRepository.GetAsssentTypeById(id);
            if(assessmentType == null)
            {
                return NotFound(new BaseResponse(true, "Not Found Assessment Type!"));
            }

            if (assessmentTyoeRepository.CheckAssmentTypeExsit(id))
            {
                return BadRequest(new BaseResponse(true, "Assessment Type is Used by Assessment Method. Can't Delete!"));
            }

            string deleteResult = assessmentTyoeRepository.DeleteAssessmentType(assessmentType);
            if (!deleteResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", assessmentType));
        }
    }
}
