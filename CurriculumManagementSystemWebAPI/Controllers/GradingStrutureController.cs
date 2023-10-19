using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.GradingCLOs;
using Repositories.GradingStruture;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradingStrutureController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IGradingStrutureRepository repo;
        private IGradingCLOsRepository repo2;


        public GradingStrutureController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new GradingStrutureRepository();
            repo2 = new GradingCLOsRepository();

        }
        [HttpGet]
        public ActionResult GetGradingStruture(int syllabus_id)
        {
            try
            {
                List<GradingStruture> rs = repo.GetGradingStruture(syllabus_id);
                var response = _mapper.Map<List<GradingStrutureResponse>>(rs);
                return Ok(new BaseResponse(false, "Sucessfully", response));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpPost]
        public ActionResult CreateGradingStruture(GradingStrutureCreateRequest gra)
        {
            if (gra == null)
            {
                return BadRequest("Invalid request. 'gra' is null.");
            }

            if (gra.gradingStruture == null || gra.gradingCLORequest == null)
            {
                return BadRequest("Invalid request. 'gradingStruture' or 'gradingCLORequest' is null.");
            }

            try
            {
                GradingStruture rs = _mapper.Map<GradingStruture>(gra.gradingStruture);
                rs = repo.CreateGradingStruture(rs);
                if(rs != null)
                {
                    foreach(var g in gra.gradingCLORequest.CLO_id)
                    {
                        GradingCLO rs2 = new GradingCLO();
                        rs2.CLO_id = g;
                        rs2.grading_id = rs.grading_id;
                        var rs3 = repo2.CreateGradingCLO(rs2);
                    }
                    
                }
               

                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }
        [HttpPut]
        public ActionResult UpdateStruture(GradingStruture gra)
        {
            GradingStruture rs = new GradingStruture();
            try
            {
                rs = repo.UpdateGradingStruture(gra);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpDelete]
        public ActionResult DeleteStruture(int id)
        {
            GradingStruture rs = new GradingStruture();
            try
            {
                rs = repo.DeleteGradingStruture(id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
    }
}
