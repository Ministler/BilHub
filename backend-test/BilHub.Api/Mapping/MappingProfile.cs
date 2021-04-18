using AutoMapper;
using BilHub.Api.Resources;
using BilHub.Core.Models;

namespace BilHub.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Instructor, InstructorResource>();
            CreateMap<Student, StudentResource>();
            CreateMap<Course, CourseResource>();

            // Resource to Domain
            CreateMap<InstructorResource, Instructor>();
            CreateMap<StudentResource, Student>();
            CreateMap<CourseResource, Course>();
        }
    }
}