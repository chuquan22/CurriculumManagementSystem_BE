using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Specialization;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private IConfiguration config;
        private readonly IMapper _mapper;
        private readonly ISpecializationRepository repo;

        public SpecializationController(IConfiguration configuration, IMapper mapper)
        {
            config = configuration;
            _mapper = mapper;
            repo = new SpecializationRepository();
        }
        
        [HttpGet]
        public ActionResult GetListSpecialization()
        {
            List<Specialization> rs = new List<Specialization>();
            try
            {
                rs = repo.GetSpec();
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "Get List Major False", null));
        }
    }
}
