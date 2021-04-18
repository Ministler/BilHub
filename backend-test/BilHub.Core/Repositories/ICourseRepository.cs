using System.Collections.Generic;
using System.Threading.Tasks;
using BilHub.Core.Models;

namespace BilHub.Core.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetAllWithInstructorAsync();
        Task<Course> GetWithInstructorByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllWithInstructorByInstructorIdAsync(int instructorId);
    }
}