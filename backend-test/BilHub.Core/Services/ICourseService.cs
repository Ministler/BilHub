using System.Collections.Generic;
using System.Threading.Tasks;
using BilHub.Core.Models;

namespace BilHub.Core.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllWithInstructor();
        Task<IEnumerable<Course>> GetCoursesByInstructorId(int instructorId);
        Task UpdateCourse(Course courseToBeUpdated, Course course);
        Task<Course> CreateCourse(Course newCourse);
        Task DeleteCourse(Course course);
        Task<Course> GetCourseById(int id);
    }
}