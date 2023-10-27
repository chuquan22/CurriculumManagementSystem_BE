using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Specialization;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private IConfiguration config;
        private readonly IMapper _mapper;
        private readonly ISpecializationRepository repo;

        public SpecializationController(IConfiguration configuration, IMapper mapper)
        {
            config = configuration;
            _mapper = mapper;
            repo = new SpecializationRepository();
        }
        
        [HttpGet]
        public ActionResult GetListAllSpecialization()
        {
            List<Specialization> rs = new List<Specialization>();
            try
            {
                rs = repo.GetSpecialization();
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "Get List Major False", null));
        }
        [HttpGet("GetListPagging")]
        public ActionResult GetListSpecialization(int page, int limit, string? txtSearch,string? major_id)
        {
            List<Specialization> rs = new List<Specialization>();
            try
            {
                int limit2 = repo.GetTotalSpecialization(txtSearch, major_id);
                rs = repo.GetListSpecialization(page,limit,txtSearch,major_id);
                var result = _mapper.Map<List<Specialization>>(rs);
                return Ok(new BaseResponse(false, "Sucessfully", new BaseListResponse(page,limit2,rs)));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "Get List Major False", null));
        }
        [HttpGet("{id}")]
        public ActionResult GetSpecialization(int id)
        {
            Specialization rs = new Specialization();
            try
            {
                
                rs = repo.GetSpeById(id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "Get List Major False", null));
        }
        [HttpPost]
        public ActionResult CreateSpecialization(SpecializationRequest spe)
        {
            try
            {
                Specialization rs = _mapper.Map<Specialization>(spe);
                bool checkCodeExist = repo.IsCodeExist(rs.specialization_code);
                if (checkCodeExist != true)
                {
                    rs = repo.CreateSpecialization(rs);
                    return Ok(new BaseResponse(false, "Create specialization successfully.", rs));
                }
                else
                {
                    return BadRequest(new BaseResponse(false, "Specialization code already exist in system.", rs));

                }
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "Get List Major False", null));
        }
        [HttpPut]
        public ActionResult UpdateSpecialization(SpecializationUpdateRequest spe)
        {
            try
            {
                Specialization rs = _mapper.Map<Specialization>(spe);
               // bool checkCodeExist = repo.IsCodeExist(rs.specialization_code);

              
                    rs = repo.UpdateSpecialization(rs);
                    return Ok(new BaseResponse(false, "Sucessfully", rs));

            
                
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(false, "Specialization update false!", null));

            }
            return Ok(new BaseResponse(true, "Get List Major False", null));
        }
        [HttpDelete]
        public ActionResult DeleteSpecialization(int id)
        {
            string rs = null;

            try
            {

                rs = repo.DeleteSpecialization(id);
                if(rs == null)
                {
                    return BadRequest(new BaseResponse(true, "Can't Delete This Specialization!"));
                }
                return Ok(new BaseResponse(false, "Sucessfully", rs));

            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(false, "Delete Specialization False. Specialization is already using in system!", null));

            }
            return Ok(new BaseResponse(true, "Delete specialization false", null));
        }
    }
}
