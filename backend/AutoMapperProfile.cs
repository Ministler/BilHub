using System.Linq;
using AutoMapper;
using backend.Dtos.Assignment;
using backend.Dtos.Comment;
using backend.Dtos.ProjectGroup;
using backend.Dtos.Submission;
using backend.Dtos.User;
using backend.Models;

namespace backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectGroup, GetProjectGroupDto>()
                .ForMember(dto => dto.GroupMembers, c => c.MapFrom(c => c.GroupMembers.Select(cs => cs.User)));

            CreateMap<User, UserInProjectGroupDto>();
            CreateMap<Course, CourseInProjectGroupDto>();
            CreateMap<Section, SectionInProjectGroupDto>();
            CreateMap<Assignment, GetAssignmentDto>();
            CreateMap<Submission, GetSubmissionDto>();
            CreateMap<Comment, GetCommentDto>();
            CreateMap<User, GetUserDto>();
        }

    }
}