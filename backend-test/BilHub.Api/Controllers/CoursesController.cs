using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BilHub.Api.Mapping;
using BilHub.Api.Resources;
using BilHub.Core.Models;
using BilHub.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BilHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        public CoursesController(ICourseService courseService, IMapper mapper)
        {
            _mapper = mapper;
            _courseService = courseService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Instructor>>> GetAllCourses()
        {
            var courses = await _courseService.GetAllWithInstructor();
            var courseResources = _mapper.Map<IEnumerable<Course>, IEnumerable<CourseResource>>(courses);
            return Ok(courseResources);
        }
    }
}