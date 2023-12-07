using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.CLOS;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Manager, Dispatcher")]
    [ApiController]
    public class CLOsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ICLORepository cloRepository;

        public CLOsController(IMapper mapper)
        {
            _mapper = mapper;
            cloRepository = new CLORepository();
        }
        [HttpGet("{syllabus_id}")]
        public ActionResult<List<CLO>> GetCLOs(int syllabus_id)
        {
            List<CLO> rs = new List<CLO>();
            try
            {
                rs = cloRepository.GetCLOs(syllabus_id);
              
                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpPost]
        public ActionResult<CLO> CreateCLOs(CLOsRequest clo)
        {

            try
            {
                CLO rs = _mapper.Map<CLO>(clo);
                rs = cloRepository.CreateCLOs(rs);
                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
    

        [HttpPut]
        public ActionResult UpdateCLOs(CLOsUpdateRequest clo)
        {
           
            try
            {
                var check = cloRepository.GetCLOsById(clo.CLO_id);
                if (check == null)
                {
                    return NotFound(new BaseResponse(true, "Not Found This CLOs!", null));
                }
                CLO rs = _mapper.Map<CLO>(clo);
                rs = cloRepository.UpdateCLOs(rs);
                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCLOs(int id)
        {
            CLO rs = new CLO();
            try
            {
                rs = cloRepository.GetCLOsById(id);
                if(rs == null)
                {
                    return NotFound(new BaseResponse(true,"Not Found This CLOs!", null));
                }
                rs = cloRepository.DeleteCLOs(id);
                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpGet("GetCLOsById/{id}")]
        public ActionResult GetCLOsById(int id)
        {
            CLO rs = new CLO();
            try
            {
                rs = cloRepository.GetCLOsById(id);
                if (rs == null)
                {
                    return NotFound(new BaseResponse(true, "Not Found This CLOs!", null));
                }
                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
    }
}
