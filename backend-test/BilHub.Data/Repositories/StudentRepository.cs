using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilHub.Core.Models;
using BilHub.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BilHub.Data.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Student>> GetAllWithCourseAsync()
        {
            return await BilHubDbContext.Students
            .Include(s => s.Course)
            .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetAllWithCourseByCourseIdAsync(int courseId)
        {
            return await BilHubDbContext.Students
                .Include(s => s.Course)
                .Where(s => s.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<Student> GetWithCourseByIdAsync(int id)
        {
            return await BilHubDbContext.Students
                .Include(s => s.CourseId)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        private BilHubDbContext BilHubDbContext
        {
            get { return Context as BilHubDbContext; }
        }

    }
}