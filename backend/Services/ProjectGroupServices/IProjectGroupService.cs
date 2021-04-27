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
        
    }
}