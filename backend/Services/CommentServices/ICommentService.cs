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

    }
}