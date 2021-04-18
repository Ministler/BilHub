using System.Collections.Generic;
using System.Threading.Tasks;
using BilHub.Core.Models;

namespace BilHub.Core.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> GetAllWithCourseAsync();
        Task<Student> GetWithCourseByIdAsync(int id);
        Task<IEnumerable<Student>> GetAllWithCourseByCourseIdAsync(int courseId);
    }
}