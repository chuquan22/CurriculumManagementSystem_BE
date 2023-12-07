using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Major;
using Repositories.Specialization;
using Repositories.Subjects;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
   // [Authorize(Roles = "Manager, Dispatcher")]
    [ApiController]
    public class MajorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IMajorRepository majorRepository;
        private ISpecializationRepository specializationRepository;
        private ISubjectRepository subjectRepository;

        public MajorsController( IMapper mapper)
        {
            _mapper = mapper;
            majorRepository = new MajorRepository();
            specializationRepository = new SpecializationRepository();
            subjectRepository = new SubjectRepository();
        }
        [HttpGet]
        public ActionResult<List<MajorResponse>> GetAllMajor()
        {
            List<Major> rs = new List<Major>();
            try
            {
                rs = majorRepository.GetAllMajor();
                var result = _mapper.Map<List<MajorResponse>>(rs);
                return Ok(new BaseResponse(false, "Successfully!", result));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true,"Error: " + ex.Message));
            }
        }

        [HttpGet("GetAllMajorSubjectByDegreeLevel/{degree_id}")]
        public ActionResult GetAllMajorSubject(int degree_id)
        {
            List<Major> rs = new List<Major>();
            try
            {
                rs = majorRepository.GetMajorByDegreeLevel(degree_id);
                var listMajorRespone = _mapper.Map<List<MajorSubjectDTOResponse>>(rs);

                foreach (var major in listMajorRespone)
                {
                    var list = _mapper.Map<List<SubjectDTO>>(subjectRepository.GetSubjectByMajorId(major.major_id));
                    major.listSubjects = list.OrderBy(x => x.subject_name).ToList();
                }

                listMajorRespone = listMajorRespone
                    .OrderBy(x => x.major_english_name)
                    .ToList();


                return Ok(new BaseResponse(false, "Successfully!", listMajorRespone));
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message));
            }
        }

        [HttpGet("{batchId}")]
        public ActionResult<List<SpecializationResponse>> GetMajor(int batchId)
        {
            List<Major> rs = new List<Major>();
            try
            {
                rs = majorRepository.GetMajorByBatch(batchId);
                if(rs == null)
                {
                    return NotFound(new BaseResponse(true, "Not Found This Major!"));

                }
                var listMajorRespone = _mapper.Map<List<MajorSpeResponse>>(rs);
                foreach (var major in listMajorRespone)
                {
                    major.lisSpe = _mapper.Map<List<SpecializationResponse>>(specializationRepository.GetSpeByBatchId(batchId, major.major_id));
                }
                return Ok(new BaseResponse(false, "Successfully!", listMajorRespone));
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
                Major checkCode = majorRepository.CheckMajorbyMajorCode(rs.major_code,rs.degree_level_id);
                if(checkCode != null)
                {
                    return BadRequest(new BaseResponse(true, "Major Code Duplicate!.", null));
                }
                else
                {
                    rs = majorRepository.AddMajor(rs);
                    var result = _mapper.Map<MajorResponse>(rs);

                    return Ok(new BaseResponse(false, "Add Major Successfully!", result));
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
            try
            {
                var major = majorRepository.FindMajorById(id);
                if (major == null)
                {
                    return NotFound(new BaseResponse(true, "Not Found This Major!", null));
                }

                majorRepository.DeleteMajor(id);
                return Ok(new BaseResponse(false, "Delete Major Successfully!", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Cannot delete this major. Major already used in the system!", null));
            }
        }

    }
}
