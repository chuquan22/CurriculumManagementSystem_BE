using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.CurriculumCombo;


namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumCombosController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurriculumComboRepository _curriculumRepository = new CurriculumComboRepository();

        public CurriculumCombosController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: api/CurriculumCombos/GetCurriculumCombo/3
        [HttpGet("GetCurriculumCombo/{curriculumId}")]
        public async Task<ActionResult<IEnumerable<ComboCurriculum>>> GetCurriculumCombo(int curriculumId)
        {
            var curriculumCombo = _curriculumRepository.GetListComboByCurriculum(curriculumId);
            if(curriculumCombo == null)
            {
                return NotFound(new BaseResponse(true, "Curriculum Hasn't combo!"));
            }
            var curriculumComboResponse = _mapper.Map<CurriculumComboDTOResponse>(curriculumCombo);
            return Ok(new BaseResponse(false, "List Combo", curriculumComboResponse));
        }

        [HttpGet("GetCurriculumComboSubject/{comboId}")]
        public async Task<ActionResult<IEnumerable<ComboCurriculum>>> GetCurriculumComboSubject(int comboId)
        {
            var curriculumComboSubject = _curriculumRepository.GetListSubjectByCombo(comboId);
            if (curriculumComboSubject == null)
            {
                return NotFound(new BaseResponse(true, "Combo Hasn't subject!"));
            }
            var curriculumComboResponse = _mapper.Map<CurriculumComboSubjectDTOResponse>(curriculumComboSubject);
            return Ok(new BaseResponse(false, "List Subject", curriculumComboResponse));
        }



        [HttpPost("CreateCurriculumComboSubject")]
        public async Task<ActionResult<Curriculum>> PostCurriculumComboSubject([FromForm] CurriculumComboSubjectDTORequest curriculumComboSubjectRequest)
        {
            

            return Ok(new BaseResponse(false, "Create Curriculum Success!", curriculumComboSubjectRequest));
        }


        [HttpPut("UpdateCurriculumComboSubject/{id}")]
        public async Task<IActionResult> UpdateCurriculumComboSubject(int id, [FromForm] CurriculumComboSubjectDTORequest curriculumComboSubjectRequest)
        {
            return Ok("Chờ update");
        }

        //Create, Remove

    }
}
