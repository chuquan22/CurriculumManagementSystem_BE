using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.Excel;
using DataAccess.Models.DTO.Report;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Subject
            CreateMap<Subject, SubjectResponse>()
               .ForMember(dest => dest.assessment_method_name, opt => opt.MapFrom(src => src.AssessmentMethod.assessment_method_component))
               .ForMember(dest => dest.learning_method_name, opt => opt.MapFrom(src => src.LearningMethod.learning_method_name))
               .ReverseMap();

            CreateMap<Subject, SubjectDTO>()
               .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.english_subject_name))
               .ReverseMap();

            CreateMap<Subject, SubjectRequest>()
                .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.subject_code.Trim().ToUpper()))
                .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.subject_name.Trim()))
                .ForMember(dest => dest.english_subject_name, opt => opt.MapFrom(src => src.english_subject_name.Trim()))
                .ReverseMap();
            CreateMap<Syllabus, SyllabusResponse>().ReverseMap();
            //Combo
            CreateMap<Combo, ComboResponse>().ReverseMap();
            CreateMap<CLO, CLOsExportExcel>().ReverseMap();
            //CLO
            CreateMap<Session, SessionExcelExport>()
                .ForMember(dest => dest.class_session_type_name, opt => opt.MapFrom(src => src.ClassSessionType.class_session_type_name))
                .ReverseMap();

            //MajorReponse

            CreateMap<BusinessObject.Major, MajorResponse>()
             .ForMember(dest => dest.degree_level_name, opt => opt.MapFrom(src => src.DegreeLevel.degree_level_english_name))
             .ReverseMap();

            CreateMap<BusinessObject.Major, MajorSubjectDTOResponse>()
             .ForMember(dest => dest.degree_level_name, opt => opt.MapFrom(src => src.DegreeLevel.degree_level_english_name))
             .ReverseMap();

            CreateMap<BusinessObject.Major, MajorSpeResponse>()
            .ReverseMap();

            //MaterialsResponse
            CreateMap<Material, MaterialsResponse>()
             .ForMember(dest => dest.learning_resource_name, opt => opt.MapFrom(src => src.LearningResource.learning_resource_type))
             .ReverseMap();

            //Major

            CreateMap<MajorRequest, BusinessObject.Major>()
                .ForMember(dest => dest.major_code, opt => opt.MapFrom(src => src.major_code.Trim().ToUpper()))
                .ForMember(dest => dest.major_name, opt => opt.MapFrom(src => src.major_name.Trim()))
                .ForMember(dest => dest.major_english_name, opt => opt.MapFrom(src => src.major_english_name.Trim()))
                .ReverseMap();

            CreateMap<MajorEditRequest, BusinessObject.Major>()
                .ForMember(dest => dest.major_name, opt => opt.MapFrom(src => src.major_name.Trim()))
                .ForMember(dest => dest.major_english_name, opt => opt.MapFrom(src => src.major_english_name.Trim()))
                .ReverseMap();
            


            CreateMap<Material, MaterialExportExcel>().ReverseMap();

            //SemesterBatchResponse
            CreateMap<SemesterPlan, SemesterPlanResponse>()
               .ForMember(dest => dest.spe, opt => opt.MapFrom(src => src.Curriculum.Specialization.specialization_english_name))
                .ForMember(dest => dest.totalSemester, opt => opt.MapFrom(src => src.Curriculum.total_semester))
                 .ForMember(dest => dest.semester, opt => opt.MapFrom(src => src.Semester.semester_name))
                //.ForMember(dest => dest.batch, opt => opt.MapFrom(src => src.Curriculum.Batch))
                .ReverseMap();

            //Excel Syllabus
            CreateMap<GradingStrutureRequest, GradingStruture>().ReverseMap();
            CreateMap<Syllabus, SyllabusRequest>().ReverseMap();
            CreateMap<GradingStruture, GradingStrutureExportExcel>()
            .ForMember(dest => dest.assessment_method_name, opt => opt.MapFrom(src => src.AssessmentMethod.assessment_method_component))
            .ForMember(dest => dest.assessment_type_name, opt => opt.MapFrom(src => src.AssessmentMethod.AssessmentType.assessment_type_name))
            .ReverseMap();

            //
            CreateMap<GradingStruture, GradingStrutureExcel>()
                 .ForMember(dest => dest.type_of_questions, opt => opt.MapFrom(src => src.type_of_questions))
                 .ForMember(dest => dest.number_of_questions, opt => opt.MapFrom(src => src.number_of_questions))
                 .ForMember(dest => dest.SessionNo, opt => opt.MapFrom(src => src.session_no))
                 .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.references))
                 .ForMember(dest => dest.weight, opt => opt.MapFrom(src => src.grading_weight))
                 .ForMember(dest => dest.Part, opt => opt.MapFrom(src => src.grading_part))
                 .ForMember(dest => dest.minimun_value_to_meet, opt => opt.MapFrom(src => src.minimum_value_to_meet_completion))
                 .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.grading_duration))
                 .ForMember(dest => dest.scope, opt => opt.MapFrom(src => src.scope_knowledge))
                 .ForMember(dest => dest.how, opt => opt.MapFrom(src => src.how_granding_structure))
                 .ForMember(dest => dest.assessment_type, opt => opt.MapFrom(src => src.assessment_method_id))
                 .ReverseMap();

            //Syllabus
            CreateMap<Syllabus, SyllabusResponse>()
                .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.Subject.subject_code))
                .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.Subject.subject_name))
                .ForMember(dest => dest.approved_date, opt => opt.MapFrom(src => src.approved_date))
                  .ForMember(dest => dest.time_allocation, opt => opt.MapFrom(src => src.time_allocation))
                .ForMember(dest => dest.syllabus_name, opt => opt.MapFrom(src => src.Subject.english_subject_name + "_" + src.Subject.subject_name))
                 .ReverseMap();
            CreateMap<Syllabus, SyllabusDetailsResponse>()
              .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.Subject.subject_code))
              .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.Subject.subject_name))
              .ForMember(dest => dest.decision_No, opt => opt.MapFrom(src => src.decision_No))
              .ForMember(dest => dest.english_subject_name, opt => opt.MapFrom(src => src.Subject.english_subject_name))
              .ForMember(dest => dest.learning_teaching_method, opt => opt.MapFrom(src => src.Subject.LearningMethod.learning_method_name))
              .ForMember(dest => dest.time_allocation, opt => opt.MapFrom(src => src.time_allocation))
              .ForMember(dest => dest.degree_level_id, opt => opt.MapFrom(src => src.degree_level_id))

              .ForMember(dest => dest.degree_level, opt => opt.MapFrom(src => src.DegreeLevel.degree_level_name))
              .ForMember(dest => dest.credit, opt => opt.MapFrom(src => src.Subject.credit))
              .ReverseMap();
            //CLOS
            CreateMap<BusinessObject.CLO, CLOsRequest>().ReverseMap();
            CreateMap<BusinessObject.CLO, CLOsUpdateRequest>().ReverseMap();
            //Syllabus
            CreateMap<BusinessObject.Syllabus, SyllabusPatchRequest>().ReverseMap();

            //Materials
            CreateMap<BusinessObject.Material, MaterialRequest>().ReverseMap();
            CreateMap<BusinessObject.Material, MaterialUpdateRequest>().ReverseMap();
            //Specialization
            CreateMap<SpecializationRequest, BusinessObject.Specialization>()
                .ForMember(dest => dest.specialization_name, opt => opt.MapFrom(src => src.specialization_name.Trim()))
                .ForMember(dest => dest.specialization_english_name, opt => opt.MapFrom(src => src.specialization_english_name.Trim()))
                .ForMember(dest => dest.specialization_code, opt => opt.MapFrom(src => src.specialization_code.Trim()))
                .ReverseMap();

            CreateMap<SpecializationUpdateRequest, BusinessObject.Specialization>()
                 .ForMember(dest => dest.specialization_name, opt => opt.MapFrom(src => src.specialization_name.Trim()))
                .ForMember(dest => dest.specialization_english_name, opt => opt.MapFrom(src => src.specialization_english_name.Trim()))
                .ReverseMap();

            CreateMap<BusinessObject.Specialization, SpecializationResponse>()
               .ReverseMap();
            //AssessmentType
            CreateMap<BusinessObject.AssessmentType, AssessmentTypeResponse>().ReverseMap();
            CreateMap<AssessmentTypeRequest, AssessmentType>()
                .ForMember(dest => dest.assessment_type_name, opt => opt.MapFrom(src => src.assessment_type_name.Trim()))
                .ReverseMap();
            //Session
            CreateMap<BusinessObject.Session, SessionRequest>().ReverseMap();
            CreateMap<BusinessObject.Session, SessionUpdate>().ReverseMap();
            CreateMap<BusinessObject.Session, SessionPatchRequest>().ReverseMap();

            CreateMap<BusinessObject.SessionCLO, SessionCLOsRequest>().ReverseMap();

            CreateMap<BusinessObject.Session, SessionResponse>()
             .ForMember(dest => dest.listCLOs, opt => opt.MapFrom(src => src.SessionCLO
             .Where(gc => gc.session_id == src.schedule_id)
             .Select(gc => new ListCLOsResponse { CLO_name = gc.CLO.CLO_name, CLO_id = gc.CLO.CLO_id })
             .ToList()))
                .ReverseMap();
            //ClassSessionType
            CreateMap<BusinessObject.ClassSessionType, ClassSessionTypeResponse>().ReverseMap();
            CreateMap<ClassSessionTypeRequest, BusinessObject.ClassSessionType>()
                .ForMember(dest => dest.class_session_type_name, opt => opt.MapFrom(src => src.class_session_type_name.Trim()))
                .ReverseMap();
            //GradingStruture
            CreateMap<BusinessObject.GradingStruture, GradingStrutureResponse>()
                 .ForMember(dest => dest.assessment_method_id, opt => opt.MapFrom(src => src.AssessmentMethod.assessment_method_id))
                 .ForMember(dest => dest.assessment_component, opt => opt.MapFrom(src => src.AssessmentMethod.assessment_method_component))
                 .ForMember(dest => dest.assessment_type, opt => opt.MapFrom(src => src.AssessmentMethod.AssessmentType.assessment_type_name))
                 .ForMember(dest => dest.listCLO, opt => opt.MapFrom(src => src.GradingCLOs
                 .Where(gc => gc.grading_id == src.grading_id)
                 .Select(gc => new ListCLOsResponse { CLO_name = gc.CLO.CLO_name, CLO_id = gc.CLO.CLO_id })
                 .ToList()))
                 .ReverseMap();

            //GradingStrutureRequest GradingUpdateRequest
            CreateMap<GradingStruture, GradingStrutureRequest>()
                .ReverseMap();
            CreateMap<GradingStruture, GradingUpdateRequest>()
               .ReverseMap();
            //GradingCLO
            CreateMap<GradingCLO, GradingCLORequest>()
                .ReverseMap();
            CreateMap<GradingCLORequest, GradingCLO>()
              .ReverseMap();
            //PreRequisite
            CreateMap<PreRequisite, PreRequisiteResponse>()
                .ForMember(dest => dest.pre_requisite_type_name, opt => opt.MapFrom(src => src.PreRequisiteType.pre_requisite_type_name.Trim()))
                .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.PreSubject.subject_code.Trim()))
                .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.PreSubject.english_subject_name.Trim()))
                .ReverseMap();

            CreateMap<PreRequisite, PreRequisiteRequest>().ReverseMap();

            CreateMap<PreRequisite, PreRequisiteResponse2>()
              .ForMember(dest => dest.prequisite_subject_name, opt => opt.MapFrom(src => src.Subject.subject_name.Trim()))
              .ForMember(dest => dest.prequisite_name, opt => opt.MapFrom(src => src.PreRequisiteType.pre_requisite_type_name.Trim()))
              .ReverseMap();

            //Combo
            CreateMap<ComboRequest, BusinessObject.Combo>()
                .ForMember(dest => dest.combo_english_name, opt => opt.MapFrom(src => src.combo_english_name.Trim()))
                .ForMember(dest => dest.combo_name, opt => opt.MapFrom(src => src.combo_name.Trim()))
                .ForMember(dest => dest.combo_code, opt => opt.MapFrom(src => src.combo_code.Trim()))
                .ReverseMap();
            CreateMap<BusinessObject.Combo, ComboUpdateRequest>().ReverseMap();

            //Curriculum
            CreateMap<Curriculum, CurriculumResponse>()
            .ForMember(dest => dest.specialization_name, opt => opt.MapFrom(src => src.Specialization.specialization_english_name))
            .ForMember(dest => dest.vocational_code, opt => opt.MapFrom(src => src.Specialization.Major.major_code))
            .ForMember(dest => dest.vocational_name, opt => opt.MapFrom(src => src.Specialization.Major.major_name))
            .ForMember(dest => dest.vocational_english_name, opt => opt.MapFrom(src => src.Specialization.Major.major_english_name))
            .ForMember(dest => dest.degree_level, opt => opt.MapFrom(src => src.Specialization.Major.DegreeLevel.degree_level_english_name))
            .ReverseMap();


            CreateMap<CurriculumRequest, Curriculum>()
                    .ForMember(dest => dest.english_curriculum_name, opt => opt.MapFrom(src => src.english_curriculum_name.Trim()))
                    .ForMember(dest => dest.curriculum_name, opt => opt.MapFrom(src => src.curriculum_name.Trim()))
                    .ForMember(dest => dest.curriculum_description, opt => opt.MapFrom(src => src.curriculum_description.Trim()))
                    .ReverseMap();

            CreateMap<CurriculumUpdateRequest, Curriculum>()
                .ForMember(dest => dest.english_curriculum_name, opt => opt.MapFrom(src => src.english_curriculum_name.Trim()))
                .ForMember(dest => dest.curriculum_name, opt => opt.MapFrom(src => src.curriculum_name.Trim()))
                .ReverseMap();

            //Curriculum Subject
            CreateMap<CurriculumSubject, CurriculumSubjectResponse>()
              .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.Subject.subject_code))
              .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.Subject.english_subject_name))
              .ForMember(dest => dest.credit, opt => opt.MapFrom(src => src.Subject.credit))
              .ForMember(dest => dest.total_time, opt => opt.MapFrom(src => src.Subject.total_time))
              .ForMember(dest => dest.specialization_id, opt => opt.MapFrom(src => src.Curriculum.specialization_id))
              .ReverseMap();

            CreateMap<CurriculumSubject, CurriculumSubjectRequest>()
                .ReverseMap();

            CreateMap<User, UserLoginResponse>();

            CreateMap<PreRequisiteType, PreRequisiteTypeRequest>().ReverseMap();
            CreateMap<PreRequisiteType, PreRequisiteTypeResponse>().ReverseMap();


            CreateMap<LearningMethod, LearningMethodDTOResponse>().ReverseMap();

            CreateMap<AssessmentMethod, AssessmentMethodDTOResponse>()
                .ForMember(dest => dest.assessment_type_name, opt => opt.MapFrom(src => src.AssessmentType.assessment_type_name))
                .ReverseMap();

            CreateMap<AssessmentMethodRequest, AssessmentMethod>()
                .ForMember(dest => dest.assessment_method_component, opt => opt.MapFrom(src => src.assessment_method_component.Trim()))
                .ReverseMap();
            //PLOs
            CreateMap<PLOs, PLOsDTOResponse>().ReverseMap();
            CreateMap<PLOs, PLOsDTORequest>().ReverseMap();

            //PLOMapping
            CreateMap<PLOMapping, PLOMappingDTO>()
               .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.Subject.subject_code))
               .ForMember(dest => dest.subject_group, opt => opt.MapFrom(src => src.Subject.CurriculumSubjects.Select(x => x.subject_group)))
               .ForMember(dest => dest.PLOs, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<BatchRequest, Batch>()
                .ForMember(dest => dest.batch_name, opt => opt.MapFrom(src => src.batch_name.Trim()))
                .ReverseMap();

            CreateMap<LearningResourceRequest, LearningResource>()
                .ForMember(dest => dest.learning_resource_type, opt => opt.MapFrom(src => src.learning_resource_type.Trim()))
                .ReverseMap();

            CreateMap<LearningMethodRequest, LearningMethod>()
                .ForMember(dest => dest.learning_method_name, opt => opt.MapFrom(src => src.learning_method_name.Trim()))
                .ReverseMap();
            //Semester
            CreateMap<SemesterRequest, Semester>()
                .ForMember(dest => dest.semester_name, opt => opt.MapFrom(src => src.semester_name.Trim()))
                .ReverseMap();

                    CreateMap<Semester, SemesterResponse>()
             .ForMember(dest => dest.start_batch_id, opt => opt.MapFrom(src => src.Batch.batch_id))
             .ForMember(dest => dest.batch_name, opt => opt.MapFrom(src => src.Batch.batch_name))
             .ForMember(dest => dest.batch_order, opt => opt.MapFrom(src => src.Batch.batch_order))
             .ForMember(dest => dest.degree_level_name, opt => opt.MapFrom(src => src.Batch.DegreeLevel.degree_level_english_name))
             .ForMember(dest => dest.semester_start_date, opt => opt.MapFrom(src => src.semester_start_date.ToString("yyyy-MM-dd")))
             .ForMember(dest => dest.semester_end_date, opt => opt.MapFrom(src => src.semester_end_date.ToString("yyyy-MM-dd")))
             .ReverseMap();

            //User
            CreateMap<UserCreateRequest, User>()
                .ForMember(dest => dest.user_email, opt => opt.MapFrom(src => src.user_email.Trim()))
                .ForMember(dest => dest.user_name, opt => opt.MapFrom(src => src.user_name.Trim()))
                .ForMember(dest => dest.full_name, opt => opt.MapFrom(src => src.full_name.Trim()))
                .ReverseMap();

            CreateMap<UserUpdateRequest, User>().ReverseMap();

            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.role_name, opt => opt.MapFrom(src => src.Role.role_name))
                .ReverseMap();

            //Quiz
            CreateMap<QuizDTORequest, Quiz>().ReverseMap();
            CreateMap<QuestionDTORequest, Question>().ReverseMap();

            CreateMap<Quiz, QuizDTOResponse>()
                .ForMember(dest => dest.number_question_single_choice, opt => opt.MapFrom(src => src.Questions.Where(x => x.question_type.ToLower().Equals("single choice")).Count()))
                .ForMember(dest => dest.number_question_mutiple_choice, opt => opt.MapFrom(src => src.Questions.Where(x => x.question_type.ToLower().Equals("mutiple choice")).Count()))
                .ReverseMap();

            CreateMap<Quiz, QuizResponse>()
                .ReverseMap();

            CreateMap<Question, QuestionResponse>()
                .ForMember(dest => dest.subject_id, opt => opt.MapFrom(src => src.Quiz.subject_id))
                .ReverseMap();
        }
    }
}
