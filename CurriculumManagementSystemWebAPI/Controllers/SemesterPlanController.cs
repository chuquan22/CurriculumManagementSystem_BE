using AutoMapper;
using BusinessObject;
using DataAccess.DAO;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.SemesterPlans;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.Xml;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterPlanController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISemesterPlanRepository _repo;
        private CMSDbContext _context = new CMSDbContext();
        public SemesterPlanController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new SemesterPlanRepository();

        }
        [HttpGet("CreateSemesterPlan/{semester_id}")]
        public ActionResult CreateSemesterPlan(int semester_id)
        {
            var Semester = _context.Semester.Where(x => x.semester_id == semester_id).FirstOrDefault();
            int degreeLevel = Semester.degree_level_id;
            var StartBatch = _context.Batch.Where(x => x.batch_id == Semester.start_batch_id).FirstOrDefault();
            var ListCurri = _context.CurriculumBatch.Where(x => x.batch_id == Semester.start_batch_id).ToList();
            List<SemesterPlanDTO> listResponse = new List<SemesterPlanDTO>();
            foreach (var item in ListCurri)
            {
                var Curriculum = _context.Curriculum.Include(x => x.Specialization).Where(x => x.curriculum_id == item.curriculum_id).FirstOrDefault();
                int validOrder = StartBatch.batch_order - Curriculum.total_semester;
                if (validOrder <= 1)
                {
                    validOrder = 1;
                }
                var listValidSemester = _context.Semester.Include(x => x.Batch).Where(x => x.degree_level_id == degreeLevel)
                    .Where(x => x.Batch.batch_order >= validOrder && x.Batch.batch_order <= StartBatch.batch_order)
                    .OrderByDescending(x => x.Batch.batch_order)
                    .ToList();
                int specializationId = Curriculum.specialization_id;
                var validBatchIds = listValidSemester.Select(validSemester => validSemester.Batch.batch_id).ToList();
                string speName = Curriculum.Specialization.specialization_english_name;
                var listCurriValid = _context.CurriculumBatch.Include(x => x.Curriculum)
                    .Where(x => validBatchIds.Contains(x.batch_id))
                    .Where(x => x.Curriculum.specialization_id == specializationId)
                    .OrderByDescending(x => x.Batch.batch_order)
                    .ToList(); 
                int termNo = 1;
                List<SemesterPlan> list = new List<SemesterPlan>();
                foreach (var curri in listCurriValid)
                {
                    foreach (var batch in listValidSemester)
                    {
                        if (curri.batch_id == batch.start_batch_id)
                        {
                            SemesterPlan s = new SemesterPlan();
                            s.curriculum_id = curri.Curriculum.curriculum_id;
                            s.semester_id = semester_id;
                            s.term_no = termNo;
                            list.Add(s);
                            termNo++;
                        }
                    }
                }
                SemesterPlanDTO sDTO = new SemesterPlanDTO { listSemester = new List<SemesterPlan>()};
                sDTO.speName = speName;
                sDTO.listSemester = list;
                listResponse.Add(sDTO);

            }
            foreach (var item in listResponse)
            {
                foreach (var semester in item.listSemester)
                {
                    SemesterPlan sw = new SemesterPlan();
                    sw.semester_id = semester.semester_id;
                    sw.curriculum_id = semester.curriculum_id;
                    sw.term_no = semester.term_no;
                    _context.SemesterPlan.Add(sw);
                    _context.SaveChanges();
                }
            }
            return Ok(new BaseResponse(false, "Get List 123", listResponse));
        }
        [HttpGet("GetSemesterPlanSubject/{semester_id}")]
        public ActionResult GetSemesterPlanSubject(int semester_id)
        {
            var Semester = _context.Semester.Include(x => x.Batch).Where(x => x.semester_id == semester_id).FirstOrDefault();
            int order = Semester.Batch.batch_order;
            var responseList = new SemesterPlanDetailsResponse
            {
                spe = new List<SemesterPlanDetailsTermResponse>(),
            };
            responseList.semesterName = Semester.semester_name + " - " + Semester.school_year ;
            var SemesterPlan = _context.SemesterPlan.Include(x => x.Curriculum).Include(x => x.Semester).Where(x => x.semester_id == Semester.semester_id).ToList();
            List<SemesterPlanCount> listCount = new List<SemesterPlanCount>();
            
            List<SemesterPlanDetailsTermResponse> SemesterPlanDetails = new List<SemesterPlanDetailsTermResponse>();
            foreach (var item in SemesterPlan)
            {
                var existingCount = listCount.FirstOrDefault(x => x.curriculumId == item.curriculum_id);

                if (existingCount != null)
                {
                    existingCount.count++;
                }
                else
                {
                    listCount.Add(new SemesterPlanCount
                    {
                        curriculumId = item.curriculum_id,
                        count = 1,
                    });
                }
            }
            foreach (var item in listCount)
            {
                var semesterPlanDetails = new SemesterPlanDetailsTermResponse
                {
                    specialization_name = "",
                    major_name = "",
                    courses = new List<DataTermNoResponse>(),
                };
                var dataTermNo = new DataTermNoResponse();
                for(int i = 1;i<=item.count;i++)
                {
                    var curriculum = _context.Curriculum
                        .Include(c => c.Specialization)
                        .ThenInclude(s => s.Major)
                        .Include(c => c.CurriculumSubjects)
                        .ThenInclude(cs => cs.Subject)
                        .ThenInclude(subject => subject.AssessmentMethod)
                        .ThenInclude(assessmentMethod => assessmentMethod.AssessmentType)
                        .FirstOrDefault(c => c.curriculum_id == item.curriculumId);
                    var curriBatch = _context.CurriculumBatch.Include(x => x.Batch)
                        .Where(x => x.curriculum_id == item.curriculumId)
                        .OrderByDescending(x => x.Batch.batch_order)
                        .ToList();
                    semesterPlanDetails.specialization_name = curriculum.Specialization.specialization_english_name;
                    semesterPlanDetails.major_name = curriculum.Specialization.Major.major_english_name;
                    semesterPlanDetails.specializationId = curriculum.Specialization.specialization_id;
                    dataTermNo = new DataTermNoResponse
                    {
                        term_no = i,
                        batch = curriBatch[i - 1].Batch.batch_name,
                        batch_order = curriBatch[i - 1].Batch.batch_order,
                        batch_check = curriBatch[i - 1].Batch.batch_name,
                        curriculum_code = curriculum.curriculum_code,
                        subjectData = curriculum.CurriculumSubjects
                            .Where(cs => cs.term_no == (order - curriBatch[i - 1].Batch.batch_order + 1) && cs.Subject.subject_code != null)
                            .Select(cs => new DataSubjectReponse
                            {
                                subject_code = cs.Subject.subject_code,
                                subject_name = cs.Subject.english_subject_name,
                                credit = cs.Subject.credit,
                                total = cs.Subject.total_time,
                                @class = cs.Subject.total_time_class,
                                @exam = cs.Subject.exam_total,
                                method = cs.Subject.AssessmentMethod.assessment_method_component,
                                assessment = cs.Subject.AssessmentMethod.AssessmentType.assessment_type_name
                            })
                            .ToList(),
                    };

                    semesterPlanDetails.courses.Add(dataTermNo);    
                }

                responseList.spe.Add(semesterPlanDetails);
            }
            return Ok(new BaseResponse(false, "Get List 123", responseList));
        }
        

        //[HttpGet("GetSemesterPlanDetails/{semester_id}/{degree_level}")]
        //public ActionResult GetSemesterPlanDetails(int semester_id, string degree_level)
        //{
        //    var list = _repo.GetSemesterPlanDetails(semester_id, degree_level);
        //    var rs = _mapper.Map<SemesterPlanDetailsResponse>(list);
        //    return Ok(new BaseResponse(false, "Get List", rs));
        //}
    }
}
