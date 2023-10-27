using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.Excel;

using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using Repositories.AssessmentMethods;
using Repositories.AssessmentTypes;
using Repositories.CLOS;
using Repositories.GradingStruture;
using Repositories.Materials;
using Repositories.Session;
using Repositories.Subjects;
using Repositories.Syllabus;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISyllabusRepository repo;
        private ISubjectRepository repo2;
        private IAssessmentTypeRepository repo3;
        private ICLORepository repo4;
        private IAssessmentMethodRepository repo5;
        private IMaterialRepository repo6;
        private IGradingStrutureRepository repo7;


        private readonly HttpClient client = null;

        public static string API_PORT = "https://localhost:8080";
        public static string API_SYLLABUS = "/api/Syllabus";
        public static string API_MATERIALS = "/api/Materials";
        public static string API_GRADING_STRUTURE = "/api/GradingStruture";
        public static string API_CLO = "/api/CLOs";
        public static string API_SCHEDULE = "/api/Session";


        public SyllabusController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new SyllabusRepository();
            repo2 = new SubjectRepository();
            repo3 = new AssessmentTypeRepository();
            repo4 = new CLORepository();
            repo5 = new AssessmentMethodRepository();
            repo6 = new MaterialRepository();
            repo7 = new GradingStrutureRepository();
            client = new HttpClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        [HttpGet]
        public ActionResult GetListSyllabus(int page, int limit, string? txtSearch, string? subjectCode)
        {
            List<Syllabus> rs = new List<Syllabus>();
            try
            {
                int limit2 = repo.GetTotalSyllabus(txtSearch, subjectCode);
                List<Syllabus> list = repo.GetListSyllabus(page, limit, txtSearch, subjectCode);
                var result = _mapper.Map<List<SyllabusResponse>>(list);
                return Ok(new BaseResponse(false, "Sucess", new BaseListResponse(page, limit2, result)));
            }
            catch (Exception)
            {
                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpPost]
        public ActionResult CreateSyllabus(SyllabusRequest request)
        {

            try
            {
                Syllabus rs = _mapper.Map<Syllabus>(request);

                var result = repo.CreateSyllabus(rs);
                return Ok(new BaseResponse(false, "Sucess", rs));
            }
            catch (Exception)
            {
                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpGet("GetSyllabusDetails")]
        public ActionResult SyllabusDetails(int syllabus_id)
        {
            try
            {
                Syllabus rs1 = repo.GetSyllabusById(syllabus_id);
                var result = _mapper.Map<SyllabusDetailsResponse>(rs1);
                List<PreRequisite> pre = repo.GetPre(rs1.subject_id);

                result.pre_required = _mapper.Map<List<PreRequisiteResponse2>>(pre);
                return Ok(new BaseResponse(true, "False", result));

            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
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

                            }
                            catch (Exception)
                            {

                                return BadRequest("False when craete syllabus.");
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


                            foreach (var item in materialExcel)
                            {
                                if (item.material_type != null)
                                {
                                    item.learning_resource_id = 1;
                                    item.syllabus_id = syllabusId;
                                    MaterialRequest addRs = _mapper.Map<MaterialRequest>(item);
                                    try
                                    {
                                        await CreateMaterialsAPI(addRs);

                                    }
                                    catch (Exception)
                                    {

                                        return BadRequest("False when saving CLOs.");

                                    }
                                }

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
                            //Add to database

                            foreach (var item in listClo)
                            {
                                CLOsRequest addRs = _mapper.Map<CLOsRequest>(item);
                                addRs.syllabus_id = syllabusId;
                                int idClo = await CreateCLOsAPI(addRs);
                                if (idClo == 0)
                                {
                                    return BadRequest("False when saving CLOs.");
                                }
                                cloId.Add(idClo);

                            }

                            gradingStrutureCreate.gradingCLORequest = new GradingCLORequest();
                            gradingStrutureCreate.gradingCLORequest.CLO_id = cloId;
                            rs.Add(value);
                        }
                        else if (i == 4)
                        {
                            var row = MiniExcel.Query<ScheduleExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            var scheduleExcel = GetScheduleExcel(row, syllabusExcel);
                            var value = new
                            {
                                GradingStruture = scheduleExcel,
                            };
                            foreach (var item in scheduleExcel)
                            {
                                SessionCreateRequest dataSession = new SessionCreateRequest();
                                dataSession.session = _mapper.Map<SessionRequest>(item);

                                // Initialize the session_clo list
                                dataSession.session_clo = new List<SessionCLOsRequest>();

                                foreach (var it in cloId)
                                {
                                    dataSession.session_clo.Add(new SessionCLOsRequest { CLO_id = it });
                                }
                                dataSession.session.syllabus_id = syllabusId;
                                dataSession.session.class_session_type_id = 1;
                                try
                                {
                                    await CreateSchudeleAPI(dataSession);

                                }
                                catch (Exception ex)
                                {

                                    return BadRequest("False when saving schudule.");
                                }
                            }

                            rs.Add(value);
                        }
                        else if (i == 5)
                        {
                            var row = MiniExcel.Query<GradingStrutureExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            List<GradingStruture> gradingStrutureExcel = GetGradingStrutureExcel(row, syllabusExcel);
                            gradingStrutureCreate.gradingStruture = new GradingStrutureRequest();
                            var list = _mapper.Map<List<GradingStrutureRequest>>(gradingStrutureExcel);

                            foreach (var gra in list)
                            {
                                gradingStrutureCreate.gradingStruture = gra;
                                gradingStrutureCreate.gradingStruture.syllabus_id = syllabusId;
                                gradingStrutureCreate.gradingCLORequest = new GradingCLORequest();
                                List<int> lst = new List<int>();

                                foreach (var cl in cloId)
                                {
                                    string name = "null";
                                    if (repo4.GetCLOsById(cl) != null)
                                    {
                                        name = repo4.GetCLOsById(cl).CLO_name;
                                        if (gra.clo_name.Contains(name))
                                        {
                                            lst.Add(cl);
                                        }
                                    }

                                    if (gra.clo_name.Contains("All CLOs"))
                                    {
                                        lst = new List<int>();
                                        lst.AddRange(cloId);
                                        gra.number_of_questions = "";
                                    }

                                }

                                gradingStrutureCreate.gradingCLORequest.CLO_id = lst;
                                try
                                {
                                    await CreateGradingStrutureAPI(gradingStrutureCreate);

                                }
                                catch (Exception ex)
                                {

                                    throw new Exception("Error when saving Grading Struture");
                                }

                            }

                            var value = new
                            {
                                GradingStruture = list,
                            };
                            rs.Add(value);
                        }
                    }
                    return Ok(new BaseResponse(true, "False", rs));

                }
                return Ok(new BaseResponse(true, "False", null));

            }
            catch (Exception ex)
            {

                return BadRequest(new BaseResponse(true, "error", ex.Message));
            }

        }

        private async Task<int> CreateSyllabusAPI(Syllabus sy)
        {
            string apiUrl = API_PORT + API_SYLLABUS;
            var jsonData = JsonSerializer.Serialize(sy);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

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
                //try
                //{

                //    if (GetAssessmentMethodByName(r.assessment_component) != null)
                //    {
                //        //g.assessment_method_id = GetAssessmentMethodByName(r.assessment_type).assessment_method_id;
                //        g.assessment_method_id = 1;
                //    }
                //}
                //catch (Exception)
                //{

                //    throw new Exception("Assesement Method not exist in system!");
                //}
                g.assessment_method_id = 1;
                g.clo_name = r.CLO;
                result.Add(g);

            }
            return result;
        }

        private AssessmentMethod GetAssessmentMethodByName(string assessment_type)
        {
            return repo5.GetAssessmentMethodByName(assessment_type);
        }

        private AssessmentMethod GetAssessmentTypeByName(string assessment_type)
        {
            return repo5.GetAssessmentMethodByName(assessment_type);
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
                se.schedule_lecturer_task = r.lecture_task;
                se.schedule_student_task = r.student_task;
                rs.Add(se);
            }
            return rs;
        }

        private List<CLO> GetClosExcel(IEnumerable<CLOsExcel> row)
        {
            List<CLO> result = new List<CLO>();
            foreach (var r in row)
            {
                if (r.CLO_Name == null) { }
                CLO c = new CLO();
                //   if(r.CLO_Name.Equals("All CLOs"))
                //   {
                c.CLO_id = 1;
                //   }
                //  else
                //  {
                //    c.CLO_id = GetCloIdByName(r.CLO_Name).CLO_id;

                // }
                c.CLO_name = r.CLO_Name;
                c.CLO_description = r.CLO_Description;
                result.Add(c);
            }
            return result;
        }

        private CLO GetCloIdByName(string cLO_Name)
        {
            return repo4.GetCLOByName(cLO_Name);
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
                // m.material_published_date = DateTime.Parse(r.Published_Date);
                m.material_edition = r.Edition;
                materials.Add(m);

            }
            return materials;
        }

        private Syllabus GetSyllabusExel(IEnumerable<SyllabusExcel> row)
        {
            var syllabus = new Syllabus();
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
                            throw new Exception("Subject code not exist in system!");
                        }
                    }
                    else if (r.Title.Equals("Leaning-Teaching Method"))
                    {


                    }
                    else if (r.Title.Equals("No of credits"))
                    {
                        // syllabus.Subject.credit = int.Parse(r.Details);
                    }
                    else if (r.Title.Equals("Degree Level"))
                    {
                        syllabus.degree_level = r.Details;
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
                    else if (r.Title.Equals("Scoring Scale"))
                    {
                        // syllabus.scoring_scale = int.Parse(r.Details);
                    }
                    else if (r.Title.Equals("Approved date"))
                    {
                        syllabus.approved_date = DateTime.Parse(r.Details);
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
            return repo2.GetSubjectByCode(name);
        }

        [HttpPatch]
        public ActionResult UpdatePatchSyllabus(SyllabusPatchRequest request)
        {
            try
            {
                Syllabus rs = _mapper.Map<Syllabus>(request);

                //   rs = repo.GetSession(syllabus_id);
                string result = repo.UpdatePatchSyllabus(rs);
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
            string templatePath = "Syllabus.xlsx";
            var syllabus = repo.GetSyllabusById(syllabus_id);
           // var materials = repo6.GetMaterialById(syllabus_id);
           // var clos = repo4.GetCLOsById(syllabus_id);
           // var gradingStruture = repo7.GetGradingStrutureById(syllabus_id);

   

            Dictionary<string, object> value = new Dictionary<string, object>()
            {
                ["syllabus"] = syllabus

            };

            MemoryStream memoryStream = new MemoryStream();
            memoryStream.SaveAsByTemplate(templatePath, value);
            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] fileContents = memoryStream.ToArray();
            //return Ok(fileContents);
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SyllabusExported.xlsx");
        }

        [HttpPost("SetStatus")]
        public ActionResult SetStatusSyllabus(int id)
        {
            try
            {
               
                var result = repo.SetStatusSyllabus(id);
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
               
                var result = repo.SetApproved(id);
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
