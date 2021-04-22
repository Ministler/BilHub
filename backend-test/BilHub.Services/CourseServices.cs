using System.Collections.Generic;
using System.Threading.Tasks;
using BilHub.Core;
using BilHub.Core.Models;
using BilHub.Core.Services;

namespace BilHub.Services
{
    public class CourseServices : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Course> CreateCourse(Course newCourse)
        {
            await _unitOfWork.courses.AddAsync(newCourse);
            await _unitOfWork.CommitAsync();
            return newCourse;
        }

        public async Task DeleteCourse(Course course)
        {
            _unitOfWork.courses.Remove(course);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Course>> GetAllWithInstructor()
        {
            return await _unitOfWork.courses.GetAllWithInstructorAsync();
        }

        public async Task<Course> GetCourseById(int id)
        {
            return await _unitOfWork.courses.
                GetByIdAsync(id);
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorId(int instructorId)
        {
            return await _unitOfWork.courses.
                GetAllWithInstructorByInstructorIdAsync(instructorId);
        }

        public async Task UpdateCourse(Course courseToBeUpdated, Course course)
        {
            courseToBeUpdated.Name = course.Name;
            courseToBeUpdated.Instructors = course.Instructors;
            courseToBeUpdated.Id = course.Id;
            await _unitOfWork.CommitAsync();
        }
    }
}