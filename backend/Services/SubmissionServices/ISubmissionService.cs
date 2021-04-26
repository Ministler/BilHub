using System.Threading.Tasks;
using backend.Models;
using backend.Dtos.Submission;
using Microsoft.AspNetCore.Http;

namespace backend.Services.SubmissionServices
{
    public interface ISubmissionService
    {
        Task<ServiceResponse<string>> SubmitAssignment(AddSubmissionFileDto file);
        Task<ServiceResponse<string>> DownloadSubmission(GetSubmissionFileDto dto);
        Task<ServiceResponse<string>> DownloadAllSubmissions(GetSubmissionsFileDto dto);
        Task<ServiceResponse<string>> DeleteSubmssion(DeleteSubmissionFIleDto dto);
    }
}