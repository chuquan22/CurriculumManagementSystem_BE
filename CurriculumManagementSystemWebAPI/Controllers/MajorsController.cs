using AutoMapper;
using BusinessObject;
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

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpPost]
        public ActionResult CreateMajor(Major major)
        {
            Major rs = new Major();
            try
            {
                rs = repo.AddMajor(major);
                return Ok(new BaseResponse(false, "Create Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "Create Major False", null));
        }

        [HttpPut]
        public ActionResult EditMajor(Major major)
        {
            Major rs = new Major();
            try
            {
                rs = repo.EditMajor(major);
                return Ok(new BaseResponse(false, "Edit Sucessfully", rs));
            }
            catch (Exception)
            {

                throw;
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

                throw;
            }
            return Ok(new BaseResponse(true, "Delete False", null));
        }
    }
}
