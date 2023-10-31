using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.AssessmentMethods;
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

        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationAssessmentType(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listAssessmentType = assessmentTyoeRepository.PaginationAssessmentType(page, limit, txtSearch);
            if (listAssessmentType.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Assessment Method!"));
            }
            var total = assessmentTyoeRepository.GetTotalAssessmentType(txtSearch);
            var listAssessmentTypeResponse = _mapper.Map<List<AssessmentTypeResponse>>(listAssessmentType);
            return Ok(new BaseResponse(false, "List Assessment Method", new BaseListResponse(page, limit, total, listAssessmentTypeResponse)));
        }

        [HttpGet("GetAssessmentTypeById/{id}")]
        public ActionResult GetAssessmentType(int id)
        {
            var assessmentType = assessmentTyoeRepository.GetAsssentTypeById(id);
            var assessmentTypeResponse = _mapper.Map<AssessmentTypeResponse>(assessmentType);
            return Ok(new BaseResponse(false, "Sucessfully", assessmentTypeResponse));
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

        [HttpPut("UpdateAssessmentType/{id}")]
        public ActionResult UpdateAssessmentType(int id, [FromBody] AssessmentTypeRequest assessmentTypeRequest)
        {
            var assessmentType = assessmentTyoeRepository.GetAsssentTypeById(id);
            if (assessmentType == null)
            {
                return NotFound(new BaseResponse(true, "Not Found Assessment Type!"));
            }

            if (assessmentTyoeRepository.CheckAssmentTypeDuplicate(assessmentTypeRequest.assessment_type_name))
            {
                return BadRequest(new BaseResponse(true, "Assessment Type is duplicate!"));
            }

            _mapper.Map(assessmentTypeRequest, assessmentType);

            string updateResult = assessmentTyoeRepository.UpdateAssessmentType(assessmentType);
            if (!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", assessmentType));
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

            return Ok(new BaseResponse(false, "Delete Success!", assessmentType));
        }
    }
}
