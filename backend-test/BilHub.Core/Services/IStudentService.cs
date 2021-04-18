using System.Collections.Generic;
using System.Threading.Tasks;
using BilHub.Core.Models;

namespace BilHub.Core.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> GetStudentById(int id);
        Task<Student> CreateStudent(Student newStudent);
        Task UpdateStudent(Student studentToBeUpdated, Student student);
        Task DeleteStudent(Student student);
        Task<IEnumerable<Student>> GetStudentsByCourseId(int courseId);
    }
}