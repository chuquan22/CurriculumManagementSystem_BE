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
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Repositories.CurriculumBatchs;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager, Dispatcher")]

    [ApiController]
    public class CurriculumsController : ControllerBase
    {
        private readonly CMSDbContext _context;
        private readonly HttpClient client = null;
        public static string API_PORT = "https://localhost:8080";
        public static string API_CURRICULUM = "/api/Curriculums/CreateCurriculum";
        public static string API_CURRICULUMSUBJECT = "/api/CurriculumSubjects/CreateCurriculumSubject";
        public static string API_PLO = "/api/PLOs";
        public static string API_PLOMAPPING = "/api/PLOMappings/UpdatePLOMapping";
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
        private readonly ICurriculumBatchRepository _curriculumBatchRepository = new CurriculumBatchRepository();

        public CurriculumsController(CMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            client = new HttpClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        // GET: api/Curriculums/GetCurriculumByBatch/code/5
        [HttpGet("GetCurriculumByBatch/{curriculumCode}/{batchId}")]
        public async Task<ActionResult<IEnumerable<CurriculumResponse>>> GetCurriculumByBatch(string curriculumCode, int batchId)
        {
            if (_context.Curriculum == null)
            {
                return NotFound();
            }
            var Curriculum = _curriculumRepository.GetCurriculum(curriculumCode);
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

            var curriculumRespone = _mapper.Map<List<CurriculumResponse>>(listCurriculum);


            foreach (var curriculum in curriculumRespone)
            {
                curriculum.batch_name = _batchRepository.GetBatchById(curriculum.start_batch_id).batch_name;
                curriculum.total_credit = _curriculumRepository.GetTotalCredit(curriculum.curriculum_id);
            }

            return Ok(new BaseResponse(false, "Get List Curriculum Sucessfully", new BaseListResponse(page, limit, totalElement, curriculumRespone)));

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
            //curriculumResponse.Semester = _context.Semester.Where(x => x.start_batch_id == curriculum.batch_id).Select(x => x.semester_name + " - " + x.school_year.ToString()).FirstOrDefault();
            return Ok(new BaseResponse(false, "Curriculum", curriculumResponse));
        }

        // GET: api/Curriculums/GetCurriculum/5
        [HttpGet("GetTotalSemester/{speId}/{batchId}")]
        public async Task<ActionResult<CurriculumResponse>> GetTotalSemester(int speId, int batchId)
        {
            var totalSemester = _curriculumRepository.GetTotalSemester(speId, batchId);
            return Ok(new BaseResponse(false, "total", totalSemester));
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
            curriculum.is_active = (curriculum.decision_No != null && curriculum.decision_No != "") ? true : false;

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

            curriculum.curriculum_code = _curriculumRepository.GetCurriculumCode(curriculum.start_batch_id, curriculum.specialization_id);
            curriculum.total_semester = _curriculumRepository.GetTotalSemester(curriculum.specialization_id, curriculum.start_batch_id);
            curriculum.is_active = (curriculum.decision_No != null && curriculum.decision_No != "") ? true : false;

            if (CheckCurriculumExists(curriculum.curriculum_code))
            {
                return BadRequest(new BaseResponse(true, $"Curriculum {curriculum.curriculum_code} is Duplicate!"));
            }
            string createResult = _curriculumRepository.CreateCurriculum(curriculum);
            if (!createResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }
            var curriResponse = _mapper.Map<CurriculumResponse>(curriculum);
            return Ok(new BaseResponse(false, "Create Curriculum Success!", curriResponse));
        }

        // DELETE: api/Curriculums/DeleteCurriculum/5
        [HttpDelete("DeleteCurriculum/{id}")]
        public async Task<IActionResult> DeleteCurriculum(int id)
        {
            var curriculum = _curriculumRepository.GetCurriculumById(id);
            if (curriculum == null)
            {
                return NotFound(new BaseResponse(true, "Not found this curriculum"));
            }

            if (CheckCurriculumCanDelete(id))
            {
                return BadRequest(new BaseResponse(true, $"Curriculum {curriculum.curriculum_code} Had Used. Can't Delete!"));
            }

            string deleteResult = _curriculumRepository.RemoveCurriculum(curriculum);
            if (!deleteResult.Equals("OK"))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }
            return Ok(new BaseResponse(false, "Delete Curriculum Successfull!"));
        }


        // Post: Export Curriculum by Excel File
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
                comboExcel += $"Combo{i}: " + comboList.Skip(i - 1).Select(x => x.combo_name).FirstOrDefault();
                if (i < comboList.Count)
                {
                    comboExcel += "; ";
                }
            }

            //if(curriculum.decision_No == null)
            //{
            //    return BadRequest(new BaseResponse(true, $"Export Fail.Curriculum not yet applied"));
            //}

            var subjectN1 = GetArraySubject(subject1, subject2, subject3);
            var subjectN2 = GetArraySubject(subject4, subject5, subject6);

            Dictionary<string, object> value = new Dictionary<string, object>()
            {
                ["decision_No"] = curriculum.decision_No == null ? "..." : curriculum.decision_No,
                ["approved_date"] = curriculum.approved_date == null ? "" : $"Ngày {curriculum.approved_date.Value.Day} tháng {curriculum.approved_date.Value.Month} năm {curriculum.approved_date.Value.Year} ",
                ["major_name"] = major.major_name,
                ["major_english_name"] = major.major_english_name,
                ["major_code"] = major.major_code,
                ["specialization_name"] = specialization.specialization_name,
                ["specialization_english_name"] = specialization.specialization_english_name,

                ["curriculum_code"] = curriculum.curriculum_code,
                ["degree_level"] = major.DegreeLevel.degree_level_english_name,
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
            return Ok(new BaseResponse(false, $"Curriculum-{curriculum.curriculum_code}-{curriculum.english_curriculum_name}", fileContents));
            // return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Curriculum.xlsx");
        }


        // Post: Import Curriculum by Excel File
        [HttpPost("ImportCurriculum")]
        public async Task<IActionResult> ImportCurriculum(IFormFile fileCurriculum)
        {
            try
            {
                List<CurriculumSubjectRequest> listCurriSubject = new List<CurriculumSubjectRequest>();

                var filePath = Path.GetTempFileName();
                var curriculum_id = 0;
                var curriculum = new Curriculum();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileCurriculum.CopyToAsync(stream);
                    //Get SheetName
                    var sheetNames = MiniExcel.GetSheetNames(filePath);
                    string validation = ValidationDataExcel(filePath, sheetNames, stream);
                    if (validation != "Success")
                    {
                        return BadRequest(new BaseResponse(true, validation));
                    }
                    for (int i = 0; i < sheetNames.Count; i++)
                    {

                        if (i == 0)
                        {
                            var row = MiniExcel.Query<CurriculumExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            var curriculumExcel = GetCurriculumInExcel(row);
                            try
                            {
                                curriculum_id = await CreateCurriculumsAPI(curriculumExcel);
                                curriculum = _curriculumRepository.GetCurriculumById(curriculum_id);
                            }
                            catch (Exception ex)
                            {
                                return BadRequest(new BaseResponse(true, "Import Fail. Please Check Sheet Curriculum!"));
                            }



                        }
                        else if (i == 1)
                        {
                            var row = MiniExcel.Query<PLOExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);

                            foreach (var item in row)
                            {
                                var plo = new PLOsDTORequest();
                                plo.curriculum_id = curriculum_id;
                                plo.PLO_name = item.PLO_name;
                                plo.PLO_description = item.PLO_description;

                                if (!_ploRepository.CheckPLONameExsit(plo.PLO_name, plo.curriculum_id))
                                {
                                    try
                                    {
                                        await CreatePLOsAPI(plo);
                                    }
                                    catch (Exception ex)
                                    {
                                        _curriculumRepository.RemoveCurriculum(curriculum);
                                        return BadRequest(new BaseResponse(true, "Import Fail. Please Check Sheet PLO!"));
                                    }
                                }

                            }
                        }
                        else if (i == 2)
                        {

                            var row = MiniExcel.Query<CurriculumSubjectExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            foreach (var item in row)
                            {
                                var curriculumSubject = new CurriculumSubjectRequest();
                                curriculumSubject.curriculum_id = curriculum_id;
                                if (item.subject_code != null && item.subject_code != "")
                                {
                                    var subject = _subjectRepository.GetSubjectByCode(item.subject_code);

                                    curriculumSubject.subject_id = subject.subject_id;
                                    curriculumSubject.term_no = item.term_no;
                                    item.combo_code = (item.combo_code == null || item.combo_code.Equals("")) ? null : item.combo_code;
                                    if (item.combo_code != null)
                                    {
                                        var combo = _comboRepository.FindComboByCode(item.combo_code);
                                        curriculumSubject.combo_id = combo.combo_id;
                                    }
                                    curriculumSubject.option = (item.option == null || item.option.Equals("") || item.option.Equals("False")) ? false : true;

                                    listCurriSubject.Add(curriculumSubject);
                                }
                            }

                        }
                        else
                        if (i == 3)
                        {
                            var group_name = "";
                            var groupNames = new List<string>
                                {
                                     "General Subject", "Basic Subject", "Specialization Subject", "Elective Subject"
                                };
                            using (var package = new ExcelPackage(stream))
                            {
                                var worksheet = package.Workbook.Worksheets[3];

                                for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                                {
                                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                                    {
                                        var cell = worksheet.Cells[row, col];
                                        string cellValue = cell.Text;
                                        if (groupNames.FirstOrDefault(x => x.ToUpper().Equals(cellValue.ToUpper().Trim())) != null)
                                        {
                                            group_name = cellValue;
                                        }

                                        var subjectCode = worksheet.Cells[row, 1].Text;
                                        foreach (var subject in listCurriSubject)
                                        {
                                            var subjects = _subjectRepository.GetSubjectByCode(subjectCode);
                                            if (subjects != null && subject.subject_id == subjects.subject_id)
                                            {
                                                subject.subject_group = group_name;
                                                break;
                                            }
                                        }

                                        if (cellValue.Trim().Equals("ü"))
                                        {
                                            var subject_code = worksheet.Cells[row, 1].Text;
                                            var plo_name = worksheet.Cells[2, col].Text;

                                            var subject = _subjectRepository.GetSubjectByCode(subject_code);
                                            var plo = _ploRepository.GetPLOsByName(plo_name, curriculum_id);

                                            var ploMapping = new PLOMapping()
                                            {
                                                PLO_id = plo.PLO_id,
                                                subject_id = subject.subject_id
                                            };
                                            string createResult = _ploMappingRepository.CreatePLOMapping(ploMapping);
                                            if (!createResult.Equals(Result.createSuccessfull.ToString()))
                                            {
                                                _curriculumRepository.RemoveCurriculum(curriculum);
                                                return BadRequest(new BaseResponse(true, "Import Fail. Please Check Sheet PLO Mapping!"));
                                            }
                                        }
                                    }
                                }
                            }
                            try
                            {
                                await CreateCurriculumsAPI(listCurriSubject);
                            }
                            catch (Exception ex)
                            {
                                _curriculumRepository.RemoveCurriculum(curriculum);
                                return BadRequest(new BaseResponse(true, "Import Fail. Please Check Sheet Curriculum Subject and PLO Mapping!"));
                            }



                        }
                    }
                    return Ok(new BaseResponse(false, "Import Success", curriculum_id));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(false, "Error: " + ex.InnerException.Message));
            }
        }

        private async Task<int> CreateCurriculumsAPI(CurriculumRequest cu)
        {
            string apiUrl = API_PORT + API_CURRICULUM;
            var jsonData = JsonSerializer.Serialize(cu);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            string[] token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ');
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token[1]);

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            response.EnsureSuccessStatusCode();
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var responseObject = JsonSerializer.Deserialize<JsonDocument>(strData, options).RootElement;

            int syllabusId = responseObject.GetProperty("data").GetProperty("curriculum_id").GetInt32();

            return syllabusId;
        }

        private async Task<PLOsDTORequest> CreatePLOsAPI(PLOsDTORequest plo)
        {
            string apiUrl = API_PORT + API_PLO;
            var jsonData = JsonSerializer.Serialize(plo);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            string[] token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ');
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token[1]);

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            response.EnsureSuccessStatusCode();
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            PLOsDTORequest rs = JsonSerializer.Deserialize<PLOsDTORequest>(strData, options);
            return rs;
        }

        private async Task<CurriculumSubjectRequest> CreateCurriculumsAPI(List<CurriculumSubjectRequest> curri)
        {
            string apiUrl = API_PORT + API_CURRICULUMSUBJECT;
            var jsonData = JsonSerializer.Serialize(curri);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            string[] token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ');
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token[1]);

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            response.EnsureSuccessStatusCode();
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            CurriculumSubjectRequest rs = JsonSerializer.Deserialize<CurriculumSubjectRequest>(strData, options);
            return rs;
        }


        private string ValidationDataExcel(string path, List<string> sheetNames, FileStream stream)
        {

            List<Subject> listSubject = new List<Subject>();
            List<PLOs> listPLO = new List<PLOs>();
            if (!sheetNames[0].Equals("Curriculum") || !sheetNames[1].Equals("PLO") || !sheetNames[2].Equals("Curriculum Subject") || !sheetNames[3].Equals("PLO Mappings"))
            {
                return "File is not in a supported format.";
            }
            for (int i = 0; i < sheetNames.Count; i++)
            {
                // validate data sheet 1
                if (i == 0)
                {
                    var major = new Major();
                    var row = MiniExcel.Query<CurriculumExcel>(path, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                    string batch_name = "";
                    bool checkApproved = false;
                    string decicionNo = null;
                    string dateTime = null;
                    foreach (var r in row)
                    {
                        // Check format of curriculum code
                        string pattern = @"^[A-Z]+-[A-Z]+-[A-Z]+-\d+(\.\d+)?$";
                        if (r.Title == null) { }
                        else if (r.Title.Equals("Curriculum Code"))
                        {
                            if (!Regex.IsMatch(r.Details, pattern))
                            {
                                return "Curriculum Code must format ex:GD-GD-CD-19.3";
                            }

                            if (CheckCurriculumExists(r.Details))
                            {
                                return $"Curriculum {r.Details} is Duplicate!";
                            }
                            string[] parts = r.Details.Split('-');
                            // get part have index 2 in array string ex: 19.4
                            batch_name = parts[3];
                            // get batch_id by batch_name
                            if (_batchRepository.GetBatchIDByName(batch_name) == 0)
                            {
                                return $"Batch {batch_name} Not Exsit";
                            }

                        }
                        // Check Major Exsit
                        else if (r.Title.Equals("Vocational Code"))
                        {
                            major = _majorRepository.CheckMajorbyMajorCode(r.Details);
                            //if major not exsit in database
                            if (major == null)
                            {
                                return $"Major not Exsit. Please Create Major {r.Details}";
                            }
                        }
                        // Check Spe exsit
                        else if (r.Title.Equals("Specialization Code"))
                        {
                            var spe_id = _specializationRepository.GetSpecializationIdByCode(r.Details);
                            var spe = _specializationRepository.GetSpeById(spe_id);

                            //if major not exsit in database
                            if (spe_id == 0)
                            {
                                return $"Specialization not Exsit. Please Create Specialization {r.Details}";
                            }

                            if (spe.major_id != major.major_id)
                            {
                                return $"Specialization {spe.specialization_code} not exsit in Major {major.major_code}";
                            }

                            if (double.Parse(spe.Semester.Batch.batch_name) >= double.Parse(batch_name))
                            {
                                return $"Curriculum must start batch greater than {spe.Semester.Batch.batch_name}";
                            }

                        }
                        else if (r.Title.Equals("Decision No."))
                        {
                            decicionNo = r.Details;
                        }
                        else if (r.Title.Equals("Approved date"))
                        {
                            dateTime = r.Details;
                            string[] date = r.Details.Split(' ');
                            if (DateTime.ParseExact(date[0], "dd/MM/yyyy", CultureInfo.InvariantCulture) == null)
                            {
                                return $"Check approved date {date[0]}";
                            }

                        }

                    }
                    if (!((decicionNo == null && dateTime == null) || (decicionNo != null && dateTime != null)))
                    {
                        return $"Both the Decicion No and Approved Date fields must have a value or be blank";
                    }


                }

                // validate data sheet 2
                if (i == 1)
                {
                    // check PLO Exsit
                    var row = MiniExcel.Query<PLOExcel>(path, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                    foreach (var item in row)
                    {
                        if (item.No != null && item.PLO_name == null || item.PLO_description == null)
                        {
                            return "Data in colounm PLO name and PLO description in sheet PLO must not be null!";
                        }
                        var plo = new PLOs();
                        plo.PLO_name = item.PLO_name;

                        foreach (var PLO in listPLO)
                        {
                            if (plo.PLO_name.Equals(PLO.PLO_name))
                            {
                                return $"Only one {plo.PLO_name} in Sheet PLO";
                            }
                            if (!plo.PLO_name.StartsWith("PLO") || plo.PLO_name.Contains(" "))
                            {
                                return "PLO must start with 'PLO' and no cointain space ";
                            }
                        }
                        listPLO.Add(plo);
                    }
                }

                // validate data sheet 3
                if (i == 2)
                {
                    // Check Subject Exsit
                    var row = MiniExcel.Query<CurriculumSubjectExcel>(path, sheetName: sheetNames[i], excelType: ExcelType.XLSX).TakeWhile(row => row.subject_code != null || row.subject_name != null || row.english_subject_name != null);
                    foreach (var item in row)
                    {
                        if (item.subject_code == null || item.subject_name == null || item.english_subject_name == null || item.term_no == 0 || item.credit == 0)
                        {
                            return "Must be fill all data in sheet Curriculum Subject";
                        }
                        if (item.subject_code != null && item.subject_code != "")
                        {
                            var subject = _subjectRepository.GetSubjectByCode(item.subject_code);
                            if (subject == null)
                            {
                                return $"Subject {item.subject_code} Not Found. Please Create Subject";
                            }
                            listSubject.Add(new Subject { subject_code = item.subject_code });
                        }
                        string combo_code = item.combo_code;
                        if (combo_code != null && combo_code != "")
                        {
                            var combo = _comboRepository.FindComboByCode(item.combo_code);
                            if (combo == null)
                            {
                                return $"Combo {item.combo_code} Not Found. Please Create Combo";
                            }
                        }

                        int count = row.Count(data => data.option != null && data.option.Equals("True") && data.term_no == item.term_no);
                        if (count % 2 != 0)
                        {
                            return $"Subject have option in term no {item.term_no} must even number";
                        }

                    }


                }
                // validate data sheet 4
                if (i == 3)
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[3];

                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            for (int col = 2; col <= worksheet.Dimension.End.Column; col++)
                            {
                                var celplo = worksheet.Cells[2, col];
                                bool found_plo = false;
                                bool found_subject = false;
                                foreach (var plo in listPLO)
                                {
                                    if (plo.PLO_name.Equals(celplo.Text))
                                    {
                                        found_plo = true;

                                    }
                                }
                                if (!found_plo)
                                {
                                    return $"PLO {celplo.Text} in header table PLO Mapping not mapp in sheet PLO";
                                }

                                var subjectCode = worksheet.Cells[row, 1];
                                foreach (var subject in listSubject)
                                {
                                    if (subject.subject_code.Equals(subjectCode.Text))
                                    {
                                        found_subject = true;
                                    }

                                }
                                if (!found_subject && !subjectCode.Text.Contains(" "))
                                {
                                    return $"Subject Code {subjectCode.Text} not mapp in sheet Curriculum Subject";
                                }

                            }
                        }
                    }
                }

            }

            return "Success";

        }
        private CurriculumRequest GetCurriculumInExcel(IEnumerable<CurriculumExcel> row)
        {
            var curriculum = new CurriculumRequest();
            curriculum.total_semester = 7;
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
                    // Split string ex: GD-GD-CD-19.4
                    string[] parts = r.Details.Split('-');
                    // get part have index 2 in array string ex: 19.4
                   var batch_name = parts[3];
                    // get batch_id by batch_name
                    curriculum.batch_id = _batchRepository.GetBatchIDByName(batch_name);
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
                else if (r.Title.Equals("Approved date") && r.Details != null && r.Details != "")
                {
                    string[] date = r.Details.Split(' ');
                    curriculum.approved_date = DateTime.ParseExact(date[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else if (r.Title.Equals("Formality"))
                {
                    curriculum.Formality = r.Details;
                }
                else if (r.Title.Equals("Specialization Code"))
                {
                    curriculum.specialization_id = _specializationRepository.GetSpecializationIdByCode(r.Details);
                }
                
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

        private bool CheckCurriculumExists(string code)
        {
            return (_context.Curriculum?.Any(e => e.curriculum_code.Equals(code))).GetValueOrDefault();
        }

        private bool CheckCurriculumCanDelete(int id)
        {
            var haveSubject = _context.CurriculumSubject?.FirstOrDefault(e => e.curriculum_id == id);
            var haveBatch = _context.CurriculumBatch?.FirstOrDefault(e => e.curriculum_id == id);
            if (haveSubject == null && haveBatch == null)
            {
                return false;
            }
            return true;
        }


    }
}
