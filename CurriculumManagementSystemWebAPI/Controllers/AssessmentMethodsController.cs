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
            return Ok(new BaseResponse(false, "List Assessment Method", listAssessmentMethodResponse));
        }


        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationAssessmentMethod(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listAssessmentMethod = _repo.PaginationAssessmentMethod(page, limit, txtSearch);
            if (listAssessmentMethod.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Assessment Method!"));
            }
            var total = _repo.GetTotalAssessmentMethod(txtSearch);
            var listAssessmentMethodResponse = _mapper.Map<List<AssessmentMethodDTOResponse>>(listAssessmentMethod);
            return Ok(new BaseResponse(false, "List Assessment Method", new BaseListResponse(page, limit, total, listAssessmentMethodResponse)));
        }

        [HttpPost("CreateAssessmentMethod")]
        public ActionResult CreateAssessmentMethod([FromBody] AssessmentMethodRequest assessmentMethodRequest)
        {
            if (_repo.CheckAssmentMethodDuplicate(assessmentMethodRequest.assessment_method_component))
            {
                return BadRequest(new BaseResponse(true, "Assessment Method Duplicate!"));
            }

            var assessmentMethod = _mapper.Map<AssessmentMethod>(assessmentMethodRequest);

            string createResult = _repo.CreateAssessmentMethod(assessmentMethod);

            if (!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", assessmentMethodRequest));
        }

        [HttpPut("updateAssessmentMethod/{id}")]
        public ActionResult UpdateAssessmentMethod(int id,[FromBody] AssessmentMethodRequest assessmentMethodRequest)
        {
            var assessmentMethod = _repo.GetAsssentMethodById(id);

            if (assessmentMethod == null)
            {
                return NotFound(new BaseResponse(true, "Can't Found Assessment Method"));
            }

            _mapper.Map(assessmentMethodRequest, assessmentMethod);

            string updateResult = _repo.UpdateAssessmentMethod(assessmentMethod);

            if (!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update Success!", assessmentMethodRequest));
        }


        [HttpDelete("DeleteAssessmentMethod/{id}")]
        public ActionResult DeleteAssessmentMethod(int id)
        {
            var assessmentMethod = _repo.GetAsssentMethodById(id);

            if (assessmentMethod == null)
            {
                return NotFound(new BaseResponse(true, "Can't Found Assessment Method"));
            }

            if (_repo.CheckAssmentMethodExsit(id))
            {
                return BadRequest(new BaseResponse(true, "Assessment Method is Used! Can't Delete"));
            }

            string createResult = _repo.DeleteAssessmentMethod(assessmentMethod);

            if (!createResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            var assessMethodRespone = _mapper.Map<AssessmentMethodDTOResponse>(assessmentMethod);

            return Ok(new BaseResponse(false, "Delete Success!", assessMethodRespone));
        }
    }
}
