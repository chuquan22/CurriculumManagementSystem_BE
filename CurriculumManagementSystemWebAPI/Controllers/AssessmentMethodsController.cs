using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
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
        private IAssessmentMethodRepository assessmentMethodRepository;

        public AssessmentMethodsController(IMapper mapper)
        {
            _mapper = mapper;
            assessmentMethodRepository = new AssessmentMethodRepository();
        }

        [HttpGet("GetAllAssessmentMethod")]
        public ActionResult GetAllAssessmentMethod()
        {
            var listAssessmentMethod = assessmentMethodRepository.GetAllAssessmentMethod();
            var listAssessmentMethodResponse = _mapper.Map<List<AssessmentMethodDTOResponse>>(listAssessmentMethod);
            return Ok(new BaseResponse(false, "List Assessment Method", listAssessmentMethodResponse));
        }

        [HttpGet("GetAssessmentMethodById/{id}")]
        public ActionResult GetAssessmentMethod(int id)
        {
            var assessmentMethod = assessmentMethodRepository.GetAsssentMethodById(id);
            var assessmentMethodResponse = _mapper.Map<AssessmentMethodDTOResponse>(assessmentMethod);
            return Ok(new BaseResponse(false, "Assessment Method", assessmentMethodResponse));
        }


        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationAssessmentMethod(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listAssessmentMethod = assessmentMethodRepository.PaginationAssessmentMethod(page, limit, txtSearch);
            if (listAssessmentMethod.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Assessment Method!"));
            }
            var total = assessmentMethodRepository.GetTotalAssessmentMethod(txtSearch);
            var listAssessmentMethodResponse = _mapper.Map<List<AssessmentMethodDTOResponse>>(listAssessmentMethod);
            return Ok(new BaseResponse(false, "List Assessment Method", new BaseListResponse(page, limit, total, listAssessmentMethodResponse)));
        }

        [HttpPost("CreateAssessmentMethod")]
        public ActionResult CreateAssessmentMethod([FromBody] AssessmentMethodRequest assessmentMethodRequest)
        {
            if (assessmentMethodRepository.CheckAssmentMethodDuplicate(0,assessmentMethodRequest.assessment_method_component))
            {
                return BadRequest(new BaseResponse(true, "Assessment Method Duplicate!"));
            }

            var assessmentMethod = _mapper.Map<AssessmentMethod>(assessmentMethodRequest);

            string createResult = assessmentMethodRepository.CreateAssessmentMethod(assessmentMethod);

            if (!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", assessmentMethodRequest));
        }

        [HttpPut("updateAssessmentMethod/{id}")]
        public ActionResult UpdateAssessmentMethod(int id,[FromBody] AssessmentMethodRequest assessmentMethodRequest)
        {
            var assessmentMethod = assessmentMethodRepository.GetAsssentMethodById(id);

            if (assessmentMethod == null)
            {
                return NotFound(new BaseResponse(true, "Can't Found Assessment Method"));
            }


            if (assessmentMethodRepository.CheckAssmentMethodDuplicate(id, assessmentMethodRequest.assessment_method_component))
            {
                return BadRequest(new BaseResponse(true, "Assessment Method Duplicate!"));
            }


            _mapper.Map(assessmentMethodRequest, assessmentMethod);

            string updateResult = assessmentMethodRepository.UpdateAssessmentMethod(assessmentMethod);

            if (!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update Success!", assessmentMethodRequest));
        }


        [HttpDelete("DeleteAssessmentMethod/{id}")]
        public ActionResult DeleteAssessmentMethod(int id)
        {
            var assessmentMethod = assessmentMethodRepository.GetAsssentMethodById(id);

            if (assessmentMethod == null)
            {
                return NotFound(new BaseResponse(true, "Can't Found Assessment Method"));
            }


            if (assessmentMethodRepository.CheckAssmentMethodExsit(id))
            {
                return BadRequest(new BaseResponse(true, "Assessment Method is Used! Can't Delete"));
            }


            string deleteResult = assessmentMethodRepository.DeleteAssessmentMethod(assessmentMethod);

            if (!deleteResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            var assessMethodRespone = _mapper.Map<AssessmentMethodDTOResponse>(assessmentMethod);

            return Ok(new BaseResponse(false, "Delete Success!", assessMethodRespone));
        }
    }
}
