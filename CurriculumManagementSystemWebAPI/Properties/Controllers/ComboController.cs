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
        private IComboRepository comboRepository;

        public ComboController(IMapper mapper)
        {
            _mapper = mapper;
            comboRepository = new ComboRepository();
        }
        [HttpGet("{specialization_id}")]
        public ActionResult GetListCombo(int specialization_id)
        {
            List<ComboResponse> rs = new List<ComboResponse>();
            try
            {
                var result = comboRepository.GetListCombo(specialization_id);
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
                var result = comboRepository.FindComboById(id);
                rs = _mapper.Map<ComboResponse>(result);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpGet("GetListComboByCurri/{curri_Id}")]
        public ActionResult GetListComboByCurriculum(int curri_Id)
        {
            List<Combo> rs = new List<Combo>();
            try
            {
                rs = comboRepository.GetListComboByCurriId(curri_Id);
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
                bool checkCode = comboRepository.IsCodeExist(rs.combo_code);
                if (checkCode == false)
                {
                    rs = comboRepository.CreateCombo(rs);
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
              
                bool rs = comboRepository.DisableCombo(id);
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
                var combo = comboRepository.FindComboById(cb.combo_id);
                _mapper.Map(cb, combo);
                combo = comboRepository.UpdateCombo(combo);
                return Ok(new BaseResponse(false, "Sucessfully", combo));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
        }
        [HttpDelete]
        public ActionResult DeleteCombo(int id)
        {

            string rs = null;
            try
            {
                rs = comboRepository.DeleteCombo(id);
                if(rs != "Delete sucessfully.")
                {
                    return BadRequest(new BaseResponse(true, "Can't Delete Combo Used"));
                }
                return Ok(new BaseResponse(false, "Sucessfully", rs));

            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }
        }
    }
}
