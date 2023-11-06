using AutoMapper;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Roles;
using Repositories.Users;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IRoleRepository _roleRepository;

        public RolesController(IMapper mapper)
        {
            _mapper = mapper;
            _roleRepository = new RoleRepository();
        }

        [HttpGet("GetAllRole")]
        public ActionResult GetAllRole()
        {
            var listRole = _roleRepository.GetAllRole();
            
            return Ok(new BaseResponse(false, "list Role", listRole));

        }
    }
}
