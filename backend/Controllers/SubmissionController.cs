using System.Threading.Tasks;
using backend.Models;
using backend.Services.Submission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }
        [HttpPost]
        public async Task<IActionResult> Submit(IFormFile file)
        {
            ServiceResponse<string> response = await _submissionService.SubmitAssignment(file);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);

        }
    }
}