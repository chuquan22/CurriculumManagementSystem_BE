using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.CLOS;
using Repositories.GradingStruture;
using Repositories.Materials;
using Repositories.Session;
using Repositories.Syllabus;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISyllabusRepository repo;


        public SyllabusController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new SyllabusRepository();
        }
        [HttpGet]
        public ActionResult GetListSyllabus(int page,int limit, string? txtSearch, string? subjectCode)
        {
            List<Syllabus> rs = new List<Syllabus>();
            try
            {             
                int limit2 = repo.GetTotalSyllabus(txtSearch, subjectCode);
                List<Syllabus> list = repo.GetListSyllabus(page, limit, txtSearch, subjectCode);
                var result = _mapper.Map<List<SyllabusResponse>>(list);
                return Ok(new BaseResponse(false, "Sucess", new BaseListResponse(page,limit2, result)));
            }
            catch (Exception)
            {
                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpGet("GetSyllabusDetails")]
        public ActionResult SyllabusDetails(int syllabus_id)
        {
            try
            {
                Syllabus rs1 = repo.GetSyllabusById(syllabus_id);
                var result = _mapper.Map<SyllabusDetailsResponse>(rs1);
                List<PreRequisite> pre = repo.GetPre(rs1.subject_id);
               
                result.pre_required = _mapper.Map<List<PreRequisiteResponse2>>(pre);
                return Ok(new BaseResponse(true, "False", result));

            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }

        }

        [HttpPatch]
        public ActionResult UpdatePatchSyllabus(SyllabusPatchRequest request)
        {
            try
            {
                Syllabus rs = _mapper.Map<Syllabus>(request);

                //   rs = repo.GetSession(syllabus_id);
                string result = repo.UpdatePatchSyllabus(rs);
                return Ok(new BaseResponse(false, "Sucessfully", result));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }

    }
}
