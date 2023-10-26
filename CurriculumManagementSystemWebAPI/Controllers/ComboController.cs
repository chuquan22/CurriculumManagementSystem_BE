using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Combos;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IComboRepository repo;

        public ComboController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new ComboRepository();
        }
        [HttpGet]
        public ActionResult GetListCombo(int specialization_id)
        {
            List<ComboResponse> rs = new List<ComboResponse>();
            try
            {
                var result = repo.GetListCombo(specialization_id);
                rs = _mapper.Map<List<ComboResponse>>(result);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpGet("{id}")]
        public ActionResult GetCombo(int id)
        {
            ComboResponse rs = new ComboResponse();
            try
            {
                var result = repo.FindComboById(id);
                rs = _mapper.Map<ComboResponse>(result);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpPost]
        public ActionResult CreateCombo(ComboRequest cb)
        {
            try
            {
                Combo rs = _mapper.Map<Combo>(cb);
                bool checkCode = repo.IsCodeExist(rs.combo_code);
                if (checkCode == false)
                {
                    rs = repo.CreateCombo(rs);
                    return Ok(new BaseResponse(false, "Sucessfully", rs));
                }
                else
                {
                    return BadRequest(new BaseResponse(true, "Combo code already exist!", null));

                }

            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpPost("DisableCombo")]
        public ActionResult DisableCombo(int id)
        {
            try
            {
              
                bool rs = repo.DisableCombo(id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpPut]
        public ActionResult UpdateCombo(ComboUpdateRequest cb)
        {
            try
            {
                Combo rs = _mapper.Map<Combo>(cb);

                rs = repo.UpdateCombo(rs);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpDelete]
        public ActionResult DeleteCombo(int id)
        {

            string rs = null;
            try
            {
                rs = repo.DeleteCombo(id);
                return Ok(new BaseResponse(false, rs, null));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }
    }
}
