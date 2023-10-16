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
        private MaterialRepository repo2;
        private CLORepository repo3;
        private SessionRepository repo4;
        private GradingStrutureRepository repo5;


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
                if (page <= 0)
                {
                    page = 1;
                }
                //Code phan trang
                int numPs, numperPage, numpage, start, end;
                numPs = repo.GetTotalSyllabus(txtSearch);
                numperPage = limit;
                numpage = numPs / numperPage + (numPs % numperPage == 0 ? 0 : 1);
                start = ((page - 1) * numperPage);
                if (page * numperPage > numPs)
                {
                    end = numPs;
                }
                else
                {
                    end = page * numperPage;
                }
                List<Syllabus> list = repo.GetListSyllabus(start, end, txtSearch);
                var result = _mapper.Map<List<SyllabusResponse>>(list);
                return Ok(new BaseResponse(false, "Sucess", new BaseListResponse(page,numPs, result)));
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
 
            return Ok(new BaseResponse(true, "False", rs1));
        }

    }
}
