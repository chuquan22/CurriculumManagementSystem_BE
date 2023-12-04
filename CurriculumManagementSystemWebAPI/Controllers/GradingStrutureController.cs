using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.GradingCLOs;
using Repositories.GradingStruture;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
   // [Authorize(Roles = "Manager, Dispatcher")]
    [ApiController]
    public class GradingStrutureController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IGradingStrutureRepository gradingStrutureRepository;
        private IGradingCLOsRepository gradingCLOsRepository;


        public GradingStrutureController(IMapper mapper)
        {
            _mapper = mapper;
            gradingStrutureRepository = new GradingStrutureRepository();
            gradingCLOsRepository = new GradingCLOsRepository();

        }
        [HttpGet("syllabus_id")]
        public ActionResult<List<GradingStrutureResponse>> GetGradingStruture(int syllabus_id)
        {
            try
            {
                List<GradingStruture> rs = gradingStrutureRepository.GetGradingStruture(syllabus_id);
                var response = _mapper.Map<List<GradingStrutureResponse>>(rs);
                return Ok(new BaseResponse(false, "Successfully!", response));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }
        }
        [HttpGet("GetGradingStrutureById/{id}")]
        public ActionResult GetGradingStrutureById(int id)
        {
            GradingStruture rs = new GradingStruture();
            try
            {
                rs = gradingStrutureRepository.GetGradingStrutureById(id);
                var response = _mapper.Map<GradingStrutureResponse>(rs);
                return Ok(new BaseResponse(false, "Successfully!", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpPost]
        public ActionResult CreateGradingStructure(GradingStrutureCreateRequest gra)
        {
            if (gra == null)
            {
                return BadRequest(new BaseResponse(true, "Invalid request. 'gra' is null.", null));
            }

            if (gra.gradingStruture == null || gra.gradingCLORequest == null)
            {
                return BadRequest(new BaseResponse(true,"Invalid request. 'gradingStruture' or 'gradingCLORequest' is null.",null));
            }

            try
            {
                GradingStruture rs = _mapper.Map<GradingStruture>(gra.gradingStruture);
                if (rs.number_of_questions == null)
                {
                    rs.number_of_questions = "";
                }
                rs = gradingStrutureRepository.CreateGradingStruture(rs);

                if (rs != null)
                {
                    foreach (var g in gra.gradingCLORequest.CLO_id)
                    {
                        GradingCLO rs2 = new GradingCLO
                        {
                            CLO_id = g,
                            grading_id = rs.grading_id 
                        };

                        var rs3 = gradingCLOsRepository.CreateGradingCLO(rs2);
                    }
                }

                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpPost("CreateGradingStructureAPI")]
        public ActionResult CreateGradingStructureAPI(GradingStrutureCreateRequest gra)
        {
            if (gra == null)
            {
                return BadRequest(new BaseResponse(true, "Invalid request. 'gra' is null.", null));
            }

            if (gra.gradingStruture == null || gra.gradingCLORequest == null)
            {
                return BadRequest(new BaseResponse(true, "Invalid request. 'gradingStruture' or 'gradingCLORequest' is null.", null));
            }

            try
            {
                GradingStruture rs = _mapper.Map<GradingStruture>(gra.gradingStruture);
                if (rs.number_of_questions == null)
                {
                    rs.number_of_questions = "";
                }
                rs = gradingStrutureRepository.CreateGradingStrutureAPI(rs);
                if(rs == null)
                {
                    return BadRequest(new BaseResponse(true, "Error: False when create grading struture | Please check weight!" , null));

                }
                if (rs != null)
                {
                    foreach (var g in gra.gradingCLORequest.CLO_id)
                    {
                        GradingCLO rs2 = new GradingCLO
                        {
                            CLO_id = g,
                            grading_id = rs.grading_id
                        };

                        var rs3 = gradingCLOsRepository.CreateGradingCLO(rs2);
                    }
                }

                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpPut]
        public ActionResult UpdateStruture(GradingStrutureUpdateRequest gra)
        {
            GradingStruture rs = _mapper.Map<GradingStruture>(gra.gradingStruture);
            try
            {
                string ressult = gradingStrutureRepository.UpdateGradingStruture(rs, gra.gradingCLORequest.CLO_id);
                return Ok(new BaseResponse(false, "Successfully!", ressult));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteStruture(int id)
        {
            GradingStruture rs = new GradingStruture();
            try
            {
                rs = gradingStrutureRepository.DeleteGradingStruture(id);
                return Ok(new BaseResponse(false, "Successfully!", true));
            }
            catch (Exception ex)
            {             
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
    }
}
