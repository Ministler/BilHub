using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Dtos.Assignment;
using backend.Dtos.Course;
using backend.Dtos.ProjectGroup;
using backend.Dtos.Section;
using backend.Dtos.User;
using backend.Models;

namespace backend.Services.CourseServices
{
    public interface ICourseService
    {

        Task<ServiceResponse<GetCourseDto>> CreateCourse(CreateCourseDto createCourseDto);
        Task<ServiceResponse<GetCourseDto>> GetCourse(int courseId);
        Task<ServiceResponse<GetCourseDto>> EditCourse(EditCourseDto editCourseDto);
        Task<ServiceResponse<GetCourseDto>> AddInstructorToCourse(int userId, int courseId);
        Task<ServiceResponse<GetCourseDto>> RemoveInstructorFromCourse(int userId, int courseId);
        Task<ServiceResponse<GetOzgurDto>> GetOzgur(int courseId);
        Task<ServiceResponse<string>> RemoveCourse(int courseId);
        Task<ServiceResponse<GetCourseDto>> ActivateCourse(int courseId);
        Task<ServiceResponse<GetCourseDto>> DeactivateCourse(int courseId);
        Task<ServiceResponse<List<GetCourseDto>>> GetInstructedCoursesOfUser(int userId);
        Task<ServiceResponse<List<GetFeedItemDto>>> GetAssignments(int courseId);
        Task<ServiceResponse<List<UsersOfCourseDto>>> GetUsersOfCourse (int courseId);
        Task<ServiceResponse<string>> LockGroupFormation(int courseId);
        Task<ServiceResponse<GetSectionDto>> RemoveStudentFromCourse(int userId, int courseId);
    }
}