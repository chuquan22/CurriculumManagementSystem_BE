using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.Excel;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using Repositories.AssessmentTypes;
using Repositories.CLOS;
using Repositories.GradingStruture;
using Repositories.Materials;
using Repositories.Session;
using Repositories.Subjects;
using Repositories.Syllabus;
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

        public string API_GRADINGSTRUTURE = "/api/GradingStruture";
        public SyllabusController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new SyllabusRepository();
            repo2 = new SubjectRepository();
            repo3 = new AssessmentTypeRepository();
            repo4 = new CLORepository();
        }
        [HttpGet]
        public ActionResult GetListSyllabus(int page,int limit, string? txtSearch, string? subjectCode)
        {
            List<Syllabus> rs = new List<Syllabus>();
            try
            {             
                int limit2 = repo.GetTotalSyllabus(txtSearch, subjectCode);
                List<Syllabus> list = repo.GetListSyllabus(page, limit, txtSearch, subjectCode);
                var result = _mapper.Map<List<SyllabusResponse>>(list);
                return Ok(new BaseResponse(false, "Sucess", new BaseListResponse(page,limit2, result)));
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
                    Syllabus syllabusExcel = new Syllabus();
                    GradingStrutureCreateRequest gradingStrutureCreate = new GradingStrutureCreateRequest();
                    for (int i = 0; i < sheetNames.Count; i++)
                    {
                        if (i == 0)
                        {
                            var row = MiniExcel.Query<SyllabusExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            syllabusExcel = GetSyllabusExel(row);
                            var value = new
                            {
                                Syllabus = syllabusExcel,
                            };
                            syllabusId = syllabusExcel.syllabus_id;
                            
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
                            rs.Add(value);
                        }
                        else if (i == 2)
                        {
                            var row = MiniExcel.Query<CLOsExcel>(filePath, sheetName: sheetNames[i], excelType: ExcelType.XLSX);
                            var cloExcel = GetClosExcel(row);
                            var value = new
                            {
                                CLOs = cloExcel,
                            };
                            List<int> cloId = new List<int>();
                            foreach (var item in cloExcel)
                            {
                                cloId.Add(item.CLO_id);
                            }
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
                            rs.Add(value);
                        }
                        else if (i == 5)
                        {
                            var row = MiniExcel.Query<GradingStrutureExcel>(filePath, sheetName: sheetNames[i],excelType: ExcelType.XLSX);
                           var gradingStrutureExcel = GetGradingStrutureExcel(row, syllabusExcel);
                            gradingStrutureCreate.gradingStruture = _mapper.Map<GradingStrutureRequest>(gradingStrutureExcel);
                            var value = new
                            {
                                GradingStruture = gradingStrutureExcel,
                            };
                            rs.Add(value);
                        }
                    }
                    //Call API 
                    try
                    {
                        //await CallAPIAsync(gradingStrutureCreate,API_GRADINGSTRUTURE);

                    }
                    catch (Exception)
                    {

                        throw;
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
        [HttpPost]
        [Route("callapi")]
        public async Task CallAPIAsync(object ob, string api)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(ob);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(api, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions. You might want to log the exception or throw it.
            }
        }
        private List<GradingStruture> GetGradingStrutureExcel(IEnumerable<GradingStrutureExcel> row, Syllabus syllabus)
        {
            List<GradingStruture> result = new List<GradingStruture>();
            foreach (var r in row)
            {
                GradingStruture g = new GradingStruture();
                g.type_of_questions = r.type_of_questions;
                g.number_of_questions = r.number_of_questions;
                g.session_no = int.Parse(r.SessionNo);
                g.references = r.Reference;
                g.grading_weight = r.weight;
                g.grading_part = r.Part;
                g.syllabus_id = syllabus.syllabus_id;
                g.minimum_value_to_meet_completion = r.minimun_value_to_meet;
                g.grading_duration = r.Duration;
                g.scope_knowledge = r.scope;
                g.how_granding_structure = r.how;
                g.assessment_method_id = GetAssessmentTypeIdByName(r.assessment_type).assessment_type_id;
                g.grading_note = r.Note;
                result.Add(g);

            }
            return result;
        }

        private AssessmentType GetAssessmentTypeIdByName(string assessment_type)
        {
            return repo3.GetAssessmentTypeByName(assessment_type);
        }

        private List<Session> GetScheduleExcel(IEnumerable<ScheduleExcel> row, Syllabus syllabus)
        {
            List<Session> rs = new List<Session>();
            foreach (var r in row)
            {
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
            foreach (var  r in row)
            {
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
                        int subjectId = subject.subject_id;
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
    }
}
