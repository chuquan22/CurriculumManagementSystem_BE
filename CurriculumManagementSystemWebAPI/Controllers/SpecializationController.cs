using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private IConfiguration config;
        private readonly IMapper _mapper;

        public SpecializationController(IConfiguration configuration, IMapper mapper)
        {
            config = configuration;
            _mapper = mapper;
        }

    }
}
