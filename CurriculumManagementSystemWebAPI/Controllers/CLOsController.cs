using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
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
            List<CLO> rs = new List<CLO>();
            try
            {
                rs = repo.GetCLOs(syllabus_id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpPost]
        public ActionResult CreateCLOs(CLOsRequest clo)
        {
          
            try
            {
                CLO rs =  _mapper.Map<CLO>(clo);
                rs = repo.CreateCLOs(rs);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpPut]
        public ActionResult UpdateCLOs(CLOsUpdateRequest clo)
        {
           
            try
            {
                CLO rs = _mapper.Map<CLO>(clo);
                rs = repo.UpdateCLOs(rs);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
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

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpGet("{id}")]
        public ActionResult GetCLOsById(int id)
        {
            CLO rs = new CLO();
            try
            {
                rs = repo.GetCLOsById(id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }
    }
}
