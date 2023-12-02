using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.Excel;

using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using Repositories.AssessmentMethods;
using Repositories.AssessmentTypes;
using Repositories.ClassSessionTypes;
using Repositories.CLOS;
using Repositories.DegreeLevels;
using Repositories.GradingStruture;
using Repositories.LearningResources;
using Repositories.Materials;
using Repositories.Session;
using Repositories.Subjects;
using Repositories.Syllabus;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager, Dispatcher")]
    [ApiController]
    public class SyllabusController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly HttpClient client = null;
        public static string API_PORT = "https://cmsfpoly-be.azurewebsites.net";
        public static string API_SYLLABUS = "/api/Syllabus";    
        public static string API_MATERIALS = "/api/Materials";
        public static string API_GRADING_STRUTURE = "/api/GradingStruture";
        public static string API_CLO = "/api/CLOs";
        public static string API_SCHEDULE = "/api/Session";
        private ISyllabusRepository syllabusRepository;
        private ISubjectRepository subjectRepository;
        private IAssessmentTypeRepository assessmentTypeRepository;
        private ICLORepository cloRepository;
        private IAssessmentMethodRepository assessmentMethodRepository;
        private IMaterialRepository materialsRepository;
        private IGradingStrutureRepository gradingStrutureRepository;
        private ISessionRepository sessionRepository;
        private IClassSessionTypeRepository classSessionTypeRepository;
        private IDegreeLevelRepository degreeLevelRepository;
        private ILearningResourceRepository learningResourceRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private static int syllaId = 0;
        public SyllabusController(IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            syllabusRepository = new SyllabusRepository();
            subjectRepository = new SubjectRepository();
            assessmentTypeRepository = new AssessmentTypeRepository();
            cloRepository = new CLORepository();
            assessmentMethodRepository = new AssessmentMethodRepository();
            materialsRepository = new MaterialRepository();
            gradingStrutureRepository = new GradingStrutureRepository();
            sessionRepository = new SessionRepository();
            classSessionTypeRepository = new ClassSessionTypeRepository();
            degreeLevelRepository = new DegreeLevelRepository();
            learningResourceRepository = new LearningResourceRepository();
            client = new HttpClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SyllabusResponse>>> GetListSyllabus(int page, int limit, string? txtSearch, string? subjectCode)
        {
            List<Syllabus> rs = new List<Syllabus>();
            try
            {
                int limit2 = syllabusRepository.GetTotalSyllabus(txtSearch, subjectCode);
                List<Syllabus> list = syllabusRepository.GetListSyllabus(page, limit, txtSearch, subjectCode);
                var result = _mapper.Map<List<SyllabusResponse>>(list);
                return Ok(new BaseResponse(false, "Get List Syllabus Sucessfully!", new BaseListResponse(page, limit2, result)));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSyllabus(SyllabusRequest request)
        {

            try
            {
                Syllabus rs = _mapper.Map<Syllabus>(request);
                if(rs.min_GPA_to_pass == null)
                {
                    rs.min_GPA_to_pass = 5;
                }
                if(rs.scoring_scale == null)
                {
                    rs.scoring_scale = 10;
                }
                var result = syllabusRepository.CreateSyllabus(rs);
                return Ok(new BaseResponse(false, "Create Syllabus Successfully!", rs));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }
        }

        [HttpGet("GetSyllabusDetails")]
        public ActionResult SyllabusDetails(int syllabus_id)
        {
            try
            {
                Syllabus rs1 = syllabusRepository.GetSyllabusById(syllabus_id);
                var result = _mapper.Map<SyllabusDetailsResponse>(rs1);
                List<PreRequisite> pre = syllabusRepository.GetPre(rs1.subject_id);
                result.pre_required = _mapper.Map<List<PreRequisiteResponse2>>(pre);
                return Ok(new BaseResponse(false, "Get Syllabus Details Successfully!", result));

            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }

        }
        [HttpPost("ImportSyllabus")]
        public async Task<ActionResult> ImportSyllabus(IFormFile file)
        {
            try
            {
                var filePath = Path.GetTempFileName();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    int syllabusId = 0;
                    //Get SheetName
                    var sheetNames = MiniExcel.GetSheetNames(filePath);
                    List<object> rs = new List<object>();
                    List<CLO> listClo = new List<CLO>();
                    Syllabus syllabusExcel = new Syllabus();
                    GradingStrutureCreateRequest gradingStrutureCreate;
                    List<int> cloId = new List<int>();

                    for (int i = 0; i < sheetNames.Count; i++)
                    {
                        gradingStrutureCreate = new GradingStrutureCreateRequest();
                        if (i == 0)
                        {
                            var row = MiniExcel.Query<SyllabusExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            syllabusExcel = GetSyllabusExel(row);
                            var value = new
                            {
                                Syllabus = syllabusExcel,
                            };
                            try
                            {
                                syllabusId = await CreateSyllabusAPI(syllabusExcel);
                                syllaId = syllabusId;

                            }
                            catch (Exception)
                            {

                                return BadRequest("Import false at sheet Syllabus!");
                            }
                            rs.Add(value);
                        }
                        else if (i == 1)
                        {
                            var row = MiniExcel.Query<MaterialExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            var materialExcel = GetMaterialExcel(row, syllabusExcel);
                            var value = new
                            {
                                Materials = materialExcel,
                            };

                            try
                            {
                                foreach (var item in materialExcel)
                                {
                                    if (item.material_type != null)
                                    {
                                        item.syllabus_id = syllabusId;
                                        MaterialRequest addRs = _mapper.Map<MaterialRequest>(item);
                                        await CreateMaterialsAPI(addRs);
                                    }

                                }
                            }
                            catch (Exception)
                            {
                                materialsRepository.DeleteMaterialBySyllabusId(syllabusId);
                                syllabusRepository.DeleteSyllabus(syllabusId);
                                return BadRequest("Import false at sheet Materials.");

                            }                         
                            rs.Add(value);

                        }
                        else if (i == 2)
                        {
                            var row = MiniExcel.Query<CLOsExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            listClo = GetClosExcel(row);
                            var value = new
                            {
                                CLOs = listClo,
                            };
                            try
                            {
                                foreach (var item in listClo)
                                {
                                    CLOsRequest addRs = _mapper.Map<CLOsRequest>(item);
                                    if (item.CLO_name != null)
                                    {

                                        addRs.syllabus_id = syllabusId;
                                        int idClo = 0;

                                        idClo = await CreateCLOsAPI(addRs);
                                        if (idClo == 0)
                                        {
                                            throw new Exception("Import false at sheet CLOs.");
                                        }
                                        cloId.Add(idClo);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                cloRepository.DeleteCLOsBySyllabusId(syllabusId);
                                materialsRepository.DeleteMaterialBySyllabusId(syllabusId);
                                syllabusRepository.DeleteSyllabus(syllabusId);
                                throw new Exception("Import false at sheet CLOs");
                            }
                            
                            gradingStrutureCreate.gradingCLORequest = new GradingCLORequest();
                            gradingStrutureCreate.gradingCLORequest.CLO_id = cloId;
                            rs.Add(value);
                        }
                        else if (i == 3)
                        {
                            var row = MiniExcel.Query<ScheduleExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            var scheduleExcel = GetScheduleExcel(row, syllabusExcel);
                            var value = new
                            {
                                GradingStruture = row,
                            };
                            foreach (var item in scheduleExcel)
                            {
                                SessionCreateRequest dataSession = new SessionCreateRequest();
                                dataSession.session = _mapper.Map<SessionRequest>(item);

                                // Initialize the session_clo list
                                dataSession.session_clo = new List<SessionCLOsRequest>();



                                List<int> lst = new List<int>();
                                foreach (var it in cloId)
                                {
                                  
                                        string name = "null";
                                        if (cloRepository.GetCLOsById(it) != null)
                                        {
                                            name = cloRepository.GetCLOsById(it).CLO_name;
                                            if (item.CLO_name.ToLower().Trim().Contains(name.ToLower().Trim()))
                                            {
                                                lst.Add(it);
                                            }
                                        }

                                        if (item.CLO_name.ToLower().Trim().Contains("All CLOs".ToLower().Trim()))
                                        {
                                            lst = new List<int>();
                                            lst.AddRange(cloId);
                                        }                                                                 
                                }
                                foreach (var idClo in lst)
                                {
                                    dataSession.session_clo.Add(new SessionCLOsRequest { CLO_id = idClo });
                                }
                                dataSession.session.syllabus_id = syllabusId;
                                try
                                {
                                    await CreateSchudeleAPI(dataSession);

                                }
                                catch (Exception ex)
                                {
                                    sessionRepository.DeleteSessionBySyllabusId(syllabusId);
                                    cloRepository.DeleteCLOsBySyllabusId(syllabusId);
                                    materialsRepository.DeleteMaterialBySyllabusId(syllabusId);
                                    syllabusRepository.DeleteSyllabus(syllabusId);

                                    return BadRequest("Import false at sheet Schedule.");
                                }
                            }

                            rs.Add(value);
                        }
                        else if (i == 4)
                        {
                            var row = MiniExcel.Query<GradingStrutureExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            List<GradingStruture> gradingStrutureExcel = GetGradingStrutureExcel(row, syllabusExcel);
                            gradingStrutureCreate.gradingStruture = new GradingStrutureRequest();
                            var list = _mapper.Map<List<GradingStrutureRequest>>(gradingStrutureExcel);
                            decimal? weightAll = 0;
                            try
                            {


                                foreach (var gra in list)
                                {
                                    gradingStrutureCreate.gradingStruture = gra;
                                    gradingStrutureCreate.gradingStruture.syllabus_id = syllabusId;
                                    gradingStrutureCreate.gradingCLORequest = new GradingCLORequest();
                                    List<int> lst = new List<int>();

                                    foreach (var cl in cloId)
                                    {
                                        string name = "null";
                                        if (cloRepository.GetCLOsById(cl) != null)
                                        {
                                            name = cloRepository.GetCLOsById(cl).CLO_name;
                                            if (gra.clo_name != null)
                                            {

                                                if (gra.clo_name.Contains(name))
                                                {
                                                    lst.Add(cl);
                                                }
                                            }
                                        }
                                        if(gra.clo_name != null)
                                        {

                                            if (gra.clo_name.Contains("All CLOs"))
                                            {
                                                lst = new List<int>();
                                                lst.AddRange(cloId);
                                            }
                                        }

                                    }
                                    if (gra.session_no == null)
                                    {
                                        weightAll += gra.grading_weight;
                                        if (weightAll > 100)
                                        {
                                            throw new Exception("Weight of grading over 100%");
                                        }
                                    }
                                    gradingStrutureCreate.gradingCLORequest.CLO_id = lst;
                                    await CreateGradingStrutureAPI(gradingStrutureCreate);


                                }
                            }
                            catch (Exception ex)
                            {
                                gradingStrutureRepository.DeleteGradingStrutureBySyllabusId(syllabusId);
                                sessionRepository.DeleteSessionBySyllabusId(syllabusId);
                                cloRepository.DeleteCLOsBySyllabusId(syllabusId);
                                materialsRepository.DeleteMaterialBySyllabusId(syllabusId);
                                syllabusRepository.DeleteSyllabus(syllabusId);
                                throw new Exception("Import false at sheet Grading Struture!.");
                            }

                        }
                    }
                    SetStatusSyllabus(syllabusId);
                    return Ok(new BaseResponse(false, "Import Sucessfully!", syllabusId));

                }
            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "Error: " + ex.Message, null));
            }

        }
        [HttpDelete("DeleteSyllabus/{syllabusId}")]
        public async Task<IActionResult> DeleteSyllabusById(int syllabusId)
        {
            var syllabus = syllabusRepository.GetSyllabusById(syllabusId);
            if(syllabus == null)
            {
                return BadRequest(new BaseResponse(true, "Syllabus not exist in system!", null));
            }
            try
            {

                gradingStrutureRepository.DeleteGradingStrutureBySyllabusId(syllabusId);
                sessionRepository.DeleteSessionBySyllabusId(syllabusId);
                cloRepository.DeleteCLOsBySyllabusId(syllabusId);
                materialsRepository.DeleteMaterialBySyllabusId(syllabusId);
                syllabusRepository.DeleteSyllabus(syllabusId);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true,"Error: " + ex.Message,null));
            }
            return Ok(new BaseResponse(false, "Delete Sucessfully!", null));
        }

        private async Task<int> CreateSyllabusAPI(Syllabus sy)
        {
            string apiUrl = API_PORT + API_SYLLABUS;
            var jsonData = JsonSerializer.Serialize(sy);
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

            int syllabusId = responseObject.GetProperty("data").GetProperty("syllabus_id").GetInt32();

            return syllabusId;
        }
        private async Task<MaterialRequest> CreateMaterialsAPI(MaterialRequest me)
        {
            string apiUrl = API_PORT + API_MATERIALS;
            var jsonData = JsonSerializer.Serialize(me);
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

            MaterialRequest rs = JsonSerializer.Deserialize<MaterialRequest>(strData, options);
            return rs;
        }
        private async Task<GradingStrutureCreateRequest> CreateGradingStrutureAPI(GradingStrutureCreateRequest me)
        {
            string apiUrl = API_PORT + API_GRADING_STRUTURE;
            var jsonData = JsonSerializer.Serialize(me);
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

            GradingStrutureCreateRequest rs = JsonSerializer.Deserialize<GradingStrutureCreateRequest>(strData, options);
            return rs;
        }
        private async Task<int> CreateCLOsAPI(CLOsRequest sy)
        {
            string apiUrl = API_PORT + API_CLO;
            var jsonData = JsonSerializer.Serialize(sy);
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

            int syllabusId = responseObject.GetProperty("data").GetProperty("clO_id").GetInt32();

            return syllabusId;
        }
        private async Task<int> CreateSchudeleAPI(SessionCreateRequest sy)
        {
            string apiUrl = API_PORT + API_SCHEDULE;
            var jsonData = JsonSerializer.Serialize(sy);
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

            int syllabusId = responseObject.GetProperty("data").GetProperty("schedule_id").GetInt32();

            return syllabusId;
        }
        private List<GradingStruture> GetGradingStrutureExcel(IEnumerable<GradingStrutureExcel> row, Syllabus syllabus)
        {
            List<GradingStruture> result = new List<GradingStruture>();
            foreach (var r in row)
            {
                if (r == null) { }
                if(r.SessionNo == null) { r.SessionNo = ""; }
                
                GradingStruture g = new GradingStruture();
                g.type_of_questions = r.type_of_questions;
                g.number_of_questions = r.number_of_questions;
                g.session_no = ((r.SessionNo.Trim() == null || r.SessionNo.Trim().Equals("")) ? null : int.Parse(r.SessionNo));
                g.references = r.Reference;
                g.grading_weight = r.weight;              
                g.grading_part = r.Part;
                g.syllabus_id = syllabus.syllabus_id;
                g.minimum_value_to_meet_completion = r.minimun_value_to_meet;
                g.grading_duration = r.Duration;
                g.scope_knowledge = r.scope;
                g.how_granding_structure = r.how;
                g.grading_note = r.Note;
                g.assessment_component = r.assessment_component;
                g.assessment_type = r.assessment_type;
               
                g.clo_name = r.CLO;
                result.Add(g);

            }
            return result;
        }

        private List<Session> GetScheduleExcel(IEnumerable<ScheduleExcel> row, Syllabus syllabus)
        {
            List<Session> rs = new List<Session>();
            foreach (var r in row)
            {
                if (r == null) { }
                Session se = new Session();
                se.session_No = r.session_No;
                se.schedule_content = r.Content;
                se.syllabus_id = syllabus.syllabus_id;
                se.ITU = r.ITU;
                se.Syllabus = syllabus;
                se.lecturer_material = r.lecture_materials;
                se.student_material = r.student_materials;
                se.schedule_lecturer_task = r.lecture_task;
                se.schedule_student_task = r.student_task;
                se.lecturer_material_link = r.lecturer_material_link;
                se.student_material_link = r.student_material_link;
                se.CLO_name = r.CLO;
                try
                {

                    se.class_session_type_id = GetClassSessionTypeByName(r.leaning_teaching_method).class_session_type_id;

                }
                catch (Exception ex)
                {
                    sessionRepository.DeleteSessionBySyllabusId(syllaId);
                    cloRepository.DeleteCLOsBySyllabusId(syllaId);
                    materialsRepository.DeleteMaterialBySyllabusId(syllaId);
                    syllabusRepository.DeleteSyllabus(syllaId);
                    throw new Exception("Import syllabus fail at sheet Schedule!");
                }
                rs.Add(se);
            }
            return rs;
        }
        private ClassSessionType GetClassSessionTypeByName(string name)
        {
            return classSessionTypeRepository.GetClassSessionTypeByName(name);
        }
        private List<CLO> GetClosExcel(IEnumerable<CLOsExcel> row)
        {
            List<CLO> result = new List<CLO>();
            foreach (var r in row)
            {
                if (r.CLO_Name == null) { }
                CLO c = new CLO();
                c.CLO_id = 1;
                c.CLO_name = r.CLO_Name;
                c.CLO_description = r.CLO_Description;
                result.Add(c);
            }
            return result;
        }

        private CLO GetCloIdByName(string cLO_Name)
        {
            return cloRepository.GetCLOByName(cLO_Name);
        }

        private List<Material> GetMaterialExcel(IEnumerable<MaterialExcel> row, Syllabus syllabus)
        {
            List<Material> materials = new List<Material>();
            foreach (var r in row)
            {
                if (r.MaterialDescription == null) { }
                Material m = new Material();
                m.material_description = r.MaterialDescription;
                m.material_purpose = r.Purpose;
                m.material_ISBN = r.ISBN;
                m.material_type = r.Type;
                m.syllabus_id = syllabus.syllabus_id;
                m.material_note = r.Note;
                m.material_author = r.Author;
                m.material_publisher = r.Publisher;
                if (!string.IsNullOrEmpty(r.Published_Date))
                {
                    if (int.TryParse(r.Published_Date, out int year))
                    {
                        m.material_published_date = new DateTime(year, 1, 1);
                    }                 
                }
                try
                {
                    m.learning_resource_id = learningResourceRepository.GetLearningResourceByName(r.LearningResource).learning_resource_id;

                }
                catch (Exception)
                {
                    materialsRepository.DeleteMaterialBySyllabusId(syllaId);
                    syllabusRepository.DeleteSyllabus(syllaId);
                    throw new Exception("Import syllabus fail at sheet Material! No Learning Resource Found");

                }
                m.material_edition = r.Edition;
                materials.Add(m);

            }
            return materials;
        }

        private Syllabus GetSyllabusExel(IEnumerable<SyllabusExcel> row)
        {
            var syllabus = new Syllabus();
            syllabus.Subject = new Subject();
            try
            {
                foreach (var r in row)
                {
                    if (r.Title == null) { }
                    else if (r.Title.Equals("Document type"))
                    {
                        syllabus.document_type = r.Details;
                    }
                    else if (r.Title.Equals("Program"))
                    {
                        syllabus.program = r.Details;
                    }
                    else if (r.Title.Equals("Decision No."))
                    {
                        syllabus.decision_No = r.Details;
                    }
                    else if (r.Title.Equals("Course Code"))
                    {
                        var subject = GetSubjectByCode(r.Details);
                        if (subject != null)
                        {
                            syllabus.subject_id = subject.subject_id;

                        }
                        else
                        {
                            throw new Exception("Import syllabus fail at sheet Syllabus! Wrong Cource Code!");
                        }
                    }
                    else if (r.Title.Equals("Leaning-Teaching Method"))
                    {


                    }
                    else if (r.Title.Equals("No of credits"))
                    {
                        syllabus.Subject.credit = int.Parse(r.Details);
                    }
                    else if (r.Title.Equals("Degree Level"))
                    {
                        try
                        {
                            syllabus.degree_level_id = degreeLevelRepository.GetDegreeLevelByName(r.Details).degree_level_id;

                        }
                        catch (Exception ex)
                        {

                            throw new Exception("Import syllabus fail at sheet Syllabus! Wrong Degree Level");
                        }
                    }
                    else if (r.Title.Equals("Time Allocation"))
                    {
                        syllabus.time_allocation = r.Details;
                    }
                    else if (r.Title.Equals("Pre-requisite"))
                    {

                    }
                    else if (r.Title.Equals("Description"))
                    {
                        syllabus.syllabus_description = r.Details;
                    }
                    else if (r.Title.Equals("Student's task"))
                    {
                        syllabus.student_task = r.Details;
                    }
                    else if (r.Title.Equals("Tools"))
                    {
                        syllabus.syllabus_tool = r.Details;
                    }
                    else if (r.Title.Equals("Note"))
                    {
                        syllabus.syllabus_note = r.Details;
                    }
                    else if (r.Title.Equals("Min GPA to pass"))
                    {//
                        syllabus.min_GPA_to_pass = int.Parse(r.Details);
                    }
                    else if (r.Title.Equals("Scoring scale"))
                    {
                        if (!string.IsNullOrEmpty(r.Details))
                        {
                            syllabus.scoring_scale = int.Parse(r.Details);

                        }
                    }
                    else if (r.Title.Equals("Approved date"))
                    {
                        string[] date = r.Details.Split(' ');
                        syllabus.approved_date = DateTime.ParseExact(date[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }



                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return syllabus;
        }

        private Subject GetSubjectByCode(string name)
        {
            return subjectRepository.GetSubjectByCode(name);
        }

        [HttpPatch]
        public ActionResult UpdatePatchSyllabus(SyllabusPatchRequest request)
        {
            try
            {
                Syllabus rs = _mapper.Map<Syllabus>(request);
                string result = syllabusRepository.UpdatePatchSyllabus(rs);
                return Ok(new BaseResponse(false, "Sucessfully", result));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        // Post: Export Curriculum by Excel File
        [HttpPost("ExportSyllabus/{syllabus_id}")]
        public async Task<IActionResult> ExportSyllabus(int syllabus_id)
        {
            string wwwrootPath = _hostingEnvironment.WebRootPath;

            string filePath = System.IO.Path.Combine(wwwrootPath, "SyllabusExcel.xlsx");
            var syllabus = syllabusRepository.GetSyllabusById(syllabus_id);
            var materials1 = materialsRepository.GetMaterial(syllabus_id);
            var clos1 = cloRepository.GetCLOs(syllabus_id);
            var schedule1 = sessionRepository.GetSession(syllabus_id);
            var gradingStruture1 = gradingStrutureRepository.GetGradingStruture(syllabus_id);
            var gradingStruture = _mapper.Map<List<GradingStrutureExportExcel>>(gradingStruture1);
            var materials = _mapper.Map<List<MaterialExportExcel>>(materials1);
            var clos = _mapper.Map <List<CLOsExportExcel>>(clos1);
            var schedule = _mapper.Map<List<SessionExcelExport>>(schedule1);
           

            for (int i = 0; i < gradingStruture.Count; i++)
            {
                gradingStruture[i].no = i + 1;
            }
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].no = i + 1;
            }
            for (int i = 0; i < clos.Count; i++)
            {
                clos[i].no = i + 1;
            }
            for (int i = 0; i < schedule.Count; i++)
            {
                schedule[i].no = i + 1;
            }

            List<PreRequisite> pre = syllabusRepository.GetPre(syllabus.Subject.subject_id);
            var pre_required = _mapper.Map<List<PreRequisiteResponse2>>(pre);
            string pre_requiredText = "";
            foreach (var item in pre_required)
            {
                pre_requiredText = pre_requiredText + item.prequisite_name + ": " + item.prequisite_subject_name + "\n";
            }
            DateTime approvedDate = (DateTime)syllabus.approved_date;

            string formattedDate = approvedDate.ToString("dd/MM/yyyy");

            Dictionary<string, object> value = new Dictionary<string, object>()
            {
                //Tab Syllabus
                ["document_type"] = syllabus.document_type,
                ["program"] = syllabus.program,
                ["decision_no"] = syllabus.decision_No,
                ["course_name"] = syllabus.Subject.subject_name,
                ["course_name_english"] = syllabus.Subject.english_subject_name,
                ["course_code"] = syllabus.Subject.subject_code,
                ["leaning-teaching_method"] = null,
                ["credit"] = null,
                ["degree_level"] = syllabus.DegreeLevel.degree_level_english_name,
                ["time_allocation"] = syllabus.time_allocation,
                ["description"] = syllabus.syllabus_description,
                ["student_task"] = syllabus.student_task,
                ["tools"] = syllabus.syllabus_tool,
                ["note"] = syllabus.syllabus_note,
                ["min_gpa_to_pass"] = syllabus.min_GPA_to_pass,
                ["scoring_scale"] = syllabus.scoring_scale,
                ["approved_date"] = formattedDate,
                ["pre_requiredText"] = pre_requiredText,
                //Tab Materials
                ["materials"] = materials,
                //Tab CLO
                ["CLOs"] = clos,
                //Tab Schedule
                ["schedule"] = schedule,
                //Tab GradingStruture
                ["gradingStruture"] = gradingStruture
            };


            MiniExcel.SaveAsByTemplate("exported.xlsx", filePath, value);

            byte[] fileContents = System.IO.File.ReadAllBytes("exported.xlsx");
            return Ok(fileContents);

            //return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "exported.xlsx");
        }

        [HttpPost("SetStatus")]
        public ActionResult SetStatusSyllabus(int id)
        {
            try
            {
               
                var result = syllabusRepository.SetStatusSyllabus(id);
                return Ok(new BaseResponse(false, "Sucessfully", result));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
        [HttpPost("SetApproved")]
        public ActionResult SetApproved(int id)
        {
            try
            {
               
                var result = syllabusRepository.SetApproved(id);
                return Ok(new BaseResponse(false, "Sucessfully", result));
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new BaseResponse(true, "False", null));
        }
    }
}
