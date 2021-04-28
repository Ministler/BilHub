using System.Threading.Tasks;
using backend.Dtos.Assignment;
using backend.Models;

namespace backend.Services.AssignmentServices
{
    public interface IAssignmentService
    {
        Task<ServiceResponse<string>> SubmitAssignmentFile(AddAssignmentFileDto file);
        Task<ServiceResponse<string>> DownloadAssignmentFile(GetAssignmentFileDto dto);
        Task<ServiceResponse<string>> DeleteFile(DeleteAssignmentFileDto dto);
        Task<ServiceResponse<string>> SubmitAssignment(AddAssignmentDto assignmentDto);
        Task<ServiceResponse<string>> DeleteAssignment(DeleteAssignmentDto dto);

    }
}