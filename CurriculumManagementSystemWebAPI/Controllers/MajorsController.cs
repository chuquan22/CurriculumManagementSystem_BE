using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Mvc;
using Repositories.Major;
using Repositories.Specialization;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IMajorRepository majorRepository;
        private ISpecializationRepository specializationRepository;

        public MajorsController( IMapper mapper)
        {
            _mapper = mapper;
            majorRepository = new MajorRepository();
            specializationRepository = new SpecializationRepository();
        }
        [HttpGet]
        public ActionResult GetAllMajor()
        {
            List<Major> rs = new List<Major>();
            try
            {
                rs = majorRepository.GetAllMajor();
                var result = _mapper.Map<List<MajorResponse>>(rs);
                return Ok(new BaseResponse(false, "Sucessfully", result));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true,"Error: " + ex.Message, null));
            }
        }

        [HttpGet("{batchId}")]
        public ActionResult GetMajor(int batchId)
        {
            List<Major> rs = new List<Major>();
            try
            {
                rs = majorRepository.GetMajorByBatch(batchId);
                var listMajorRespone = _mapper.Map<List<MajorSpeResponse>>(rs);
                foreach (var major in listMajorRespone)
                {
                    major.lisSpe = _mapper.Map<List<SpecializationResponse>>(specializationRepository.GetSpeByBatchId(batchId, major.major_id));
                }
                return Ok(new BaseResponse(false, "Sucessfully", listMajorRespone));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpPost]
        public ActionResult CreateMajor(MajorRequest major)
        {
            try
            {
                Major rs = _mapper.Map<Major>(major);
                Major checkCode = majorRepository.CheckMajorbyMajorCode(rs.major_code);
                if(checkCode != null)
                {
                    return BadRequest(new BaseResponse(true, "Major Code Duplicate!.", null));
                }
                else
                {
                    rs = majorRepository.AddMajor(rs);
                    var result = _mapper.Map<MajorResponse>(rs);

                    return Ok(new BaseResponse(false, "Add +"+rs.major_name+"+ successful!", result));
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Add major false. Error: " + ex.Message, null));
            }
        }

        [HttpPut]
        public ActionResult EditMajor(MajorEditRequest major)
        {

            try
            {
                Major rs = _mapper.Map<Major>(major);            
                rs = majorRepository.EditMajor(rs);
                return Ok(new BaseResponse(false, "Edit major sucessfully.", rs));
                
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));

            }
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
        }
    }
}
