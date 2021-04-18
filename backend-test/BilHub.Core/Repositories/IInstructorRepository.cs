using System.Collections.Generic;
using System.Threading.Tasks;
using BilHub.Core.Models;

namespace BilHub.Core.Repositories
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<IEnumerable<Instructor>> GetAllWithCoursesAsync();
        Task<Instructor> GetWithCoursesByIdAsync(int id);
    }
}