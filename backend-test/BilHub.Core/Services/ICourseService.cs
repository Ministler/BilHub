using System.Collections.Generic;
using System.Threading.Tasks;
using BilHub.Core.Models;

namespace BilHub.Core.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllWithInstructor();
        Task<Course> GetCourseById(int id);
        Task<IEnumerable<Course>> GetCoursesByInstructorId(int instructorId);
        Task<Course> CreateCourse(Course newCourse);
        Task UpdateCourse(Course courseToBeUpdated, Course course);
        Task DeleteCourse(Course course);
    }
}