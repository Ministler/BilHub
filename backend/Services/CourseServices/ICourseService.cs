using System.Threading.Tasks;
using backend.Dtos.Course;
using backend.Dtos.ProjectGroup;
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
        Task<ServiceResponse<string>> RemoveCourse ( int courseId );
        Task<ServiceResponse<GetCourseDto>> ActivateCourse(int courseId);
        Task<ServiceResponse<GetCourseDto>> DeactivateCourse(int courseId);

    }
}