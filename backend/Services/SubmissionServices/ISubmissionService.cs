using System.Threading.Tasks;
using backend.Models;
using backend.Dtos.Submission;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using backend.Dtos.Comment;
using backend.Dtos.Comment.FeedbackItems;

namespace backend.Services.SubmissionServices
{
    public interface ISubmissionService
    {
        Task<ServiceResponse<string>> SubmitAssignment(AddSubmissionFileDto file);
        Task<ServiceResponse<string>> DownloadSubmission(GetSubmissionFileDto dto);
        Task<ServiceResponse<string>> DownloadAllSubmissions(GetSubmissionsFileDto dto);
        Task<ServiceResponse<IEnumerable<string>>> DownloadNotGradedSubmissions(GetUngradedSubmissionFilesDto dto);
        Task<ServiceResponse<string>> DeleteSubmissionFile(DeleteSubmissionFileDto dto);
        Task<ServiceResponse<string>> DeleteWithForce(int submissionId);
        Task<ServiceResponse<string>> Delete(int submissionId);
        Task<ServiceResponse<GetSubmissionDto>> AddSubmission(AddSubmissionDto addSubmissionDto);
        Task<ServiceResponse<GetSubmissionDto>> GetSubmission(int submissionId);
        Task<ServiceResponse<GetSubmissionDto>> UpdateSubmission(UpdateSubmissionDto submissionDto);
        // Task<ServiceResponse<IEnumerable<GetCommentDto>>> GetInstructorComments(int submissionId);
        Task<ServiceResponse<List<GetCommentDto>>> GetInstructorComments(int submissionId);
        Task<ServiceResponse<List<GetCommentDto>>> GetTaComments(int submissionId);
        Task<ServiceResponse<List<GetCommentDto>>> GetStudentComments(int submissionId);
        Task<ServiceResponse<string>> EnterSrsGrade(decimal srsGrade, int submissionId);
        Task<ServiceResponse<string>> DeleteSrsGrade(int submissionId);
        Task<ServiceResponse<decimal>> GetSrsGrade(int submissionId);
        Task<ServiceResponse<decimal>> GetGradeWithGraderId(int submissionId, int graderId);
        Task<ServiceResponse<FeedbacksDto>> GetNewFeedbacks();
    }
}