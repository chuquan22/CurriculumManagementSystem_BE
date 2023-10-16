using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Materials;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IMaterialRepository repo;

        public MaterialsController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new MaterialRepository();
        }
        [HttpGet]
        public ActionResult GetMaterial(int syllabus_id)
        {
            Material rs = new Material();
            try
            {
                rs = repo.GetMaterial(syllabus_id);
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
