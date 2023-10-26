﻿using AutoMapper;
using BusinessObject;
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
           
            CreateMap<Subject, SubjectRequest>().ReverseMap();
            CreateMap<Syllabus, SyllabusResponse>().ReverseMap();
            //Combo
            CreateMap<Combo, ComboResponse>().ReverseMap();

            //Major
            CreateMap<BusinessObject.Major, MajorRequest>().ReverseMap();
            CreateMap<BusinessObject.Major, MajorEditRequest>().ReverseMap();
            //Syllabus
            CreateMap<Syllabus, SyllabusResponse>()
                .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.Subject.subject_code))
                .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.Subject.subject_name))
                .ForMember(dest => dest.approved_date, opt => opt.MapFrom(src => src.approved_date))
                .ForMember(dest => dest.syllabus_name, opt => opt.MapFrom(src => src.Subject.english_subject_name + "_" + src.Subject.subject_name))
                 .ReverseMap();
            CreateMap<Syllabus, SyllabusDetailsResponse>()
              .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.Subject.subject_code))
              .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.Subject.subject_name))
              .ForMember(dest => dest.decision_No, opt => opt.MapFrom(src => src.decision_No))
              .ForMember(dest => dest.english_subject_name, opt => opt.MapFrom(src => src.Subject.english_subject_name))
              .ForMember(dest => dest.learning_teaching_method, opt => opt.MapFrom(src => src.Subject.LearningMethod.learning_method_name))
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
            CreateMap<BusinessObject.Specialization, SpecializationRequest>().ReverseMap();
            CreateMap<BusinessObject.Specialization, SpecializationUpdateRequest>().ReverseMap();
            //AssessmentType
            CreateMap<BusinessObject.AssessmentType, AssessmentTypeResponse>().ReverseMap();
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
                .ForMember(dest => dest.pre_requisite_type_name, opt => opt.MapFrom(src => src.PreRequisiteType.pre_requisite_type_name))
                .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.PreSubject.subject_code))
                .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.PreSubject.english_subject_name))
                .ReverseMap();

            CreateMap<PreRequisite, PreRequisiteRequest>().ReverseMap();

            CreateMap<PreRequisite, PreRequisiteResponse2>()
             .ForMember(dest => dest.prequisite_subject_name, opt => opt.MapFrom(src => src.Subject.subject_name))
              .ForMember(dest => dest.prequisite_name, opt => opt.MapFrom(src => src.PreRequisiteType.pre_requisite_type_name))
              .ReverseMap();

            //Combo
            CreateMap<BusinessObject.Combo, ComboRequest>().ReverseMap();
            CreateMap<BusinessObject.Combo, ComboUpdateRequest>().ReverseMap();

            //Curriculum
            CreateMap<Curriculum, CurriculumResponse>()
              .ForMember(dest => dest.specialization_name, opt => opt.MapFrom(src => src.Specialization.specialization_english_name))
              .ForMember(dest => dest.batch_name, opt => opt.MapFrom(src => src.Batch.batch_name))
              .ForMember(dest => dest.vocational_code, opt => opt.MapFrom(src => src.Specialization.Major.major_code))
              .ForMember(dest => dest.vocational_name, opt => opt.MapFrom(src => src.Specialization.Major.major_name))
              .ForMember(dest => dest.vocational_english_name, opt => opt.MapFrom(src => src.Specialization.Major.major_english_name))
              .ReverseMap();

            CreateMap<Curriculum, CurriculumRequest>().ReverseMap();

            CreateMap<Curriculum, CurriculumUpdateRequest>().ReverseMap();

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
            //PLOs
            CreateMap<PLOs, PLOsDTOResponse>().ReverseMap();
            CreateMap<PLOs, PLOsDTORequest>().ReverseMap();

            //PLOMapping
            CreateMap<PLOMapping, PLOMappingDTO>()
                .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.Subject.subject_code))
                .ForMember(dest => dest.subject_group, opt => opt.MapFrom(src => src.Subject.CurriculumSubjects.Select(x => x.subject_group)))
                .ForMember(dest => dest.PLOs, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
