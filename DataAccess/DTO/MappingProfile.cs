using AutoMapper;
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
            CreateMap<Subject, SubjectResponse>()
               .ForMember(dest => dest.assessment_method_name, opt => opt.MapFrom(src => src.AssessmentMethod.assessment_method_component))
               .ForMember(dest => dest.learning_method_name, opt => opt.MapFrom(src => src.LearningMethod.learning_method_name))
               .ReverseMap();

            CreateMap<Subject, SubjectExeclResponse>()
               .ForMember(dest => dest.assessment_method_name, opt => opt.MapFrom(src => src.AssessmentMethod.assessment_method_component))
               .ForMember(dest => dest.learning_method_name, opt => opt.MapFrom(src => src.LearningMethod.learning_method_name))

               .ReverseMap();

            CreateMap<Subject, SubjectRequest>().ReverseMap();
            CreateMap<Syllabus, SyllabusResponse>().ReverseMap();

            CreateMap<Syllabus, SyllabusResponse>()
                .ForMember(dest => dest.subject_code,opt => opt.MapFrom(src => src.Subject.subject_code))
                .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.Subject.subject_name))
                .ForMember(dest => dest.isApproved, opt => opt.MapFrom(src => src.approved_date))
                .ForMember(dest => dest.syllabus_name, opt => opt.MapFrom(src => src.Subject.english_subject_name + "_" + src.Subject.subject_name))
                 .ReverseMap();

            CreateMap<Curriculum, CurriculumResponse>()
              .ForMember(dest => dest.specialization_name, opt => opt.MapFrom(src => src.Specialization.specialization_name))
              .ForMember(dest => dest.batch_name, opt => opt.MapFrom(src => src.Batch.batch_name))
              .ReverseMap();

            CreateMap<Curriculum, CurriculumRequest>().ReverseMap();

            CreateMap<CurriculumSubject, CurriculumSubjectResponse>()
              .ForMember(dest => dest.curriculum_code, opt => opt.MapFrom(src => src.Curriculum.curriculum_code))
              .ForMember(dest => dest.curriculum_name, opt => opt.MapFrom(src => src.Curriculum.curriculum_name))
              .ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.Subject.subject_code))
              .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.Subject.subject_name))
              .ReverseMap();

            CreateMap<User, UserLoginResponse>();
            CreateMap<PreRequisiteType, PreRequisiteTypeRequest>().ReverseMap();
            CreateMap<PreRequisiteType, PreRequisiteTypeResponse>().ReverseMap();

            CreateMap<PreRequisite, PreRequisiteResponse>()
                //.ForMember(dest => dest.subject_code, opt => opt.MapFrom(src => src.Subject.subject_code))
                //.ForMember(dest => dest.pre_subject_code, opt => opt.MapFrom(src => src.PreSubject.subject_code))
                .ForMember(dest => dest.pre_requisite_type_name, opt => opt.MapFrom(src => src.PreRequisiteType.pre_requisite_type_name))
                .ReverseMap();

            CreateMap<PreRequisite, PreRequisiteRequest>().ReverseMap();

            CreateMap<ComboCurriculum, CurriculumComboDTOResponse>()
                .ForMember(dest => dest.combo_name, opt => opt.MapFrom(src => src.Combo.combo_code + ": " + src.Combo.combo_name))
                .ReverseMap();

            CreateMap<ComboSubject, CurriculumComboSubjectDTOResponse>()
                .ForMember(dest => dest.subject_name, opt => opt.MapFrom(src => src.Subject.subject_code + ": " + src.Subject.subject_name))
                .ReverseMap();

            CreateMap<ComboSubject, CurriculumComboSubjectDTORequest>().ReverseMap();

        }
    }
}
