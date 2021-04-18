using System.Collections.Generic;
using System.Threading.Tasks;
using BilHub.Core.Models;

namespace BilHub.Core.Services
{
    public interface IInstructorService
    {
        Task<IEnumerable<Instructor>> GetAllInstructors();
        Task<Instructor> GetInstructorById(int id);
        Task<Instructor> CreateInstructor(Instructor newInstructor);
        Task UpdateInstructor(Instructor instructorToBeUpdated, Instructor instructor);
        Task DeleteInstructor(Instructor instructor);
    }
}