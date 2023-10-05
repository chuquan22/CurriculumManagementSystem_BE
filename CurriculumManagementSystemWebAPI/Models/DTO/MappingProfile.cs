using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Models.DTO.request;
using CurriculumManagementSystemWebAPI.Models.DTO.response;
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

            CreateMap<Subject, SubjectRequest>().ReverseMap();
        }
    }
}
