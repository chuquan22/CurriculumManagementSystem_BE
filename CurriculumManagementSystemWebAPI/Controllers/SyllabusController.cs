using AutoMapper;
using BusinessObject;
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
        public ActionResult GetListSyllabus(int page,int limit, string? txtSearch)
        {
            List<Syllabus> rs = new List<Syllabus>();
            try
            {             
                int limit2 = repo.GetTotalSyllabus(txtSearch);
                List<Syllabus> list = repo.GetListSyllabus(page, limit, txtSearch);
                var result = _mapper.Map<List<SyllabusResponse>>(list);
                return Ok(new BaseResponse(false, "Sucess", new BaseListResponse(page,limit2, result)));
            }
            catch (Exception)
            {
                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpPost]
        public ActionResult SyllabusDetails(int syllabus_id)
        {
            Syllabus rs1 = repo.GetSyllabusById(syllabus_id);
            var result = _mapper.Map<SyllabusDetailsResponse>(rs1);
            List<PreRequisite> pre = repo.GetPre(rs1.subject_id);
            List<PreRequisiteResponse2> preRes = new List<PreRequisiteResponse2>();
            foreach (PreRequisite item in pre)
            {
                PreRequisiteResponse2 p = new PreRequisiteResponse2();
                p.prequisite_subject_name = item.Subject.subject_name;
                p.prequisite_name = item.PreRequisiteType.pre_requisite_type_name;
                preRes.Add(p);
            }
            result.pre_required = preRes;
            return Ok(new BaseResponse(true, "False", result));
        }

    }
}
