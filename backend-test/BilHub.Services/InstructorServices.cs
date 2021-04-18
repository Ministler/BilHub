using System.Collections.Generic;
using System.Threading.Tasks;
using BilHub.Core;
using BilHub.Core.Models;
using BilHub.Core.Services;

namespace BilHub.Services
{
    public class InstructorServices : IInstructorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public InstructorServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Instructor> CreateInstructor(Instructor newInstructor)
        {
            await _unitOfWork.instructors.AddAsync(newInstructor);
            await _unitOfWork.CommitAsync();
            return newInstructor;
        }

        public async Task DeleteInstructor(Instructor instructor)
        {
            _unitOfWork.instructors.Remove(instructor);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Instructor>> GetAllInstructors()
        {
            return await _unitOfWork.instructors.GetAllAsync();
        }

        public async Task<Instructor> GetInstructorById(int id)
        {
            return await _unitOfWork.instructors.GetByIdAsync(id);
        }

        public async Task UpdateInstructor(Instructor instructorToBeUpdated, Instructor instructor)
        {
            instructorToBeUpdated.Name = instructor.Name;
            await _unitOfWork.CommitAsync();
        }
    }
}