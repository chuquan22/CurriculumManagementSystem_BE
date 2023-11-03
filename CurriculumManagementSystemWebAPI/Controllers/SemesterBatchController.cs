using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Curriculums;
using Repositories.SemesterBatchs;
using Repositories.SemesterPlans;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterBatchController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISemesterBatchRepository semesterBatchRepository;
        private ICurriculumRepository curriculumRepositoryu;
        private ISemesterPlanRepository semesterPlanRepository;
        public SemesterBatchController(IMapper mapper)
        {
            _mapper = mapper;
            semesterBatchRepository = new SemesterBatchRepository();
            curriculumRepositoryu = new CurriculumRepository();
            semesterPlanRepository = new SemesterPlanRepository();
        }
        [HttpGet("{semester_id}/{degree_level}")]
        public ActionResult GetSemesterBatch(int semester_id, string degree_level)
        {
            var list = semesterBatchRepository.GetSemesterBatch(semester_id, degree_level);
            var rs = _mapper.Map<List<SemesterBatchResponse>>(list);
            return Ok(new BaseResponse(false,"Get List ",rs));
        }
        [HttpPost]
        public ActionResult CreateSemesterBatch(int semester_id, string degree_level)
        {
            var rs2 = semesterBatchRepository.CreateSemesterBatch(new SemesterBatch() { semester_id = semester_id, degree_level = degree_level });
            var rs = _mapper.Map<List<SemesterBatchResponse>>(rs2);
            return Ok(rs);
        }

        //[HttpPut]
        //public ActionResult UpdateSemesterBatch(List<SemesterBatchRequest> list)
        //{
        //   // List<Curriculum> curriculumList = curriculumRepositoryu.GetCurriculumByDegreeLevel(list[0].degree_level);
        //    if( curriculumList == null || curriculumList.Count == 0)
        //    {
        //        return BadRequest(new BaseResponse(false, "No curriculum equal that degree level", null));
        //    }
        //    //Create SemesterPlan
        //    SemesterPlan sp = new SemesterPlan();
        //    foreach(Curriculum curriculum in curriculumList)
        //    {
        //        sp.curriculum_id = curriculum.curriculum_id;
        //        sp.semester_id = list[0].semester_id;
        //        //sp.degree_level = list[0].degree_level;
        //        semesterPlanRepository.CreateSemesterPlan(sp);
        //    }

        //    //Update Semester Batch
        //    string result = null;
        //    foreach (var item in list)
        //    {
        //        var semester = _mapper.Map<SemesterBatch>(item);
        //        result = semesterBatchRepository.UpdateSemesterBatch(semester);
        //    }

        //    return Ok(new BaseResponse(false, result, null));
        //}
    }
}
