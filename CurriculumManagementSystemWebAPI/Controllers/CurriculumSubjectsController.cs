﻿using System;
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

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumSubjectsController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurriculumSubjectRepository _curriculumSubjectRepository = new CurriculumSubjectRepository();
        private readonly IComboRepository _comboRepository = new ComboRepository();

        public CurriculumSubjectsController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/CurriculumSubjects/GetCurriculumSubjectByTermNo/3
        [HttpGet("GetCurriculumSubjectByTermNo/{termNo}")]
        public async Task<ActionResult<IEnumerable<CurriculumSubjectResponse>>> GetCurriculumSubjectByTermNo(int termNo)
        {
            
            var curriculumSubject = _curriculumSubjectRepository.GetCurriculumSubjectByTermNo(termNo);

            if(curriculumSubject == null)
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
           
            var curriculumSubjectResponse = new List<CurriculumSubjectDTO>();

            for (int semesterNo = 1; semesterNo <= 7; semesterNo++)
            {
                var newCurriculumSubjectDTO = new CurriculumSubjectDTO
                {
                    semester_no = semesterNo.ToString(),
                    list = new List<CurriculumSubjectResponse>()
                };
                curriculumSubjectResponse.Add(newCurriculumSubjectDTO);
            }

            foreach (var curriSubject in curriculumSubject)
            {
                var curriculumSubjectMapper = _mapper.Map<CurriculumSubjectResponse>(curriSubject);
                if(curriSubject.combo_id == null)
                {
                    curriSubject.combo_id = 0;
                }

                if (curriSubject.combo_id != 0)
                {
                    curriculumSubjectMapper.combo_name = _comboRepository.FindComboById((int)curriSubject.combo_id).combo_english_name;
                }

                foreach(var curriSubjectResponse  in curriculumSubjectResponse)
                {
                    if(curriSubject.term_no.ToString() == curriSubjectResponse.semester_no)
                    {
                        curriSubjectResponse.list.Add(curriculumSubjectMapper);
                    }
                }
                
            }

            return Ok(new BaseResponse(false, "Success!", curriculumSubjectResponse));
        }

        // GET: api/CurriculumSubjects/GetSubjectNotExsitCurriculum/5
        [HttpGet("GetSubjectNotExsitCurriculum/{curriculumId}")]
        public async Task<ActionResult<CurriculumSubjectResponse>> GetSubjectNotExistCurriculum(int curriculumId)
        {
            var subject = _curriculumSubjectRepository.GetListSubject(curriculumId);
            if(subject.Count == 0)
            {
                return Ok(new BaseResponse(false, "Not Found Subject!"));
            }
            var subjectResponse = _mapper.Map<List<SubjectResponse>>(subject);
            return Ok(new BaseResponse(false, "List Subject", subjectResponse));
        }

            // POST: api/CurriculumSubjects/CreateCurriculumSubject
            [HttpPost("CreateCurriculumSubject")]
        public async Task<ActionResult<CurriculumSubject>> PostCurriculumSubject([FromBody] CurriculumSubjectRequest curriculumSubjectRequest)
        {
           
            var curriculumSubject = _mapper.Map<CurriculumSubject>(curriculumSubjectRequest);

            string createResult = _curriculumSubjectRepository.CreateCurriculumSubject(curriculumSubject);

            if(createResult != Result.createSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create success!", curriculumSubjectRequest));
        }

        // Delete: api/CurriculumSubjects/DeleteCurriculum/1/2
        [HttpDelete("DeleteCurriculum/{curriId}/{subId}")]
        public async Task<IActionResult> DeleteCurriculumSubject(int curriId, int subId)
        {
            if (_context.CurriculumSubject == null)
            {
                return NotFound();
            }
            if(!CurriculumSubjectExists(curriId, subId))
            {
                return NotFound(new BaseResponse(true, "Not found this Curriculum Subject"));
            }
            var curriculumSubject = _curriculumSubjectRepository.GetCurriculumSubjectById(curriId, subId);

            string deleteResult = _curriculumSubjectRepository.DeleteCurriculumSubject(curriculumSubject);
            if(deleteResult != Result.deleteSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, "delete successfull!"));
        }

        private bool CurriculumSubjectExists(int curriId, int subId)
        {
            return (_context.CurriculumSubject?.Any(e => e.curriculum_id == curriId && e.subject_id==subId)).GetValueOrDefault();
        }

        
    }
}
