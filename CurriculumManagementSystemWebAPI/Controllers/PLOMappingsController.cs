using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.CurriculumSubjects;
using Repositories.PLOMappings;
using Repositories.PLOS;
using Repositories.Subjects;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PLOMappingsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IPLOMappingRepository _repo;
        private IPLOsRepository _repo1 = new PLOsRepository();
        private ISubjectRepository _repo2 = new SubjectRepository();

        public PLOMappingsController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new PLOMappingRepository();
        }

        [HttpGet("GetAllPLOMapping/{curriculumId}")]
        public ActionResult GetAllPLOMapping(int curriculumId)
        {
            var listPLOMapping = _repo.GetPLOMappingsInCurriculum(curriculumId);

            var listPLO = _repo1.GetListPLOsByCurriculum(curriculumId);
            var listSubject = _repo2.GetSubjectByCurriculum(curriculumId);

            var listPLOMappingResponse = new List<PLOMappingDTO>();

                foreach (var subject in listSubject)
                {
                    if (!SubjectExsit(subject.subject_id, listPLOMappingResponse))
                    {
                        PLOMappingDTO dto = new PLOMappingDTO();
                        dto.subject_code = subject.subject_code;
                        dto.subject_id = subject.subject_id;
                        dto.PLOs = new Dictionary<string, bool>();
                        foreach (var plo in listPLO)
                        {
                            dto.PLOs.Add($"{plo.PLO_id}-{plo.PLO_name}", false);
                        }
                        listPLOMappingResponse.Add(dto);
                    }
                }
                foreach (var item in listPLOMappingResponse)
                {
                        foreach (var plo in listPLO)
                        {
                            if (CheckPLOIsMapping(plo.PLO_id, item.subject_id, listPLOMapping))
                            {
                               item.PLOs[$"{plo.PLO_id}-{plo.PLO_name}"] = true;
                            }
                        }
                }
            

            return Ok(new BaseResponse(false, "List PLO Mapping", listPLOMappingResponse));
        }

        [HttpPost("CreatePLOMapping")]
        public ActionResult CreatePLOMapping([FromBody] List<PLOMappingRequest> pLOMappingRequest)
        {

            return Ok(new BaseResponse(false, "List PLO Mapping"));
        }


        private bool CheckPLOIsMapping(int plo_id, int subject_id, List<PLOMapping> pLOMappings)
        {
            foreach (var item in pLOMappings)
            {
                if (item.PLO_id == plo_id && subject_id == item.subject_id)
                {
                    return true;
                }
            }
            return false;
        }

        private bool SubjectExsit(int subjectId, List<PLOMappingDTO> pLOMappings)
        {
            foreach (var item in pLOMappings)
            {
                if (subjectId == item.subject_id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
