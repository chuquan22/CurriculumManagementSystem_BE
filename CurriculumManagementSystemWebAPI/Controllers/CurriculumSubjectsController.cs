using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using AutoMapper;
using Repositories.Curriculums;
using Repositories.CurriculumSubjects;
using DataAccess.Models.DTO.response;
using DataAccess.Models.DTO.request;
using DataAccess.Models.Enums;
using Repositories.Combos;
using Microsoft.AspNetCore.Authorization;
using Repositories.Subjects;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager, Dispatcher")]
    [ApiController]
    public class CurriculumSubjectsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICurriculumSubjectRepository _curriculumSubjectRepository = new CurriculumSubjectRepository();
        private readonly ICurriculumRepository _curriculumRepository = new CurriculumRepository();
        private readonly IComboRepository _comboRepository = new ComboRepository();
        private readonly ISubjectRepository _subjectRepository = new SubjectRepository();

        public CurriculumSubjectsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/CurriculumSubjects/GetCurriculumSubjectByTermNo/3
        [HttpGet("GetCurriculumSubjectByTermNo/{termNo}")]
        public async Task<ActionResult<IEnumerable<CurriculumSubjectResponse>>> GetCurriculumSubjectByTermNo(int termNo)
        {

            var curriculumSubject = _curriculumSubjectRepository.GetCurriculumSubjectByTermNo(termNo);

            if (curriculumSubject == null)
            {
                return BadRequest(new BaseResponse(true, $"Term No {termNo} Hasn't Subject in this Curriculum"));
            }
            var curriculumSubjectResponse = _mapper.Map<List<CurriculumSubjectResponse>>(curriculumSubject);

            return Ok(new BaseResponse(false, "success!", curriculumSubjectResponse));
        }

        // GET: api/CurriculumSubjects/GetCurriculumBySubject/5
        [HttpGet("GetCurriculumBySubject/{subjectId}")]
        public async Task<ActionResult<CurriculumSubjectResponse>> GetCurriculumBySubject(int subjectId)
        {

            var curriculumSubject = _curriculumSubjectRepository.GetListCurriculumBySubject(subjectId);

            if (curriculumSubject == null)
            {
                return NotFound();
            }
            var curriculumSubjectResponse = _mapper.Map<List<CurriculumSubjectResponse>>(curriculumSubject);

            return Ok(new BaseResponse(false, "Success!", curriculumSubjectResponse));
        }


        // GET: api/CurriculumSubjects/GetSubjectByCurriculum/5
        [HttpGet("GetSubjectByCurriculum/{curriculumId}")]
        public async Task<ActionResult<CurriculumSubjectResponse>> GetSubjectByCurriculum(int curriculumId)
        {
            var curriculumSubject = _curriculumSubjectRepository.GetListCurriculumSubject(curriculumId);
            var curriculum = _curriculumRepository.GetCurriculumById(curriculumId);

            var curriculumSubjectResponse = new List<CurriculumSubjectDTO>();

            for (int semesterNo = 1; semesterNo <= curriculum.total_semester; semesterNo++)
            {
                var newCurriculumSubjectDTO = new CurriculumSubjectDTO
                {
                    total_all_credit = 0,
                    total_all_time = 0,
                    semester_no = semesterNo.ToString(),
                    list = new List<CurriculumSubjectResponse>()
                };
                curriculumSubjectResponse.Add(newCurriculumSubjectDTO);
            }
            foreach (var curriSubject in curriculumSubject)
            {
                var curriculumSubjectMapper = _mapper.Map<CurriculumSubjectResponse>(curriSubject);

                if (curriSubject.combo_id != 0 && curriSubject.combo_id != null)
                {
                    curriculumSubjectMapper.combo_name = _comboRepository.FindComboById((int)curriSubject.combo_id).combo_code;
                }

                

                foreach (var curriSubjectResponse in curriculumSubjectResponse)
                {
                    if (curriSubject.term_no.ToString() == curriSubjectResponse.semester_no)
                    {
                        curriSubjectResponse.list.Add(curriculumSubjectMapper);
                    }
                    curriSubjectResponse.total_all_credit = curriSubjectResponse.list.Sum(x => x.credit);
                    curriSubjectResponse.total_all_time = curriSubjectResponse.list.Sum(x => x.total_time);
                }
            }
            foreach (var curriSubjectResponse in curriculumSubjectResponse)
            {
                curriSubjectResponse.list = curriSubjectResponse.list
                    .OrderBy(x => x.combo_id == 0 ? 0 : 1)
                    .ThenBy(x => x.option == null ? 0 : 1)
                    .ThenBy(x => x.option)
                    .ToList();
            }

            return Ok(new BaseResponse(false, "Success!", curriculumSubjectResponse));
        }

        // GET: api/CurriculumSubjects/GetSubjectNotExsitCurriculum/5
        [HttpGet("GetSubjectNotExsitCurriculum/{curriculumId}")]
        public async Task<ActionResult<CurriculumSubjectResponse>> GetSubjectNotExistCurriculum(int curriculumId)
        {
            var subject = _curriculumSubjectRepository.GetListSubject(curriculumId);
            if (subject.Count == 0)
            {
                return Ok(new BaseResponse(false, "Not Found Subject!"));
            }
            var subjectResponse = _mapper.Map<List<SubjectResponse>>(subject);
            return Ok(new BaseResponse(false, "List Subject", subjectResponse));
        }

        // POST: api/CurriculumSubjects/CreateCurriculumSubject
        [HttpPost("CreateCurriculumSubject")]
        public async Task<ActionResult<CurriculumSubject>> PostCurriculumSubject([FromBody] List<CurriculumSubjectRequest> curriculumSubjectRequest)
        {
            if(curriculumSubjectRequest.Count == 2 && curriculumSubjectRequest.First(x => x.subject_group.Equals("Elective subjects")) != null)
            {
                var curriculumSubject = _curriculumSubjectRepository.GetListCurriculumSubject(curriculumSubjectRequest.First().curriculum_id).Where(x => x.term_no == curriculumSubjectRequest.First().term_no);
                int maxOption = curriculumSubject.Count() == 0 ? 0 : (int)curriculumSubject.Max(x => x.option);
                foreach (var item in curriculumSubjectRequest)
                {
                    item.option = maxOption + 1;
                }
            }

            foreach (var subject in curriculumSubjectRequest)
            {
                var curriculumSubject = _mapper.Map<CurriculumSubject>(subject);

                string createResult = _curriculumSubjectRepository.CreateCurriculumSubject(curriculumSubject);

                if (createResult != Result.createSuccessfull.ToString())
                {
                    return BadRequest(new BaseResponse(true, createResult));
                }
            }
            return Ok(new BaseResponse(false, "Create success!", curriculumSubjectRequest));
        }

        // Delete: api/CurriculumSubjects/DeleteCurriculum/1/2
        [HttpDelete("DeleteCurriculum/{curriId}/{subId}")]
        public async Task<IActionResult> DeleteCurriculumSubject(int curriId, int subId)
        {
            if (!CurriculumSubjectExists(curriId, subId))
            {
                return NotFound(new BaseResponse(true, "Not found this Curriculum Subject"));
            }
            var curriculumSubject = _curriculumSubjectRepository.GetCurriculumSubjectById(curriId, subId);
            var curriculumSubject2 = _curriculumSubjectRepository.GetCurriculumSubjectByTermNoAndSubjectGroup(curriculumSubject.term_no, (int)curriculumSubject.option);

            string deleteResult = _curriculumSubjectRepository.DeleteCurriculumSubject(curriculumSubject);
            if (curriculumSubject2 != null)
            {
                string deleteResult2 = _curriculumSubjectRepository.DeleteCurriculumSubject(curriculumSubject2);
                if (deleteResult2 != Result.deleteSuccessfull.ToString())
                {
                    return BadRequest(new BaseResponse(true, "Remove Fail"));
                }
            }
            if (deleteResult != Result.deleteSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, "Remove Fail"));
            }

            return Ok(new BaseResponse(false, "delete successfull!"));
        }

        private bool CurriculumSubjectExists(int curriId, int subId)
        {
            return _curriculumSubjectRepository.CurriculumSubjectExist(curriId, subId);
        }


    }
}
