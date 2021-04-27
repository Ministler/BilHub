using System.Threading.Tasks;
using backend.Dtos.ProjectGroup;
using backend.Services.ProjectGroupServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize]
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

        [HttpPut]
        public async Task<ActionResult> UpdateProjectGroupInformation ( UpdateProjectGroupDto updateProjectGroupDto ) 
        {
            return Ok ( await _projectGroupService.UpdateProjectGroupInformation(updateProjectGroupDto) );
        }

        [HttpPost("ConfirmationState")]
        public async Task<ActionResult> ConfirmationOfStudent ( ConfirmationAnswerDto confirmationAnswerDto ) 
        {
            return Ok ( await _projectGroupService.ConfirmationOfStudent (confirmationAnswerDto) );
        }

        [HttpPost("LeftGroup")]
        public async Task<ActionResult> LeaveGroup ( int projectGroupId ) 
        {
            return Ok ( await _projectGroupService.LeaveGroup(projectGroupId) );
        }
    }
}