using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.SemesterBatchs;
using Repositories.SemesterPlans;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterPlanController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISemesterBatchRepository _repo;
        private ISemesterPlanRepository _repo2;

        public SemesterPlanController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new SemesterBatchRepository();
            _repo2 = new SemesterPlanRepository();
        }

        [HttpGet]
        public ActionResult GetSemesterPlan(int semester_id,string degree_level)
        {
            var list = _repo.GetSemesterBatch(semester_id, degree_level);
            return Ok();
        }

    }
}
