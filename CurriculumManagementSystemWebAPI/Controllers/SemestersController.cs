using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
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
        private ISemestersRepository semesterRepository;

        public SemestersController(IMapper mapper)
        {
            _mapper = mapper;
            semesterRepository = new SemestersRepository();
        }

        [HttpGet("GetAllSemester")]
        public ActionResult GetAllSemester()
        {
            var listSemester = semesterRepository.GetSemesters();
            return Ok(new BaseResponse(false, "List Semester", listSemester));
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationSemester(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listSemester = semesterRepository.PaginationSemester(page, limit, txtSearch);
            if (listSemester.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Semester!"));
            }
            var total = semesterRepository.GetTotalSemester(txtSearch);
            return Ok(new BaseResponse(false, "List Semester", new BaseListResponse(page, limit, total, listSemester)));
        }

        [HttpGet("GetSemesterById/{Id}")]
        public ActionResult GetSemester(int Id)
        {
            var semester = semesterRepository.GetSemester(Id);
            if(semester == null)
            {
                return NotFound(new BaseResponse(true, "Not Found Semester!"));
            }
            return Ok(new BaseResponse(false, "Semester", semester));
        }

        [HttpPost("CreateSemester")]
        public ActionResult CreateSemester([FromBody]SemesterRequest semesterRequest)
        {
            var semester = _mapper.Map<Semester>(semesterRequest); 

            if(semesterRepository.CheckSemesterDuplicate(0, semester.semester_name, semester.school_year))
            {
                return BadRequest(new BaseResponse(true, "Semester Duplicate!"));
            }

            string createResult = semesterRepository.CreateSemester(semester);
            if(!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", semesterRequest));
        }

        [HttpPost("UpdateSemester/{id}")]
        public ActionResult UpdateSemester(int id,[FromBody] SemesterRequest semesterRequest)
        {
            var semester = semesterRepository.GetSemester(id);
            if(semester == null)
            {
                return NotFound(new BaseResponse(true, "Not Found Semester!"));
            }

            if (semesterRepository.CheckSemesterDuplicate(id, semester.semester_name, semester.school_year))
            {
                return BadRequest(new BaseResponse(true, "Semester Duplicate!"));
            }

            _mapper.Map(semesterRequest, semester);

            string updateResult = semesterRepository.UpdateSemester(semester);
            if(!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", semesterRequest));
        }


    }
}
