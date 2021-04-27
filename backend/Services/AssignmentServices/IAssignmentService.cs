using System.Threading.Tasks;
using backend.Dtos.Assignment;
using backend.Models;

namespace backend.Services.AssignmentServices
{
    public interface IAssignmentService
    {
        Task<ServiceResponse<string>> SubmitAssignmentFile(AddAssignmentFileDto file);
        Task<ServiceResponse<string>> DownloadCommentFile(GetAssignmentFileDto dto);
        Task<ServiceResponse<string>> DeleteFile(DeleteAssignmentFileDto dto);
    }
}