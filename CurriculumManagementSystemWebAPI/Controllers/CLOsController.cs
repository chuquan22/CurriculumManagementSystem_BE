using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.CLOS;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CLOsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ICLORepository repo;

        public CLOsController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new CLORepository();
        }
        [HttpGet]
        public ActionResult GetCLOs(int syllabus_id)
        {
            CLO rs = new CLO();
            try
            {
                rs = repo.GetCLOsById(syllabus_id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpPost]
        public ActionResult CreateCLOs(CLO clo)
        {
            CLO rs = new CLO();
            try
            {
                rs = repo.CreateCLOs(clo);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpPut]
        public ActionResult UpdateCLOs(CLO clo)
        {
            CLO rs = new CLO();
            try
            {
                rs = repo.UpdateCLOs(clo);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpDelete]
        public ActionResult DeleteCLOs(int id)
        {
            CLO rs = new CLO();
            try
            {
                rs = repo.DeleteCLOs(id);
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
