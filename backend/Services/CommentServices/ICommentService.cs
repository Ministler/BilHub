using System.Threading.Tasks;
using backend.Dtos.Comment;
using backend.Models;

namespace backend.Services.CommentServices
{
    public interface ICommentService
    {
        Task<ServiceResponse<string>> SubmitCommentFile(AddCommentFileDto file);
        Task<ServiceResponse<string>> DownloadCommentFile(GetCommentFileDto dto);
        Task<ServiceResponse<string>> DownloadAllCommentFiles(GetAllCommentFilesDto dto);
        Task<ServiceResponse<string>> DeleteFile(DeleteCommentFileDto dto);
        Task<ServiceResponse<string>> DeleteWithForce(int commentId);
        Task<ServiceResponse<string>> Delete(int commentId);
        Task<ServiceResponse<GetCommentDto>> Add(AddCommentDto addCommentDto);
        Task<ServiceResponse<GetCommentDto>> Update(UpdateCommentDto updateCommentDto);
        Task<ServiceResponse<GetCommentDto>> Get(int commentId);

    }
}