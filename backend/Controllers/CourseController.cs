using System.Threading.Tasks;
using backend.Dtos.Course;
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
        public async Task<ActionResult> CreateCourse ( CreateCourseDto createCourseDto )
        {
            return Ok ( await _courseService.CreateCourse( createCourseDto ) );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCourse ( int id )
        {
            return Ok ( await _courseService.GetCourse( id ) );
        }

        [HttpPut]
        public async Task<ActionResult> EditCourse ( EditCourseDto editCourseDto )
        {
            return Ok ( await _courseService.EditCourse( editCourseDto ) );
        }

        [HttpPost("AddInstructorToCourse")]
        public async Task<ActionResult> AddInstructorToCourse ( int userId, int courseId )
        {
            return Ok ( await _courseService.AddInstructorToCourse( userId, courseId ) );
        }

        [HttpDelete("RemoveInstructorFromCourse")]
        public async Task<ActionResult> RemoveInstructorFromCourse ( int userId, int courseId )
        {
            return Ok ( await _courseService.RemoveInstructorFromCourse( userId, courseId ) );
        }
    }
}