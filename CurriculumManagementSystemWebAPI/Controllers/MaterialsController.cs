using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Materials;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    //  [Authorize(Roles = "Manager, Dispatcher")]

    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IMaterialRepository repo;

        public MaterialsController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new MaterialRepository();
        }
        [HttpGet]
        public ActionResult<List<MaterialsResponse>> GetMaterial(int syllabus_id)
        {
            List<Material> rs = new List<Material>();
            try
            {
                rs = repo.GetMaterial(syllabus_id);

                var result = _mapper.Map<List<MaterialsResponse>>(rs);
                return Ok(new BaseResponse(false, "Successfully!", result));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpPost]
        public ActionResult CreateMaterial(MaterialRequest material)
        {
            try
            {
                Material rs = _mapper.Map<Material>(material);

                rs = repo.CreateMaterial(rs);
                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));


            }
        }

        [HttpPut]
        public ActionResult EditMaterial(MaterialUpdateRequest material)
        {
            try
            {
                Material rs = repo.GetMaterialById(material.material_id);
                if (rs == null)
                {
                    return NotFound(new BaseResponse(true, "Not Found The Material!", null));
                }

                rs = _mapper.Map<Material>(material);
                rs = repo.EditMaterial(rs);

                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpDelete]
        public ActionResult DeleteMaterial(int id)
        {
            try
            {
                Material rs = repo.GetMaterialById(id);
                if (rs == null)
                {
                    return NotFound(new BaseResponse(true, "Not Found The Material!", null));
                }

                rs = repo.DeleteMaterial(id);

                return Ok(new BaseResponse(false, "Successfully!", rs));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetMaterialById(int id)
        {
            try
            {
                Material rs = repo.GetMaterialById(id);
                if (rs == null)
                {
                    return NotFound(new BaseResponse(true, "Not Found The Material!", null));
                }

                var result = _mapper.Map<MaterialsResponse>(rs);

                return Ok(new BaseResponse(false, "Successfully!", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

    }
}