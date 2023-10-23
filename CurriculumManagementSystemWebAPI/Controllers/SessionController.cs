using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Session;
using Repositories.SessionCLOs;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISessionRepository repo;
        private ISessionCLOsRepository repo2;

        public SessionController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new SessionRepository();
            repo2 = new SessionCLOsRepository();
        }
        [HttpGet]
        public ActionResult GetSession(int syllabus_id)
        {
            List < Session> rs = new List<Session>();
            try
            {
                rs = repo.GetSession(syllabus_id);
                var result = _mapper.Map < List<SessionResponse>>(rs);
                return Ok(new BaseResponse(false, "Sucessfully", result));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpPost]
        public ActionResult CreateSession(SessionCreateRequest request)
        {
            Session rs = new Session();
            try
            {
                var session = _mapper.Map<Session>(request.session);
                rs = repo.CreateSession(session);

                var session_clo = _mapper.Map<List<SessionCLO>>(request.session_clo);
                if(rs != null)
                {
                    foreach (var item in session_clo)
                    {
                        item.session_id = rs.schedule_id;
                        var rs2 = repo2.CreateSessionCLO(item);

                    }

                }
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpPut]
        public ActionResult UpdateSesion(SessionUpdateRequest request)
        {
            try
            {
                Session rs = _mapper.Map<Session>(request.session);

                //   rs = repo.GetSession(syllabus_id);
                string result = repo.UpdateSession(rs, request.session_clo);
                return Ok(new BaseResponse(false, "Sucessfully", result));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpDelete]
        public ActionResult DeleteSession(int id)
        {
 
            try
            {
                string rs = repo.DeleteSession(id);
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
                 rs = repo.GetSessionById(id);
                var result = _mapper.Map<SessionResponse>(rs);

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
