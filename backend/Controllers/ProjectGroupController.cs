using System.Threading.Tasks;
using backend.Services.ProjectGroupServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    // surayi geri ac!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProjectGroupController : ControllerBase
    {
        private readonly IProjectGroupService _projectGroupService;
        public ProjectGroupController(IProjectGroupService projectGroupService)
        {
            _projectGroupService = projectGroupService;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectGroupById ( int id ) 
        {
            return Ok( await _projectGroupService.GetProjectGroupById(id) );
        }
    }
}