using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                if (page <= 0)
                {
                    page = 1;
                }
                //Code phan trang
                int numPs, numperPage, numpage, start, end;
                numPs = repo.GetTotalSyllabus(txtSearch);
                numperPage = limit;
                numpage = numPs / numperPage + (numPs % numperPage == 0 ? 0 : 1);
                start = ((page - 1) * numperPage) + 1;
                if (page * numperPage > numPs)
                {
                    end = numPs;
                }
                else
                {
                    end = page * numperPage;
                }
                //Code phan trang
                List<Syllabus> result = repo.GetListSyllabus(start, end, txtSearch);
                var xx = _mapper.Map<List<SyllabusResponse>>(result);
                return Ok(new BaseResponse(false, "Sucess", xx));
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

            return Ok(new BaseResponse(true, "False", null));
        }

    }
}
