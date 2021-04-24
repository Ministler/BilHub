using System.Threading.Tasks;
using BilHub.Dtos.Submission;
using BilHub.Models;
using Microsoft.AspNetCore.Http;

namespace BilHub.Services.Submission
{
    public interface ISubmissionService
    {
        Task<ServiceResponse<string>> SubmitAssignment(IFormFile file);
    }
}