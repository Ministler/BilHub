using System.Threading.Tasks;
using BilHub.Core;
using BilHub.Core.Repositories;
using BilHub.Data.Repositories;

namespace BilHub.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BilHubDbContext _context;
        private CourseRepository _courseRepository;
        public InstructorRepository _instructorRepository { get; set; }
        public StudentRepository _studentRepository { get; set; }

        public UnitOfWork(BilHubDbContext context)
        {
            this._context = context;
        }

        public IStudentRepository students => _studentRepository ?? new StudentRepository(_context);
        public ICourseRepository courses => _courseRepository ?? new CourseRepository(_context);
        public IInstructorRepository instructors => _instructorRepository ?? new InstructorRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}