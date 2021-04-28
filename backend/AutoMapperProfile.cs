using System.Linq;
using AutoMapper;
using backend.Dtos.ProjectGroup;
using backend.Models;

namespace backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectGroup,GetProjectGroupDto>()
                .ForMember(dto => dto.GroupMembers, c => c.MapFrom(c => c.GroupMembers.Select(cs => cs.User)));

            CreateMap<User,UserInProjectGroupDto>();
            CreateMap<Course,CourseInProjectGroupDto>();
            CreateMap<Section,SectionInProjectGroupDto>();
        }
        
    }
}