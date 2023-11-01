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
        private ISemesterPlanRepository _repo;
        private ISemesterBatchRepository _repo2;

        public SemesterPlanController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new SemesterPlanRepository();
            _repo2 = new SemesterBatchRepository();

        }

        [HttpGet("{semester_id}/{degree_level}")]
        public ActionResult GetSemesterPlan (int semester_id,string degree_level)
        {
            var list = _repo.GetSemesterPlan(semester_id, degree_level);
            var rs = _mapper.Map<List<SemesterPlanResponse>>(list);
            return Ok(new BaseResponse(false, "Get List", rs));
        }
        [HttpGet("GetSemesterPlanDetails/{semester_id}/{degree_level}")]
        public ActionResult GetSemesterPlanDetails(int semester_id, string degree_level)
        {
            var list = _repo.GetSemesterPlanDetails(semester_id, degree_level);
            var rs = _mapper.Map<List<SemesterPlanDetailsResponse>>(list);
            return Ok(new BaseResponse(false, "Get List", rs));
        }
    }
}
