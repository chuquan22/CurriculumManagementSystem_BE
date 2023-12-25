using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Specialization;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager, Dispatcher")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISpecializationRepository specializationRepository;

        public SpecializationController(IMapper mapper)
        {
            _mapper = mapper;
            specializationRepository = new SpecializationRepository();
        }

        [HttpGet]
        public ActionResult<List<SpecializationResponse>> GetListAllSpecialization()
        {
            List<Specialization> rs = new List<Specialization>();
            try
            {
                rs = specializationRepository.GetSpecialization();
                var speResponse = _mapper.Map<List<SpecializationResponse>>(rs);
                return Ok(new BaseResponse(false, "Successfully!", speResponse));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }
        }
        [HttpGet("GetListPagging")]
        public ActionResult<List<SpecializationResponse>> GetListSpecialization(int page, int limit, string? txtSearch, string? major_id)
        {
            List<Specialization> rs = new List<Specialization>();
            try
            {
                int limit2 = specializationRepository.GetTotalSpecialization(txtSearch, major_id);
                rs = specializationRepository.GetListSpecialization(page, limit, txtSearch, major_id);
                var result = _mapper.Map<List<Specialization>>(rs);
                return Ok(new BaseResponse(false, "Successfully!", new BaseListResponse(page, limit2, rs)));

            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }
        }
        [HttpGet("{id}")]
        public ActionResult GetSpecialization(int id)
        {
            Specialization rs = new Specialization();
            try
            {
                rs = specializationRepository.GetSpeById(id);
                if (rs == null)
                {
                    return NotFound(new BaseResponse(true, "Not Found This Specialization!", null));
                }
                return Ok(new BaseResponse(false, "Successfully!!", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }
        }
        [HttpPost]
        public ActionResult CreateSpecialization(SpecializationRequest spe)
        {
            try
            {
                Specialization rs = _mapper.Map<Specialization>(spe);
                bool checkCodeExist = specializationRepository.IsCodeExist(rs.specialization_code);
                if (checkCodeExist != true)
                {
                    rs = specializationRepository.CreateSpecialization(rs);
                    return Ok(new BaseResponse(false, "Successfully!", rs));
                }
                else
                {
                    return BadRequest(new BaseResponse(true, "Specialization code already exists in the system!", spe.specialization_code));

                }
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }
        }
        [HttpPut]
        public ActionResult UpdateSpecialization(SpecializationUpdateRequest spe)
        {
            try
            {
                Specialization rs = _mapper.Map<Specialization>(spe);
                rs = specializationRepository.UpdateSpecialization(rs);
                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }
        }
        [HttpDelete]
        public ActionResult DeleteSpecialization(int id)
        {
            string rs = null;

            try
            {

                rs = specializationRepository.DeleteSpecialization(id);
                if (rs == null)
                {
                    return BadRequest(new BaseResponse(true, "Specialization used in the system, can’t delete specialization"));
                }
                return Ok(new BaseResponse(false, "Successfully!", rs));

            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "Delete Specialization False. Specialization is already using in system!", null));

            }
        }
    }
}