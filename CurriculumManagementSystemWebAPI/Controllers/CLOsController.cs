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
        public ActionResult GetMaterial(int syllabus_id)
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
    }
}
