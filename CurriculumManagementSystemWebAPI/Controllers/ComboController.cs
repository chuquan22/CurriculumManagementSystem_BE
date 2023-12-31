﻿using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Combos;
using Repositories.Specialization;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager, Dispatcher")]

    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IComboRepository comboRepository;
        private ISpecializationRepository speRepository;

        public ComboController(IMapper mapper)
        {
            _mapper = mapper;
            comboRepository = new ComboRepository();
            speRepository = new SpecializationRepository();
        }
        [HttpGet("{specialization_id}")]
        public ActionResult GetListCombo(int specialization_id)
        {
            ComboSpeResponse rs = new ComboSpeResponse();
            try
            {
                rs.specialization_name = speRepository.GetSpeById(specialization_id).specialization_name;
                rs.specializataion_english_name = speRepository.GetSpeById(specialization_id).specialization_english_name;
                var result = comboRepository.GetListCombo(specialization_id);
                rs.combo_response = _mapper.Map<List<ComboResponse>>(result);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpGet("GetComboById/{id}")]
        public ActionResult GetCombo(int id)
        {
            ComboResponse rs = new ComboResponse();
            try
            {
                var result = comboRepository.FindComboById(id);
                rs = _mapper.Map<ComboResponse>(result);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpGet("GetListComboByCurri/{curri_Id}")]
        public ActionResult GetListComboByCurriculum(int curri_Id)
        {
            List<Combo> rs = new List<Combo>();
            try
            {
                rs = comboRepository.GetListComboByCurriId(curri_Id);
                var rsCombo = _mapper.Map<List<ComboResponse>>(rs);
                return Ok(new BaseResponse(false, "Sucessfully", rsCombo));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }


        [HttpPost]
        public ActionResult CreateCombo(ComboRequest cb)
        {
            try
            {
                Combo rs = _mapper.Map<Combo>(cb);
                bool checkCode = comboRepository.IsCodeExist(rs.combo_code,rs.specialization_id);
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
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpPost("DisableCombo")]
        public ActionResult DisableCombo(int id)
        {
            try
            {
              
                bool rs = comboRepository.DisableCombo(id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
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
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
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
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
    }
}
