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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

            foreach (var curriSubjectResponse in curriculumSubjectResponse)
            {
                var listCurriSubject = SortListSubject((curriculumSubject.Where(x => x.term_no.ToString() == curriSubjectResponse.semester_no).ToList()));
                var curriculumSubjectMapper = _mapper.Map<List<CurriculumSubjectResponse>>(listCurriSubject);
                foreach (var item in curriculumSubjectMapper)
                {
                    if (item.combo_id != 0 && item.combo_id != null)
                    {
                        item.combo_name = _comboRepository.FindComboById((int)item.combo_id).combo_code;
                    }
                }
                curriSubjectResponse.list = curriculumSubjectMapper;
            }

            foreach (var curriSubjectResponse in curriculumSubjectResponse)
            {
                // credit total all subject no option and no combo
                curriSubjectResponse.total_all_credit += curriSubjectResponse.list.Where(x => x.option == null && x.combo_id == 0).Sum(x => x.credit);
                curriSubjectResponse.total_all_time += curriSubjectResponse.list.Where(x => x.option == null && x.combo_id == 0).Sum(x => x.total_time);
                // credit total only one subject each option
                curriSubjectResponse.total_all_credit += curriSubjectResponse.list.Where(x => x.option != null && x.combo_id == 0).DistinctBy(x => x.option).Sum(x => x.credit);
                curriSubjectResponse.total_all_time += curriSubjectResponse.list.Where(x => x.option != null && x.combo_id == 0).DistinctBy(x => x.option).Sum(x => x.total_time);
                // credit total get only one subject combo
                curriSubjectResponse.total_all_credit += GetSumCreditCurriSubjectCombo(curriSubjectResponse.list);
                curriSubjectResponse.total_all_time += GetSumTimeCurriSubjectCombo(curriSubjectResponse.list);

            }

            return Ok(new BaseResponse(false, "Success!", curriculumSubjectResponse));
        }

        [NonAction]
        private List<CurriculumSubject> SortListSubject(List<CurriculumSubject> listCurriSubject)
        {
            var listComboId = listCurriSubject.Where(x => x.combo_id != 0 && x.combo_id != null).OrderBy(x => x.combo_id).DistinctBy(x => x.combo_id).ToList();

            var curriSubjectCombo = listCurriSubject
                .Where(x => x.combo_id != 0 && x.combo_id != null)
                .OrderBy(x => x.combo_id)
                .ToList();

            var curriSubjectComboOrder = new List<CurriculumSubject>();
            curriSubjectComboOrder = listCurriSubject.Where(x => x.combo_id == 0 || x.combo_id == null).ToList();
            var count = curriSubjectCombo.Count + listCurriSubject.Where(x => x.combo_id == 0 || x.combo_id == null).Count();
            curriSubjectComboOrder = curriSubjectComboOrder
                   .OrderBy(x => x.option == null ? 0 : 1)
                   .ThenBy(x => x.option)
                   .ToList();
            do
            {
                foreach (var id in listComboId)
                {
                    foreach (var item in curriSubjectCombo)
                    {
                        if (item.combo_id == id.combo_id && curriSubjectComboOrder.FirstOrDefault(x => x.subject_id == item.subject_id) == null)
                        {
                            curriSubjectComboOrder.Add(item);
                            break;
                        }
                    }
                }

            } while (curriSubjectComboOrder.Count < count);

            return curriSubjectComboOrder;
        }

        [NonAction]
        private int GetSumCreditCurriSubjectCombo(List<CurriculumSubjectResponse> list)
        {
            int index = 0;
            int credit = 0;
            var listCurriSUbject = list.Where(x => x.combo_id != 0 && x.combo_id != null).ToList();
            foreach (var subjectCombo in listCurriSUbject)
            {
                if (index == listCurriSUbject.DistinctBy(x => x.combo_id).Count() || index == 0)
                {
                    index = 1;
                    credit += subjectCombo.credit;
                }
                else
                {
                    index++;
                }
            }
            return credit;
        }

        [NonAction]
        private int GetSumTimeCurriSubjectCombo(List<CurriculumSubjectResponse> list)
        {
            int index = 0;
            int time = 0;
            var listCurriSUbject = list.Where(x => x.combo_id != 0 && x.combo_id != null).ToList();
            foreach (var subjectCombo in listCurriSUbject)
            {
                if (index == listCurriSUbject.DistinctBy(x => x.combo_id).Count() || index == 0)
                {
                    index = 1;
                    time += subjectCombo.total_time;
                }
                else
                {
                    index++;
                }
            }
            return time;
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

            if (curriculumSubjectRequest.Count == 2 && curriculumSubjectRequest.First(x => x.subject_group.Equals("Elective subjects")) != null)
            {
                var curriculumSubject = _curriculumSubjectRepository.GetListCurriculumSubject(curriculumSubjectRequest.First().curriculum_id).Where(x => x.term_no == curriculumSubjectRequest.First().term_no && x.option != null);

                int maxOption = curriculumSubject.OrderByDescending(x => x.option).FirstOrDefault()?.option ?? 0;

                foreach (var item in curriculumSubjectRequest)
                {
                    item.option = maxOption + 1;
                }
            }

            var listCurriSubject = _mapper.Map<List<CurriculumSubject>>(curriculumSubjectRequest);
            if (_curriculumSubjectRepository.CheckSubjectComboOrOptionMustBeEqualCreditAndToTalTime(listCurriSubject))
            {
                return BadRequest(new BaseResponse(true, "Credit or Total Time of Subject Combo or Subject Option must be equal"));
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
            if (curriculumSubject.option != null)
            {
                var curriculumSubject2 = _curriculumSubjectRepository.GetCurriculumSubjectByTermNoAndSubjectGroup(curriculumSubject.curriculum_id, curriculumSubject.term_no, curriculumSubject.subject_id, (int)curriculumSubject.option);
                if (curriculumSubject2 != null)
                {
                    string deleteResult2 = _curriculumSubjectRepository.DeleteCurriculumSubject(curriculumSubject2);
                    if (deleteResult2 != Result.deleteSuccessfull.ToString())
                    {
                        return BadRequest(new BaseResponse(true, "Remove Fail"));
                    }
                }
            }
            string deleteResult = _curriculumSubjectRepository.DeleteCurriculumSubject(curriculumSubject);

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
