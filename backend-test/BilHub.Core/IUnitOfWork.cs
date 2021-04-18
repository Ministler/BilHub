using System;
using System.Threading.Tasks;
using BilHub.Core.Repositories;

namespace BilHub.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository students { get; }
        ICourseRepository courses { get; }
        IInstructorRepository instructors { get; }
        Task<int> CommitAsync();
    }
}