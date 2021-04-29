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

        [HttpGet("ProjectGroupsOfSection")]
        public async Task<ActionResult> GetProjectGroupsOfSection ( int sectionId ) 
        {
            return Ok ( await _projectGroupService.GetProjectGroupsOfSection (sectionId) );
        }

        [HttpDelete("SilmeDeneme")]
        public async Task<ActionResult>  DeleteProjectGroup ( int projectGroupId )
        {
            return Ok ( await _projectGroupService.DeleteProjectGroup( projectGroupId ) );
        }

        [HttpPost("ForceCancelProjectGroup")]
        public async Task<ActionResult>  ForceCancelProjectGroup ( int projectGroupId )
        {
            return Ok ( await _projectGroupService.ForceCancelGroup ( projectGroupId ) );
        }

        [HttpPost("KickStudentFromGroup")]
        public async Task<ActionResult> KickStudentFromGroup ( int projectGroupId, int userId )
        {  
            return Ok ( await _projectGroupService.KickStudentFromGroup ( projectGroupId, userId ) );
        }

        [HttpPost("CompleteJoinRequest")]
        public async Task<ActionResult> CompleteJoinRequest ( int joinRequestId )
        {  
            return Ok ( await _projectGroupService.CompleteJoinRequest ( joinRequestId ) );
        }

        [HttpPost("CompleteMergeRequest")]
        public async Task<ActionResult> CompleteMergeRequest ( int mergeRequestId )
        {  
            return Ok ( await _projectGroupService.CompleteMergeRequest ( mergeRequestId ) );
        }
    }
}