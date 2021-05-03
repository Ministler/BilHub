using System.Linq;
using AutoMapper;
using backend.Dtos.Course;
using backend.Dtos.Section;
using backend.Dtos.Assignment;
using backend.Dtos.Comment;
using backend.Dtos.ProjectGroup;
using backend.Dtos.Submission;
using backend.Dtos.User;
using backend.Dtos.PeerGrade;
using backend.Models;
using backend.Dtos.JoinRequest;
using backend.Dtos.MergeRequest;

namespace backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectGroup, GetProjectGroupDto>()
                .ForMember(dto => dto.GroupMembers, c => c.MapFrom(c => c.GroupMembers.Select(cs => cs.User)))
                .ForMember( dto => dto.AffiliatedCourseName, opt => opt.Ignore() )
                .ForMember( dto => dto.IsActive, opt => opt.Ignore() )
                .ForMember( dto => dto.ConfirmStateOfCurrentUser, opt => opt.Ignore() );

            CreateMap<User, UserInProjectGroupDto>();
            CreateMap<User, InstructorInCourseDto>();
            CreateMap<Course, CourseInProjectGroupDto>();
            CreateMap<Section, SectionInProjectGroupDto>();
            CreateMap<Course, GetCourseDto>()
                .ForMember(dto => dto.Instructors, c => c.MapFrom(c => c.Instructors.Select(cs => cs.User)))
                .ForMember( dto => dto.CurrentUserSectionId, opt => opt.Ignore() )
                .ForMember( dto => dto.IsInstructorOrTAInCourse, opt => opt.Ignore() )
                .ForMember( dto => dto.IsUserInFormedGroup, opt => opt.Ignore() )
                .ForMember( dto => dto.IsUserAlone, opt => opt.Ignore() );

            CreateMap<Course, CourseInSectionDto>();
            
            CreateMap<User,GroupMemberInSectionDto>();
            CreateMap<ProjectGroup, ProjectGroupInSectionDto>()
                .ForMember ( dto => dto.GroupMembers, c => c.MapFrom(c => c.GroupMembers.Select(cs => cs.User)) );

            CreateMap<Section, GetSectionOfCourseDto>();
            CreateMap<Section, GetSectionDto>();
            CreateMap<Assignment, GetAssignmentDto>();
            CreateMap<Submission, GetSubmissionDto>();
            CreateMap<Comment, GetCommentDto>();
            CreateMap<User, GetUserDto>();
            CreateMap<User, GetCommentorDto>();
            CreateMap<PeerGrade, PeerGradeInfoDto>();

            CreateMap<User,UserInJoinRequestDto>();
            CreateMap<ProjectGroup,ProjectGroupInJoinRequestDto>();
            CreateMap<JoinRequest,GetJoinRequestDto>();

            CreateMap<ProjectGroup,ProjectGroupInMergeRequestDto>();
            CreateMap<MergeRequest,GetMergeRequestDto>();
            CreateMap<User,UsersOfCourseDto>();

            CreateMap<User,GetUserInfoDto>();
        }

    }
}