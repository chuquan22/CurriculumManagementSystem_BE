using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Mvc;
using Repositories.Major;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IMajorRepository repo;

        public MajorsController( IMapper mapper)
        {
            _mapper = mapper;
            repo = new MajorRepository();
        }
        [HttpGet]
        public ActionResult GetAllMajor()
        {
            List<Major> rs = new List<Major>();
            try
            {
                rs = repo.GetAllMajor();
                return Ok(new BaseResponse(false, "Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "False", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpPost]
        public ActionResult CreateMajor(MajorRequest major)
        {
            try
            {
                Major rs = _mapper.Map<Major>(major);
                rs = repo.AddMajor(rs);
                return Ok(new BaseResponse(false, "Create Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "False", null));
            }
            return Ok(new BaseResponse(true, "Create Major False", null));
        }

        [HttpPut]
        public ActionResult EditMajor(MajorEditRequest major)
        {
            try
            {
                Major rs = _mapper.Map<Major>(major);
                rs = repo.EditMajor(rs);
                return Ok(new BaseResponse(false, "Edit Sucessfully", rs));
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "False", null));

            }
            return Ok(new BaseResponse(true, "Edit False", null));
        }

        [HttpDelete]
        public ActionResult DeleteMajor(int id)
        {
            Major rs = new Major();
            try
            {
                var major = repo.FindMajorById(id);
                if(major != null)
                {
                    Ok(new BaseResponse(false, "Không tìm thấy id trong hệ thống", null));
                }
                repo.DeleteMajor(id);
                return Ok(new BaseResponse(false, "Delete Sucessfully", major));
            }
            catch (Exception)
            {
                return BadRequest(new BaseResponse(true, "False", null));
            }
            return Ok(new BaseResponse(true, "Delete False", null));
        }
    }
}
