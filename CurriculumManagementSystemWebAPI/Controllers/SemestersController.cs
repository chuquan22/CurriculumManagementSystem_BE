using AutoMapper;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.Semesters;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemestersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISemestersRepository _repo;

        public SemestersController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new SemestersRepository();
        }

        [HttpGet("GetAllSemester")]
        public ActionResult GetAllBatch()
        {
            var listSemester = _repo.GetSemesters();
            return Ok(new BaseResponse(false, "List Semester", listSemester));
        }
    }
}
