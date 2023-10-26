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
                Major checkCode = repo.CheckMajorbyMajorCode(rs.major_code);
                if(checkCode == null)
                {
                    rs = repo.AddMajor(rs);
                    return Ok(new BaseResponse(false, "Create major sucessfully", rs));
                }
                else
                {
                    return BadRequest(new BaseResponse(true, "Major code already exist!.", null));
                }


            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "Create major false.", null));
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
                return Ok(new BaseResponse(false, "Edit major sucessfully.", rs));
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
                    Ok(new BaseResponse(false, "Cant not delete this major. Major id not found in system!", null));
                }
                repo.DeleteMajor(id);
                return Ok(new BaseResponse(false, "Delete major sucessfully!", major));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Cant not delete this major. Major already used in system!", null));
            }
            return Ok(new BaseResponse(true, "Delete False", null));
        }
    }
}
