using System.Linq;
using AutoMapper;
using backend.Dtos.Course;
using backend.Dtos.ProjectGroup;
using backend.Dtos.Section;
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
            CreateMap<User,InstructorInCourseDto>();
            CreateMap<Course,CourseInProjectGroupDto>();
            CreateMap<Section,SectionInProjectGroupDto>();
            CreateMap<Course,GetCourseDto>()
                .ForMember( dto => dto.Instructors, c => c.MapFrom( c => c.Instructors.Select( cs => cs.User ) ) );
            CreateMap<Course,CourseInSectionDto>();
            CreateMap<ProjectGroup,ProjectGroupInSectionDto>();
            CreateMap<Section,GetSectionOfCourseDto>();
            CreateMap<Section,GetSectionDto>();
        }
        
    }
}