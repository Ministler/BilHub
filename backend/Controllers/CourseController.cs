using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Dtos.Course;
using backend.Dtos.ProjectGroup;
using backend.Models;
using backend.Services.CourseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;

        }

        [HttpPost]
        public async Task<ActionResult> CreateCourse(CreateCourseDto createCourseDto)
        {
            return Ok(await _courseService.CreateCourse(createCourseDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCourse(int id)
        {
            return Ok(await _courseService.GetCourse(id));
        }

        [HttpPut]
        public async Task<ActionResult> EditCourse(EditCourseDto editCourseDto)
        {
            return Ok(await _courseService.EditCourse(editCourseDto));
        }

        [HttpPost("AddInstructorToCourse")]
        public async Task<ActionResult> AddInstructorToCourse(int userId, int courseId)
        {
            return Ok(await _courseService.AddInstructorToCourse(userId, courseId));
        }

        [HttpDelete("RemoveInstructorFromCourse")]
        public async Task<ActionResult> RemoveInstructorFromCourse(int userId, int courseId)
        {
            return Ok(await _courseService.RemoveInstructorFromCourse(userId, courseId));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCourse(int id)
        {
            return Ok(await _courseService.RemoveCourse(id));
        }

        [HttpGet]
        [Route("Statistics/{courseId}")]
        public async Task<IActionResult> GetOzgur(int courseId)
        {
            ServiceResponse<GetOzgurDto> response = await _courseService.GetOzgur(courseId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost("ActivateCourse")]
        public async Task<IActionResult> ActivateCourse(int courseId)
        {
            return Ok(await _courseService.ActivateCourse(courseId));
        }

        [HttpPost("DeactivateCourse")]
        public async Task<IActionResult> DeactivateCourse(int courseId)
        {
            return Ok(await _courseService.DeactivateCourse(courseId));
        }

        [HttpGet("InstructedCoursesOfUser")]
        public async Task<ActionResult> GetInstructedCoursesOfUser(int userId)
        {
            return Ok(await _courseService.GetInstructedCoursesOfUser(userId));
        }

        [HttpGet("Assignments/{courseId}")]
        public async Task<ActionResult> GetAssignments(int courseId)
        {
            return Ok(await _courseService.GetAssignments(courseId));
        }

        [HttpGet("UsersOfCourse")]
        public async Task<ActionResult> GetUsersOfCourse(int courseId)
        {
            return Ok(await _courseService.GetUsersOfCourse(courseId));
        }
    }
}