using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Http;

namespace backend.Services.Submission
{
    public interface ISubmissionService
    {
        Task<ServiceResponse<string>> SubmitAssignment(IFormFile file);
    }
}