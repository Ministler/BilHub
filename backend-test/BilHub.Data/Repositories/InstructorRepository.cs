using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilHub.Core.Models;
using BilHub.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BilHub.Data.Repositories
{
    public class InstructorRepository : Repository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Instructor>> GetAllWithCoursesAsync()
        {
            return await BilHubDbContext.Instructors
                .Include(a => a.Courses)
                .ToListAsync();
        }

        public async Task<Instructor> GetWithCoursesByIdAsync(int id)
        {
            return await BilHubDbContext.Instructors
               .Include(a => a.Courses)
               .SingleOrDefaultAsync(a => a.Id == id);
        }

        private BilHubDbContext BilHubDbContext
        {
            get { return Context as BilHubDbContext; }
        }

    }
}