using AutoMapper;
using BusinessObject;
using DataAccess.DAO;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.SemesterPlans;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.Xml;
using static Google.Apis.Requests.BatchRequest;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterPlanController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISemesterPlanRepository _repo;
        private CMSDbContext _context = new CMSDbContext();
        public SemesterPlanController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new SemesterPlanRepository();

        }
        [HttpGet("CreateSemesterPlan/{semester_id}")]
        public ActionResult CreateSemesterPlan(int semester_id)
        {
            try
            {
                var list = _repo.GetSemesterPlanDetails(semester_id);
                List<CreateSemesterPlanResponse> rs = null;
                if(list.spe.Count == 0)
                {
                    var response = _repo.GetSemesterPlan(semester_id);
                    rs = _repo.GetSemesterPlanOverView(semester_id);

                }
                return Ok(new BaseResponse(false, "Create new semester plan successfully!", rs));

            }
            catch (Exception ex)
            {

                return Ok(new BaseResponse(false, "Error: " + ex.Message, null));

            }
        }
        [HttpGet("GetSemesterPlanOverView/{semester_id}")]
        public ActionResult GetSemesterPlanOverView(int semester_id)
        {
            try
            {
                var response = _repo.GetSemesterPlanOverView(semester_id);
                return Ok(new BaseResponse(false, "Successfully!", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpGet("GetSemesterPlanSubject/{semester_id}")]
        public ActionResult GetSemesterPlanSubject(int semester_id)
        {
            try
            {
                var response = _repo.GetSemesterPlanDetails(semester_id);
                return Ok(new BaseResponse(false, "Successfully!", response));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpGet("GetSemesterPlanDetails/{semester_id}")]
        public ActionResult GetSemesterPlanDetails(int semester_id)
        {
            var semester_root = _context.Semester.Include(x => x.Batch).Where(x => x.semester_id == semester_id).FirstOrDefault();
            int degreeLv = semester_root.degree_level_id;
            var listSemesterPlan = _context.SemesterPlan
               .Include(x => x.Curriculum)
               .Include(x => x.Curriculum.Specialization)
               .Include(x => x.Semester)
               .Where(s => s.semester_id == semester_id)
               .ToList();

            var responseList = new List<SemesterPlanResponse>();

            var uniqueCurriculumIds = listSemesterPlan.Select(sp => sp.Curriculum.specialization_id).Distinct().ToList();

            foreach (var curriculumId in uniqueCurriculumIds)
            {
                var semesterPlanForCurriculum = listSemesterPlan.FirstOrDefault(sp => sp.Curriculum.specialization_id == curriculumId);

                if (semesterPlanForCurriculum != null)
                {
                    var spe = semesterPlanForCurriculum.Curriculum.Specialization.specialization_english_name;
                    var totalSemester = semesterPlanForCurriculum.Curriculum.total_semester;
                    var semester = semesterPlanForCurriculum.Semester.semester_name + " " + semesterPlanForCurriculum.Semester.school_year;

                    int order = semester_root.Batch.batch_order - semesterPlanForCurriculum.Curriculum.total_semester;
                    var validSemester = _context.Semester.Include(x => x.Batch).Where(x => x.Batch.batch_order > order && x.Batch.batch_order <= semester_root.Batch.batch_order).Where(x => x.degree_level_id == degreeLv).OrderByDescending(x => x.Batch.batch_order).ToList();
                    List<SemesterBatchResponse> batchResponses = new List<SemesterBatchResponse>();
                    int i = 1;
                    foreach (var item in validSemester)
                    {
                        SemesterBatchResponse batch = new SemesterBatchResponse();
                        batch.semester_batch_id = item.semester_id;
                        batch.semester_id = item.semester_id;
                        batch.batch_id = item.Batch.batch_id;
                        batch.batch_name = "K"+ item.Batch.batch_name;
                        batch.term_no = i;
                        batch.degree_level = 1;
                        i++;
                        batchResponses.Add(batch);
                    }
                    i = 1;
                    var semesterPlanResponse = new SemesterPlanResponse
                    {
                        spe = spe,
                        totalSemester = totalSemester,
                        semester = semester,
                        batch = batchResponses
                    };

                    responseList.Add(semesterPlanResponse);
                }
            }
            return Ok(new BaseResponse(false, "123!", responseList));
        }

    }
}
