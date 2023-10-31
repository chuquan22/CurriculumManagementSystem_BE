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
        private IMajorRepository majorRepository;

        public MajorsController( IMapper mapper)
        {
            _mapper = mapper;
            majorRepository = new MajorRepository();
        }
        [HttpGet]
        public ActionResult GetAllMajor()
        {
            List<Major> rs = new List<Major>();
            try
            {
                rs = majorRepository.GetAllMajor();
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
                Major checkCode = majorRepository.CheckMajorbyMajorCode(rs.major_code);
                Major checkName = majorRepository.CheckMajorbyMajorName(rs.major_name);
                Major checkEngName = majorRepository.CheckMajorbyMajorEnglishName(rs.major_english_name);
                if(checkCode != null)
                {
                    return BadRequest(new BaseResponse(true, "Major Code Duplicate!.", null));
                }else if(checkName != null)
                {
                    return BadRequest(new BaseResponse(true, "Major Name Duplicate.", null));
                }else if(checkEngName != null)
                {
                    return BadRequest(new BaseResponse(true, "Major English Name Duplicate.", null));
                }
                else
                {
                    rs = majorRepository.AddMajor(rs);
                    return Ok(new BaseResponse(false, "Add +"+rs.major_name+"+ successful!", rs));
                }
            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "Add major false.", null));
            }
            return Ok(new BaseResponse(true, "Add major False", null));
        }

        [HttpPut]
        public ActionResult EditMajor(MajorEditRequest major)
        {

            try
            {
                Major rs = _mapper.Map<Major>(major);
                Major checkName = majorRepository.CheckMajorbyMajorName(rs.major_name);
                Major checkEngName = majorRepository.CheckMajorbyMajorEnglishName(rs.major_english_name);
                if (checkName != null)
                {
                    return BadRequest(new BaseResponse(true, "Major Name Duplicate.", null));
                }
                else if (checkEngName != null)
                {
                    return BadRequest(new BaseResponse(true, "Major English Name Duplicate.", null));
                }
                else
                {
                    rs = majorRepository.EditMajor(rs);
                    return Ok(new BaseResponse(false, "Edit major sucessfully.", rs));
                }
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
                var major = majorRepository.FindMajorById(id);
                if(major != null)
                {
                    Ok(new BaseResponse(false, "Cant not delete this major. Major id not found in system!", null));
                }
                majorRepository.DeleteMajor(id);
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
