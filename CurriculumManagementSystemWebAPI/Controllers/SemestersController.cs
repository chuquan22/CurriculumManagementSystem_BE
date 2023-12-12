using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Authorization;
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
        private IBatchRepository batchRepository;

        public SemestersController(IMapper mapper)
        {
            _mapper = mapper;
            semesterRepository = new SemestersRepository();
            batchRepository = new BatchRepository();
        }
        [Authorize(Roles = "Manager, Dispatcher")]
        [HttpGet("GetAllSemester")]
        public ActionResult GetAllSemester()
        {
            var listSemester = semesterRepository.GetSemesters();
            var listSemesterResponse = _mapper.Map<List<SemesterResponse>>(listSemester);
            return Ok(new BaseResponse(false, "List Semester", listSemesterResponse));
        }
        [Authorize(Roles = "Manager, Dispatcher")]
        [HttpGet("GetAllSemesterByMajorId/{major_id}")]
        public ActionResult GetAllSemesterByMajorId(int major_id)
        {
            var listSemester = semesterRepository.GetAllSemestersByMajorId(major_id);
            var listSemesterResponse = _mapper.Map<List<SemesterResponse>>(listSemester);
            return Ok(new BaseResponse(false, "List Semester", listSemesterResponse));
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationSemester([FromQuery] int? degree_id, int page, int limit, [FromQuery] string? txtSearch)
        {
            var listSemester = semesterRepository.PaginationSemester(degree_id, page, limit, txtSearch);
            if (listSemester.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Semester!"));
            }
            var total = semesterRepository.GetTotalSemester(degree_id, txtSearch);
            var listSemesterResponse = _mapper.Map<List<SemesterResponse>>(listSemester);
            return Ok(new BaseResponse(false, "List Semester", new BaseListResponse(page, limit, total, listSemesterResponse)));
        }
        [Authorize(Roles = "Manager, Dispatcher")]
        [HttpGet("GetSemesterById/{Id}")]
        public ActionResult GetSemester(int Id)
        {
            var semester = semesterRepository.GetSemester(Id);
            if(semester == null)
            {
                return NotFound(new BaseResponse(true, "Not Found Semester!"));
            }
            var semesterResponse = _mapper.Map<SemesterResponse>(semester);
            return Ok(new BaseResponse(false, "Semester", semesterResponse));
        }
        [Authorize(Roles = "Manager, Dispatcher")]
        [HttpGet("GetSemesterByDegreeLevel/{Id}")]
        public ActionResult GetSemesterByDegreeLevel(int Id)
        {
            var semester = semesterRepository.GetSemesterByDegreeLevel(Id);
            if (semester == null)
            {
                return NotFound(new BaseResponse(true, "Not Found Semester Plan By Degree Level!"));
            }
            var semesterResponse = _mapper.Map<List<SemesterResponse>>(semester);
            return Ok(new BaseResponse(false, "Semester", semesterResponse));
        }
        [Authorize(Roles = "Manager, Dispatcher")]
        [HttpGet("GetSemesterBySpeId/{speId}")]
        public ActionResult GetSemesterBy(int speId)
        {
            var semester = semesterRepository.GetSemesterBySpe(speId);
            if (semester == null)
            {
                return NotFound(new BaseResponse(true, "Not Found Semester!"));
            }
            var semesterResponse = _mapper.Map<List<SemesterResponse>>(semester);
            return Ok(new BaseResponse(false, "Semester", semesterResponse));
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("CreateSemester")]
        public ActionResult CreateSemester([FromBody]SemesterRequest semesterRequest)
        {
            var semester = _mapper.Map<Semester>(semesterRequest);

            if(semesterRepository.CheckSemesterDuplicate(0, semester.semester_name, semester.school_year, semester.start_batch_id))
            {
                return BadRequest(new BaseResponse(true, $"Semester {semester.semester_name + "-" + semester.school_year} is Duplicate!"));
            }
            string createResult = semesterRepository.CreateSemester(semester);
            if(!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Success!", semesterRequest));
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("UpdateSemester/{id}")]
        public ActionResult UpdateSemester(int id,[FromBody] SemesterRequest semesterRequest)
        {
            var semester = semesterRepository.GetSemester(id);
            
            if(semester == null)
            {
                return NotFound(new BaseResponse(true, "Not Found Semester!"));
            }

            if (semesterRepository.CheckSemesterDuplicate(id, semester.semester_name, semester.school_year, semester.start_batch_id))
            {
                return BadRequest(new BaseResponse(true, "Semester Duplicate!"));
            }
            _mapper.Map(semesterRequest, semester);

            string updateResult = semesterRepository.UpdateSemester(semester);
            if(!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update Success!", semesterRequest));
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("DeleteSemester/{id}")]
        public ActionResult DeleteSemester(int id)
        {
            var semester = semesterRepository.GetSemester(id);
            if (semester == null)
            {
                return NotFound(new BaseResponse(true, "Not Found Semester!"));
            }
            if (semesterRepository.CheckSemesterExsit(id))
            {
                return NotFound(new BaseResponse(true, "Semester Used. Can't Delete!"));
            }

            string deleteResult = semesterRepository.DeleteSemester(semester);
            if (!deleteResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, "Delete Success!"));
        }
    }
}
