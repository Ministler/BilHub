using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Dtos.ProjectGroup;
using backend.Models;

namespace backend.Services.ProjectGroupServices
{
    public interface IProjectGroupService
    {
        Task<ServiceResponse<GetProjectGroupDto>> GetProjectGroupById(int id);
        Task<ServiceResponse<GetProjectGroupDto>> UpdateProjectGroupInformation ( UpdateProjectGroupDto updateProjectGroupDto );
        Task<ServiceResponse<GetProjectGroupDto>> ConfirmationOfStudent ( ConfirmationAnswerDto confirmationAnswerDto );
        Task<ServiceResponse<GetProjectGroupDto>> LeaveGroup ( int projectGroupId );
        Task<ServiceResponse<GetProjectGroupDto>> KickStudentFromGroup ( int projectGroupId, int userId );
        Task<ServiceResponse<List<GetProjectGroupDto>>> GetProjectGroupsOfSection (int sectionId);
        Task<ServiceResponse<string>> DeleteProjectGroup ( int projectGroupId );
        Task<ServiceResponse<string>> ForceCancelGroup ( int projectGroupId );
        Task<ServiceResponse<GetProjectGroupDto>> CompleteJoinRequest ( int joinRequestId );
        Task<ServiceResponse<GetProjectGroupDto>> CompleteMergeRequest ( int mergeRequestId );
        
    }
}