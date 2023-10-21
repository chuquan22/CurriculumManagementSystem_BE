using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using AutoMapper;
using Repositories.Subjects;
using Repositories.Curriculums;
using DataAccess.Models.DTO.response;
using DataAccess.Models.DTO.request;
using System.Diagnostics.CodeAnalysis;
using DataAccess.Models.Enums;
using Repositories.CurriculumSubjects;
using MiniExcelLibs;
using DataAccess.Models.DTO.Excel;
using System.IO;
using MiniExcelLibs.OpenXml;
using Repositories.Batchs;
using Repositories.Major;
using Newtonsoft.Json.Linq;
using Repositories.Specialization;
using Repositories.PLOS;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumsController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurriculumRepository _curriculumRepository = new CurriculumRepository();
        private readonly ICurriculumSubjectRepository _curriculumsubjectRepository = new CurriculumSubjectRepository();
        private readonly IBatchRepository _batchRepository = new BatchRepository();
        private readonly IMajorRepository _majorRepository = new MajorRepository();
        private readonly ISpecializationRepository _specializationRepository = new SpecializationRepository();
        private readonly IPLOsRepository _ploRepository = new PLOsRepository();
        private readonly ISubjectRepository _subjectRepository = new SubjectRepository();

        public CurriculumsController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Curriculums/GetCurriculumByBatch/code/5
        [HttpGet("GetCurriculumByBatch/{curriculumCode}/{batchId}")]
        public async Task<ActionResult<IEnumerable<CurriculumResponse>>> GetCurriculumByBatch(string curriculumCode, int batchId)
        {
            if (_context.Curriculum == null)
            {
                return NotFound();
            }
            var Curriculum = _curriculumRepository.GetCurriculum(curriculumCode, batchId);
            if (Curriculum == null)
            {
                return Ok(new BaseResponse(true, "Not Found Curriculum"));
            }
            var CurriculumRespone = _mapper.Map<CurriculumResponse>(Curriculum);
            return Ok(new BaseResponse(false, "list Curriculums", CurriculumRespone));
        }

        // GET: api/Curriculums/GetListBatchByCurriculumCode/code
        [HttpGet("GetListBatchByCurriculumCode/{curriculumCode}")]
        public async Task<ActionResult<IEnumerable<Batch>>> GetListBatch(string curriculumCode)
        {

            var listBatch = _curriculumRepository.GetBatchByCurriculumCode(curriculumCode);
            if (listBatch.Count == 0)
            {
                return BadRequest(new BaseResponse(true, "Cannot Found Batch By this curriculum"));
            }
            return Ok(new BaseResponse(false, "list Batch", listBatch));
        }


        // GET: api/Curriculums/Pagination/5/6/search/1
        [HttpGet("Pagination/{page}/{limit}")]
        public async Task<ActionResult<IEnumerable<SubjectResponse>>> PaginationCurriculum(int page, int limit, [FromQuery] string? txtSearch, [FromQuery] int? specializationId)
        {
            var listCurriculum = _curriculumRepository.PanigationCurriculum(page, limit, txtSearch, specializationId);

            if (listCurriculum.Count == 0)
            {
                return Ok(new BaseResponse(true, "Not Found Subject"));
            }
            var totalElement = _curriculumRepository.GetAllCurriculum(txtSearch, specializationId).Count();

            var subjectRespone = _mapper.Map<List<CurriculumResponse>>(listCurriculum);

            foreach (var curriculum in subjectRespone)
            {
                curriculum.total_credit = _curriculumRepository.GetTotalCredit(curriculum.curriculum_id);
            }

            return Ok(new BaseResponse(false, "Get List Curriculum Sucessfully", new BaseListResponse(page, limit, totalElement, subjectRespone)));

        }

        // GET: api/Curriculums/GetCurriculum/5
        [HttpGet("GetCurriculum/{id}")]
        public async Task<ActionResult<CurriculumResponse>> GetCurriculum(int id)
        {
            var curriculum = _curriculumRepository.GetCurriculumById(id);

            if (curriculum == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found This Curriculum!"));
            }
            var curriculumResponse = _mapper.Map<CurriculumResponse>(curriculum);
            return Ok(new BaseResponse(false, "Curriculum", curriculumResponse));
        }

        // GET: api/Curriculums/GetListBatchNotInCurriculum/code
        [HttpGet("GetListBatchNotInCurriculum/{curriculumCode}")]
        public async Task<ActionResult<Batch>> GetlistBatch(string curriculumCode)
        {
            var batch = _curriculumRepository.GetListBatchNotExsitInCurriculum(curriculumCode);

            if (batch.Count == 0)
            {
                return BadRequest(new BaseResponse(true, "Not Found Batch Not Exsit in Curriculum!"));
            }
            return Ok(new BaseResponse(false, "Curriculum", batch));
        }


        // PUT: api/Curriculums/UpdateCurriculum/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCurriculum/{id}")]
        public async Task<IActionResult> PutCurriculum(int id, [FromBody] CurriculumUpdateRequest curriculumRequest)
        {
            if (!CurriculumExists(id))
            {
                return NotFound(new BaseResponse(true, "Can't Update because not found curriculum"));
            }
            var curriculum = _curriculumRepository.GetCurriculumById(id);
            _mapper.Map(curriculumRequest, curriculum);
            curriculum.updated_date = DateTime.Today;
            string updateResult = _curriculumRepository.UpdateCurriculum(curriculum);

            if (!updateResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update Curriculum Success!", curriculumRequest));
        }

        // POST: api/Curriculums/CreateCurriculum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateCurriculum")]
        public async Task<ActionResult<Curriculum>> PostCurriculum([FromBody] CurriculumRequest curriculumRequest)
        {
            var curriculum = _mapper.Map<Curriculum>(curriculumRequest);

            curriculum.curriculum_code = _curriculumRepository.GetCurriculumCode(curriculum.batch_id, curriculum.specialization_id);
            curriculum.total_semester = 7;

            if (CheckCurriculumExists(curriculum.curriculum_code, curriculum.batch_id))
            {
                return BadRequest(new BaseResponse(true, "Curriculum Existed. Please Create other curriculum!"));
            }
            string createResult = _curriculumRepository.CreateCurriculum(curriculum);
            if (!createResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create Curriculum Success!", curriculumRequest));
        }

        // DELETE: api/Curriculums/DeleteCurriculum/5
        [HttpDelete("DeleteCurriculum/{id}")]
        public async Task<IActionResult> DeleteCurriculum(int id)
        {
            if (CheckCurriculumCanDelete(id))
            {
                return Ok(new BaseResponse(true, "Can not Delete this Curriculum!"));
            }
            var curriculum = _curriculumRepository.GetCurriculumById(id);
            if (curriculum == null)
            {
                return NotFound(new BaseResponse(true, "Not found this curriculum"));
            }

            string deleteResult = _curriculumRepository.RemoveCurriculum(curriculum);
            if (!deleteResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }
            return Ok(new BaseResponse(false, "Delete Curriculum Successfull!"));
        }

        // Post: Import Curriculum by Excel File
        [HttpPost("ImportCurriculum")]
        public async Task<IActionResult> ImportCurriculum(IFormFile fileCurriculum)
        {
            try
            {
                var filePath = Path.GetTempFileName();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileCurriculum.CopyToAsync(stream);
                }

                //Get SheetName
                var sheetNames = MiniExcel.GetSheetNames(filePath);
                string result = "";
                List<object> rs = new List<object>();
                var curriculum_id = 0;
                for (int i = 0; i < sheetNames.Count; i++)
                {
                    
                    if (i == 0)
                    {
                        var row = MiniExcel.Query<CurriculumExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                        var curriculumExcel = GetCurriculumInExcel(row);
                        var curriculum = _curriculumRepository.GetCurriculum(curriculumExcel.curriculum_code, curriculumExcel.batch_id);
                        
                        if(curriculum != null)
                        {
                            return Ok(new BaseResponse(true, "Curriculum Exsited"));
                        }
                        _curriculumRepository.CreateCurriculum(curriculumExcel);
                        if(curriculumExcel.curriculum_id == 0)
                        {
                            return Ok(new BaseResponse(true, "Create Curriculum Fail"));
                        }
                        curriculum_id = curriculumExcel.curriculum_id;
                    }
                    else if (i == 1)
                    {
                        var row = MiniExcel.Query<PLOExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                        
                        foreach (var item in row)
                        {
                            var plo = new PLOs();
                            plo.curriculum_id = curriculum_id;
                            plo.PLO_name = item.PLO_name;
                            plo.PLO_description = item.PLO_description;
                            
                            if(!_ploRepository.CheckPLONameExsit(plo.PLO_name, plo.curriculum_id))
                            {
                                _ploRepository.CreatePLOs(plo);
                            }
                           
                        }
                    }
                    else if (i == 2)
                    {

                        var row = MiniExcel.Query<CurriculumSubjectExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                        foreach (var item in row)
                        {
                            var curriculumSubject = new CurriculumSubject();
                            curriculumSubject.curriculum_id = curriculum_id;
                            var subject = _subjectRepository.GetSubjectByCode(item.subject_code);
                            curriculumSubject.subject_id = subject.subject_id;
                            curriculumSubject.term_no = item.term_no;
                            curriculumSubject.option = (item.option == null || item.option.Equals("")) ?  false :  true;

                            _curriculumsubjectRepository.CreateCurriculumSubject(curriculumSubject);
                        }
                        
                    }
                    //else if (i == 3)
                    //{

                    //    var row = MiniExcel.Query(filePath, sheetName: sheetNames[i], useHeaderRow: true, startCell: "A2", excelType: ExcelType.XLSX);
                    //    var value = new
                    //    {
                    //        PLOMapping = row,
                    //    };

                    //    rs.Add(row);
                    //}
                }

                return Ok(new BaseResponse(false, "Success", rs));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(false, "Error: " + ex.InnerException, null));
            }
        }

        private Curriculum GetCurriculumInExcel(IEnumerable<CurriculumExcel> row)
        {
            var curriculum = new Curriculum();
            var major = new Major();
            foreach (var r in row)
            {
                // if title is null -> nothing
                if (r.Title == null) { }
                // if Title equal Curriculum Name -> set curriculum_name = value in coloum detail
                else if (r.Title.Equals("Curriculum Name"))
                {
                    curriculum.curriculum_name = r.Details;
                }
                // if Title equal Curriculum Code -> set curriculum_code = value in coloum detail
                else if (r.Title.Equals("Curriculum Code"))
                {
                    curriculum.curriculum_code = r.Details;
                    // Split string ex: GD-GD-19.4
                    string[] parts = r.Details.Split('-');
                    // get part have index 2 in array string ex: 19.4
                    string batch_name = parts[2];
                    // get batch_id by batch_name
                    curriculum.batch_id = _batchRepository.GetBatchIDByName(batch_name);
                    //
                    string specializationCode = parts[1];
                    curriculum.specialization_id = _specializationRepository.GetSpecializationIdByCode(specializationCode);
                }
                // if Title equal English Curriculum Name -> set english_curriculum_name = value in coloum detail
                else if (r.Title.Equals("English Curriculum Name"))
                {
                    curriculum.english_curriculum_name = r.Details;
                }
                // if Title equal Curriculum Description -> set curriculum_description = value in coloum detail
                else if (r.Title.Equals("Curriculum Description"))
                {
                    curriculum.curriculum_description = r.Details;
                }
                // if Title equal Decision No. -> set decision_No = value in coloum detail
                else if (r.Title.Equals("Decision No."))
                {
                    curriculum.decision_No = r.Details;
                }
                // if Title equal Approved date -> set approved_date = value in coloum detail
                else if (r.Title.Equals("Approved date"))
                {
                    curriculum.approved_date = DateTime.Parse(r.Details);
                }else if(r.Title.Equals("Vocational Name"))
                {
                    major.major_name = r.Details;
                }
                else if (r.Title.Equals("English Vocational Name"))
                {
                    major.major_english_name = r.Details;
                }
                else if(r.Title.Equals("Vocational Code"))
                {
                    // set major_code = value in coloum detail
                    major.major_code = r.Details;
                }
            }
            curriculum.total_semester = 7;
            curriculum.is_active = true;

            major.is_active = true;
            var majors = _majorRepository.CheckMajorbyMajorCode(major.major_code);
            // if major not exsited -> add major
            if (majors == null)
            {
                _majorRepository.AddMajor(major);
            }
            return curriculum;
        }

        

            private bool CurriculumExists(int id)
        {
            return (_context.Curriculum?.Any(e => e.curriculum_id == id)).GetValueOrDefault();
        }

        private bool CheckCurriculumExists(string code, int batchId)
        {
            return (_context.Curriculum?.Any(e => e.curriculum_code.Equals(code) && e.batch_id == batchId)).GetValueOrDefault();
        }

        private bool CheckCurriculumCanDelete(int id)
        {
            return (_context.CurriculumSubject?.Any(e => e.curriculum_id == id)).GetValueOrDefault();
        }


     

    }
}
