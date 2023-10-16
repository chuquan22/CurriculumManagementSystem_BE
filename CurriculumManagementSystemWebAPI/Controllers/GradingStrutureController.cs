using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.GradingStruture;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradingStrutureController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IGradingStrutureRepository repo;

        public GradingStrutureController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new GradingStrutureRepository();
        }
        [HttpGet]
        public ActionResult GetGradingStruture(int syllabus_id)
        {
            GradingStruture rs = new GradingStruture();
            try
            {
                rs = repo.GetGradingStruture(syllabus_id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpPost]
        public ActionResult CreateGradingStruture(GradingStruture gra)
        {
            GradingStruture rs = new GradingStruture();
            try
            {
                rs = repo.CreateGradingStruture(gra);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
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
