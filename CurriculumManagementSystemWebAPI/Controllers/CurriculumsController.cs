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
using OfficeOpenXml;
using Repositories.PLOMappings;
using Microsoft.AspNetCore.Routing.Template;
using Repositories.Combos;
using System.Text.RegularExpressions;

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
        private readonly IPLOMappingRepository _ploMappingRepository = new PLOMappingRepository();
        private readonly ISubjectRepository _subjectRepository = new SubjectRepository();
        private readonly IComboRepository _comboRepository = new ComboRepository();

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
        public async Task<ActionResult<IEnumerable<SubjectResponse>>> PaginationCurriculum(int page, int limit, [FromQuery] string? txtSearch, [FromQuery] int? majorId)
        {
            var listCurriculum = _curriculumRepository.PanigationCurriculum(page, limit, txtSearch, majorId);

            if (listCurriculum.Count == 0)
            {
                return Ok(new BaseResponse(true, "Not Found Subject"));
            }
            var totalElement = _curriculumRepository.GetAllCurriculum(txtSearch, majorId).Count();

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
            curriculum.is_active = true;
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
        [HttpPost("ExportCurriculum/{curriculumId}")]
        public async Task<IActionResult> ExportCurriculum(int curriculumId)
        {
            string templatePath = "Curriculum.xlsx";
            var curriculum = _curriculumRepository.GetCurriculumById(curriculumId);
            var specialization = _specializationRepository.GetSpeById(curriculum.specialization_id);
            var major = _majorRepository.FindMajorById(curriculum.Specialization.major_id);
            var comboList = _comboRepository.GetListCombo(specialization.specialization_id);
            var subject = _subjectRepository.GetSubjectByCurriculum(curriculumId);
            var subject1 = _subjectRepository.GetListSubjectByTermNo(1, curriculumId);
            var subject2 = _subjectRepository.GetListSubjectByTermNo(2, curriculumId);
            var subject3 = _subjectRepository.GetListSubjectByTermNo(3, curriculumId);
            var subject4 = _subjectRepository.GetListSubjectByTermNo(4, curriculumId);
            var subject5 = _subjectRepository.GetListSubjectByTermNo(5, curriculumId);
            var subject6 = _subjectRepository.GetListSubjectByTermNo(6, curriculumId);

            var comboExcel = "";
            var curriculumExcel = "";
            for (int i = 1; i <= comboList.Count; i++)
            {
                comboExcel += $"CN{i}: " + comboList.Skip(i-1).Select(x => x.combo_name).FirstOrDefault();
                //curriculumExcel += $"CN{i}: " + curriculum.skip
                if(i < comboList.Count)
                {
                    comboExcel += "; ";
                }
            }

            var subjectN1 = GetArraySubject(subject1, subject2, subject3);
            var subjectN2 = GetArraySubject(subject4, subject5, subject6);

            Dictionary<string, object> value = new Dictionary<string, object>()
            {
                ["decision_No"] = curriculum.decision_No,
                ["approved_date"] = $"Ngày {curriculum.approved_date.Day} tháng {curriculum.approved_date.Month} năm {curriculum.approved_date.Year}",
                ["major_name"] = major.major_name,
                ["major_english_name"] = major.major_english_name,
                ["major_code"] = major.major_code,
                ["specialization_name"] = specialization.specialization_name,
                ["specialization_english_name"] = specialization.specialization_english_name,

                ["curriculum_code"] = "CN1:" + curriculum.curriculum_code,
                ["degree_level"] = curriculum.degree_level,
                ["formality"] = curriculum.Formality,

                ["Combo"] = comboExcel,

                ["SubjectN1"] = subjectN1,
                ["SubjectN2"] = subjectN2,

                ["total_credit"] = subject1.Sum(x => x.credit),
                ["total_time"] = subject1.Sum(x => x.total_time),

                ["total_credit_2"] = subject2.Sum(x => x.credit),
                ["total_time_2"] = subject2.Sum(x => x.total_time),

                ["total_credit_3"] = subject3.Sum(x => x.credit),
                ["total_time_3"] = subject3.Sum(x => x.total_time),

                ["total_credit_4"] = subject4.Sum(x => x.credit),
                ["total_time_4"] = subject4.Sum(x => x.total_time),

                ["total_credit_5"] = subject5.Sum(x => x.credit),
                ["total_time_5"] = subject5.Sum(x => x.total_time),

                ["total_credit_6"] = subject6.Sum(x => x.credit),
                ["total_time_6"] = subject6.Sum(x => x.total_time),

                ["total_all_credit"] = subject.Sum(x => x.credit),
                ["total_all_time"] = subject.Sum(x => x.total_time),
                ["total_subject"] = subject.Count

            };

            MemoryStream memoryStream = new MemoryStream();
            memoryStream.SaveAsByTemplate(templatePath, value);
            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] fileContents = memoryStream.ToArray();
            return Ok(fileContents);
            //return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Curriculum.xlsx");
        }


        // Post: Import Curriculum by Excel File
        [HttpPost("ImportCurriculum")]
        public async Task<IActionResult> ImportCurriculum(IFormFile fileCurriculum)
        {
            try
            {
                List<object> rs = new List<object>();

                var filePath = Path.GetTempFileName();
                var curriculum_id = 0;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileCurriculum.CopyToAsync(stream);
                    //Get SheetName
                    var sheetNames = MiniExcel.GetSheetNames(filePath);
                    
                    for (int i = 0; i < sheetNames.Count; i++)
                    {

                        if (i == 0)
                        {
                            var row = MiniExcel.Query<CurriculumExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            string result = ValidationDataExcel(row);
                            if (result.Equals("Success"))
                            {
                                var curriculumExcel = GetCurriculumInExcel(row);
                                var curriculum = _curriculumRepository.GetCurriculum(curriculumExcel.curriculum_code, curriculumExcel.batch_id);

                                if (curriculum != null)
                                {
                                    return BadRequest(new BaseResponse(true, "Curriculum Exsited"));
                                }
                                string createResult = _curriculumRepository.CreateCurriculum(curriculumExcel);
                                if (curriculumExcel.curriculum_id == 0)
                                {
                                    return BadRequest(new BaseResponse(true, createResult));
                                }
                                curriculum_id = curriculumExcel.curriculum_id;
                            }
                            else
                            {
                                return BadRequest(new BaseResponse(true, result));
                            }
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

                                if (!_ploRepository.CheckPLONameExsit(plo.PLO_name, plo.curriculum_id))
                                {
                                    _ploRepository.CreatePLOs(plo);
                                }
                                else
                                {
                                    return BadRequest(new BaseResponse(true, $"{plo.PLO_name} is exsited in curriculum"));
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
                                if (subject == null)
                                {
                                    return Ok(new BaseResponse(true, $"Subject {item.subject_code} Not Found. Please Create Subject"));
                                    //var subjectCreate = new Subject()
                                    //{
                                    //    subject_code = item.subject_code,
                                    //    subject_name = item.subject_name,
                                    //    english_subject_name = item.english_subject_name,
                                    //    credit = item.credit,
                                    //    total_time = 70,
                                    //    total_time_class = 30,
                                    //    is_active = true,
                                    //    assessment_method_id = 1,
                                    //    learning_method_id = 1,
                                    //    exam_total = 3,

                                    //};
                                    //_subjectRepository.CreateNewSubject(subjectCreate);
                                    //subject = subjectCreate;
                                }
                                curriculumSubject.subject_id = subject.subject_id;
                                curriculumSubject.term_no = item.term_no;
                                if (item.combo_code != null)
                                {
                                    var combo = _comboRepository.FindComboByCode(item.combo_code);
                                    if(combo == null)
                                    {
                                        return Ok(new BaseResponse(true, $"Subject {item.combo_code} Not Found. Please Create Combo"));
                                    }
                                    curriculumSubject.combo_id = combo.combo_id;
                                }
                                curriculumSubject.option = (item.option == null || item.option.Equals("")) ? false : true;

                                string createResult = _curriculumsubjectRepository.CreateCurriculumSubject(curriculumSubject);
                                if (!createResult.Equals(Result.createSuccessfull.ToString()))
                                {
                                    return Ok(new BaseResponse(true, "Create Curriculum Subject Fail"));
                                }
                            }

                        }
                        else
                        if (i == 3)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            var Row = MiniExcel.QueryAsDataTable(filePath, sheetName: sheetNames[i], startCell: "A2", excelType: ExcelType.XLSX);
                            using (var package = new ExcelPackage(stream))
                            {
                                var worksheet = package.Workbook.Worksheets[3]; 

                                for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                                {
                                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                                    {
                                        var cell = worksheet.Cells[row, col];
                                        string cellValue = cell.Text;

                                        if (cellValue.Equals("ü"))
                                        {
                                            var subject_code = worksheet.Cells[row, 1].Text;
                                            var plo_name = worksheet.Cells[2, col].Text;

                                            var subject = _subjectRepository.GetSubjectByCode(subject_code);
                                            var plo = _ploRepository.GetPLOsByName(plo_name);

                                            var ploMapping = new PLOMapping()
                                            {
                                                PLO_id = plo.PLO_id,
                                                subject_id = subject.subject_id
                                            };
                                            string createResult = _ploMappingRepository.CreatePLOMapping(ploMapping);
                                            if (!createResult.Equals(Result.createSuccessfull.ToString()))
                                            {
                                                return Ok(new BaseResponse(true, "Create PLO Mapping Fail"));
                                            }
                                        }
                                    }
                                }
                            }


                        }
                    }


                    return Ok(new BaseResponse(false, "Success", curriculum_id));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(false, "Error: " + ex.InnerException, null));
            }
        }

        private string ValidationDataExcel(IEnumerable<CurriculumExcel> row)
        {
            string[] expectedOrder = {  "Curriculum Code", "Curriculum Name", "English Curriculum Name", "Curriculum Description", "Vocational Code", "Vocational Name", "English Vocational Name", "Decision No.", "Approved date", "Degree level", "Formality" };
            int index = 0;
            foreach (var r in row)
            {
               // CHECK VALIDATION IN SHEET CURRICULUM

                // Check information in coloumn title
                if (!r.Title.Equals(expectedOrder[index]))
                {
                    return "Can't change title in sheet curriculum";
                }
                index++;
                // Check format of curriculum code
                string pattern = @"^([A-Z]{2}-[A-Z]{2}-\d{2}.\d{1})$";
                if (r.Title.Equals("Curriculum Code"))
                {
                    if (!Regex.IsMatch(r.Details, pattern))
                    {
                        return "Curriculum Code must format ex:GD-GD-19.3 (GD: Graphic Design)";
                    }
                }
                // Check Major Exsit
                else if (r.Title.Equals("Vocational Code"))
                {
                   
                }

            }

            return "Success";
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
                }
                else if (r.Title.Equals("Vocational Name"))
                {
                    major.major_name = r.Details;
                }
                else if (r.Title.Equals("English Vocational Name"))
                {
                    major.major_english_name = r.Details;
                }
                else if (r.Title.Equals("Vocational Code"))
                {
                    // set major_code = value in coloum detail
                    major.major_code = r.Details;
                }
                else if (r.Title.Equals("Degree level"))
                {
                    curriculum.degree_level = r.Details;
                }
                else if (r.Title.Equals("Formality"))
                {
                    curriculum.Formality = r.Details;
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

        private SubjectExcel[] GetArraySubject(List<Subject> listSubject, List<Subject> listSubject1, List<Subject> listSubject2)
        {
            int maxIndex = Math.Max(listSubject.Count, Math.Max(listSubject1.Count, listSubject2.Count));
            var ArraySubject = new SubjectExcel[maxIndex];

            for (int i = 0; i < maxIndex; i++)
            {
                var subjectExcel = new SubjectExcel()
                {
                    No = i + 1,
                };

                if (i < listSubject.Count)
                {
                    subjectExcel.subject_code = listSubject[i].subject_code;
                    subjectExcel.subject_name = listSubject[i].subject_name;
                    subjectExcel.english_subject_name = listSubject[i].english_subject_name;
                    subjectExcel.credit = (listSubject[i].credit != 0) ? listSubject[i].credit.ToString() : "";
                    subjectExcel.total_time = (listSubject[i].total_time != 0) ? listSubject[i].total_time.ToString() : "";
                }

                if (i < listSubject1.Count)
                {
                    subjectExcel.subject_code_2 = listSubject1[i].subject_code;
                    subjectExcel.subject_name_2 = listSubject1[i].subject_name;
                    subjectExcel.english_subject_name_2 = listSubject1[i].english_subject_name;
                    subjectExcel.credit_2 = (listSubject1[i].credit != 0) ? listSubject1[i].credit.ToString() : "";
                    subjectExcel.total_time_2 = (listSubject1[i].total_time != 0) ? listSubject1[i].total_time.ToString() : "";
                }

                if (i < listSubject2.Count)
                {
                    subjectExcel.subject_code_3 = listSubject2[i].subject_code;
                    subjectExcel.subject_name_3 = listSubject2[i].subject_name;
                    subjectExcel.english_subject_name_3 = listSubject2[i].english_subject_name;
                    subjectExcel.credit_3 = (listSubject2[i].credit != 0) ? listSubject2[i].credit.ToString() : "";
                    subjectExcel.total_time_3 = (listSubject2[i].total_time != 0) ? listSubject2[i].total_time.ToString() : "";
                }

                ArraySubject[i] = subjectExcel;
            }

            return ArraySubject;
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
