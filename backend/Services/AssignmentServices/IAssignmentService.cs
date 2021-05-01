using System.Threading.Tasks;
using backend.Dtos.Assignment;
using backend.Dtos.ProjectGroup;
using backend.Models;

namespace backend.Services.AssignmentServices
{
    public interface IAssignmentService
    {
        Task<ServiceResponse<string>> SubmitAssignmentFile(AddAssignmentFileDto file);
        Task<ServiceResponse<string>> DownloadAssignmentFile(GetAssignmentFileDto dto);
        Task<ServiceResponse<string>> DeleteFile(DeleteAssignmentFileDto dto);
        Task<ServiceResponse<GetAssignmentDto>> SubmitAssignment(AddAssignmentDto assignmentDto);
        Task<ServiceResponse<string>> DeleteAssignment(DeleteAssignmentDto dto);
        Task<ServiceResponse<GetAssignmentDto>> GetAssignment(int assignmentId);
        Task<ServiceResponse<GetAssignmentDto>> UpdateAssignment(UpdateAssignmentDto dto);
        Task<ServiceResponse<GetOzgurDto>> GetOzgur(int groupId);
        Task<ServiceResponse<string>> DeleteWithForce(int assignmentId);

        Task<ServiceResponse<string>> DeleteWithForce(int assignmentId);
    }
}