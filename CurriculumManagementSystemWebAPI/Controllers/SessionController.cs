using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.ClassSessionTypes;
using Repositories.Session;
using Repositories.SessionCLOs;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager, Dispatcher")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISessionRepository sessionRepository;
        private ISessionCLOsRepository sessionCLOsRepository;
        private IClassSessionTypeRepository classSessionTypeRepository;

        public SessionController(IMapper mapper)
        {
            _mapper = mapper;
            sessionRepository = new SessionRepository();
            sessionCLOsRepository = new SessionCLOsRepository();
            classSessionTypeRepository = new ClassSessionTypeRepository();
        }
        [HttpGet("{syllabus_id}")]
        public ActionResult GetSession(int syllabus_id)
        {
            List<Session> rs = new List<Session>();
            try
            {
                rs = sessionRepository.GetSession(syllabus_id);
                List<SessionResponse> result = _mapper.Map<List<SessionResponse>>(rs);
                foreach (SessionResponse rs2 in result)
                {
                    var class_session_type = classSessionTypeRepository.GetClassSessionType(rs2.class_session_type_id);
                    rs2.class_session_type_name = class_session_type.class_session_type_name;
                }
                return Ok(new BaseResponse(false, "Successfully!", result));

            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }
        }
        [HttpPost]
        public ActionResult CreateSession(SessionCreateRequest request)
        {
            Session rs = new Session();
            try
            {
                var session = _mapper.Map<Session>(request.session);
                var check = sessionRepository.IsSessionNoExist(session.session_No, session.schedule_id);
                if (check != null)
                {
                    return BadRequest(new BaseResponse(false, "Session No already used in system.", rs));

                }
                rs = sessionRepository.CreateSession(session);

                var session_clo = _mapper.Map<List<SessionCLO>>(request.session_clo);
                if (rs != null)
                {
                    foreach (var item in session_clo)
                    {
                        item.session_id = rs.schedule_id;
                        var rs2 = sessionCLOsRepository.CreateSessionCLO(item);

                    }
                }
                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, ex.Message, null));
            }

        }
        [HttpPut]
        public ActionResult UpdateSesion(SessionUpdateRequest request)
        {
            try
            {
                Session rs = _mapper.Map<Session>(request.session);

                //   rs = repo.GetSession(syllabus_id);
                string result = sessionRepository.UpdateSession(rs, request.session_clo);
                return Ok(new BaseResponse(false, result, null));
            }

            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, ex.Message, null));
            }
        }
        [HttpPatch]

        public ActionResult UpdatePatchSesion(List<SessionPatchRequest> request)
        {
            try
            {
                foreach (var item in request)
                {
                    if(item.remote_learning < 0 || item.ass_defense < 0 || item.eos_exam < 0 ||  item.video_learning < 0 || item.IVQ < 0 || item.online_lab < 0 || item.online_lab < 0 || item.assigment < 0)
                    {
                        return BadRequest(new BaseResponse(true, "Value of time allocation musts > 0.", null));
                    }
                    Session rs = _mapper.Map<Session>(item);

                    //   rs = repo.GetSession(syllabus_id);
                    string result = sessionRepository.UpdatePatchSession(rs);
                }



                return Ok(new BaseResponse(false, "Successfully!", null));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }

        }
        [HttpDelete("{id}")]
        public ActionResult DeleteSession(int id)
        {

            try
            {
                string rs = sessionRepository.DeleteSession(id);
                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }
        }
        [HttpGet("GetSessionById/{id}")]
        public ActionResult GetSessionById(int id)
        {
            Session rs = new Session();
            try
            {
                rs = sessionRepository.GetSessionById(id);
                var result = _mapper.Map<SessionResponse>(rs);

                return Ok(new BaseResponse(false, "Successfully!", result));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }
        }
    }
}
