using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Session;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISessionRepository repo;

        public SessionController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new SessionRepository();
        }
        [HttpGet]
        public ActionResult GetSession(int syllabus_id)
        {
            List < Session> rs = new List<Session>();
            try
            {
                rs = repo.GetSession(syllabus_id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpPost]
        public ActionResult CreateSession(int syllabus_id)
        {
            Session rs = new Session();
            try
            {
               // rs = repo.GetSession(syllabus_id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpPut]
        public ActionResult UpdateSesion(int syllabus_id)
        {
            Session rs = new Session();
            try
            {
             //   rs = repo.GetSession(syllabus_id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpDelete]
        public ActionResult DeleteSession(int syllabus_id)
        {
            Session rs = new Session();
            try
            {
               // rs = repo.GetSession(syllabus_id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpGet("{id}")]
        public ActionResult GetSessionById(int id)
        {
            Session rs = new Session();
            try
            {
                // rs = repo.GetSession(syllabus_id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
    } 
}
