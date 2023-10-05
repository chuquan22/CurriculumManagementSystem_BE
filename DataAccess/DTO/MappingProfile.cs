using AutoMapper;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.DTO.response;

namespace DataAccess.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Subject, SubjectDTO>()
               .ForMember(dest => dest.assessment_method_name, opt => opt.MapFrom(src => src.AssessmentMethod.assessment_method_component))
               .ForMember(dest => dest.learning_method_name, opt => opt.MapFrom(src => src.LearningMethod.learning_method_name))
               .ReverseMap();
            CreateMap<User, UserLoginResponse>();
        }
    }
}
