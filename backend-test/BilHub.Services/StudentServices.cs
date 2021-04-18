using System.Collections.Generic;
using System.Threading.Tasks;
using BilHub.Core;
using BilHub.Core.Models;
using BilHub.Core.Services;

namespace BilHub.Services
{
    public class StudentServices : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Student> CreateStudent(Student newStudent)
        {
            await _unitOfWork.students.AddAsync(newStudent);
            await _unitOfWork.CommitAsync();
            return newStudent;
        }

        public async Task DeleteStudent(Student student)
        {
            _unitOfWork.students.Remove(student);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _unitOfWork.students.GetAllWithCourseAsync();
        }

        public async Task<Student> GetStudentById(int id)
        {
            return await _unitOfWork.students.
                GetByIdAsync(id);
        }

        public async Task<IEnumerable<Student>> GetStudentsByCourseId(int courseId)
        {
            return await _unitOfWork.students.
                GetAllWithCourseByCourseIdAsync(courseId);
        }

        public async Task UpdateStudent(Student studentToBeUpdated, Student student)
        {
            studentToBeUpdated.Name = student.Name;
            studentToBeUpdated.CourseId = student.CourseId;
            await _unitOfWork.CommitAsync();
        }
    }
}