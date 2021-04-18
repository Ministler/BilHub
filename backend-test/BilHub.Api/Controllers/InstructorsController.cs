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
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        private readonly IMapper _mapper;
        public InstructorsController(IInstructorService instructorService, IMapper mapper)
        {
            _mapper = mapper;
            _instructorService = instructorService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Instructor>>> GetAllInstructors()
        {
            var instructors = await _instructorService.GetAllInstructors();
            var instructorResources = _mapper.Map<IEnumerable<Instructor>, IEnumerable<InstructorResource>>(instructors);
            return Ok(instructorResources);
        }
    }
}