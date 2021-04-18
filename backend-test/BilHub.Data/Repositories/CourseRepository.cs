using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilHub.Core.Models;
using BilHub.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BilHub.Data.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetAllWithInstructorAsync()
        {
            return await BilHubDbContext.Courses
                .Include(m => m.Instructor)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetAllWithInstructorByInstructorIdAsync(int instructorId)
        {
            return await BilHubDbContext.Courses
                .Include(m => m.Instructor)
                .Where(m => m.InstructorId == instructorId)
                .ToListAsync();
        }

        public async Task<Course> GetAllWithCoursesAsync(int id)
        {
            return await BilHubDbContext.Courses
                .Include(m => m.Instructor)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Course> GetWithInstructorByIdAsync(int id)
        {
            return await BilHubDbContext.Courses
            .Include(m => m.Instructor)
            .SingleOrDefaultAsync(m => m.Id == id);
        }

        private BilHubDbContext BilHubDbContext
        {
            get { return Context as BilHubDbContext; }
        }
    }
}