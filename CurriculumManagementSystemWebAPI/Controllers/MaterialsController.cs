using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Materials;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
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
        public ActionResult GetMaterial(int syllabus_id)
        {
            List<Material> rs = new List<Material>();
            try
            {
                rs = repo.GetMaterial(syllabus_id);
                var result = _mapper.Map<List<MaterialsResponse>>(rs);
                return Ok(new BaseResponse(false, "Sucessfully", result));
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
                return Ok(new BaseResponse(false, "Sucessfully", rs));
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

                Material rs = _mapper.Map<Material>(material);
                rs = repo.EditMaterial(rs);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpDelete]
        public ActionResult DeleteMaterial(int id)
        {
            Material rs = new Material();
            try
            {
                rs = repo.DeleteMaterial(id);
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }
        [HttpGet("id")]
        public ActionResult GetMaterialById(int id)
        {
            Material rs = new Material();
            try
            {
                rs = repo.GetMaterialById(id);
                var result = _mapper.Map<MaterialsResponse>(rs);
                return Ok(new BaseResponse(false, "Sucessfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }

        }
    }
}
