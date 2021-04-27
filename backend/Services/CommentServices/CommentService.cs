
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.Comment;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        public CommentService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        // public async Task<ServiceResponse<string>> DownloadAllInstructorCommentFiles(GetAllCommentFilesDto dto)
        // {
        //     ServiceResponse<string> response = new ServiceResponse<string>();
        //     User user = await _context.Users.Include(u => u.ProjectGroups).FirstOrDefaultAsync(u => u.Id == GetUserId());
        //     Submission submission = await _context.Submissions.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == dto.SubmissionId);
        //     if (submission == null)
        //     {
        //         response.Data = null;
        //         response.Message = "There is no project group under this Id";
        //         response.Success = false;
        //         return response;
        //     }
        //     if (user.ProjectGroups.FirstOrDefault(pgu => pgu.ProjectGroupId == submission.AffiliatedGroupId) == null && user.UserType == UserTypeClass.Student)
        //     {
        //         response.Data = null;
        //         response.Message = "You are not authorized for this endpoint";
        //         response.Success = false;
        //         return response;
        //     }
        //     if (submission.Comments.FirstOrDefault(cm => cm.CommentedSubmissionId == dto.SubmissionId) == null)
        //     {
        //         response.Data = null;
        //         response.Message = "There is no comment for this group yet";
        //         response.Success = false;
        //         return response;
        //     }
        //     response.Data = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}",
        //         "StaticFiles/Feedbacks", submission.CourseId, submission.SectionId, dto.SubmissionId));
        //     return response;
        // }

        public async Task<ServiceResponse<string>> DownloadAllCommentFiles(GetAllCommentFilesDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.ProjectGroups).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == dto.SubmissionId);
            if (submission == null)
            {
                response.Data = null;
                response.Message = "There is no project group under this Id";
                response.Success = false;
                return response;
            }
            if (user.ProjectGroups.FirstOrDefault(pgu => pgu.ProjectGroupId == submission.AffiliatedGroupId) == null && user.UserType == UserTypeClass.Student)
            {
                response.Data = null;
                response.Message = "You are not authorized for this endpoint";
                response.Success = false;
                return response;
            }
            if (submission.Comments.FirstOrDefault(cm => cm.CommentedSubmissionId == dto.SubmissionId) == null)
            {
                response.Data = null;
                response.Message = "There is no comment for this group yet";
                response.Success = false;
                return response;
            }
            response.Data = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}",
                "StaticFiles/Feedbacks", submission.CourseId, submission.SectionId, dto.SubmissionId));
            return response;
        }

        public async Task<ServiceResponse<string>> DownloadCommentFile(GetCommentFileDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.ProjectGroups).ThenInclude(pgu => pgu.ProjectGroup)
                .ThenInclude(pg => pg.Submissions).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Comment comment = await _context.Comments.FirstOrDefaultAsync(s => s.Id == dto.CommentId);
            if (comment == null)
            {
                response.Data = null;
                response.Message = "There is no comment with this Id";
                response.Success = false;
                return response;
            }
            if (user.ProjectGroups.FirstOrDefault(pgu => pgu.ProjectGroup.Submissions.Any(s => s.Id == comment.CommentedSubmissionId)) == null && user.UserType == UserTypeClass.Student)
            {
                response.Data = null;
                response.Message = "You are not authorized for this endpoint";
                response.Success = false;
                return response;
            }
            if (!comment.FileAttachmentAvailability)
            {
                response.Data = null;
                response.Message = "This user has not yet submitted his comment as a file for this group yet";
                response.Success = false;
                return response;
            }
            response.Data = comment.FilePath;
            return response;
        }

        public async Task<ServiceResponse<string>> SubmitCommentFile(AddCommentFileDto file)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.ProjectGroups).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == file.SubmissionId);
            if (submission == null)
            {
                response.Data = "No submission";
                response.Message = "There is no submission under this name";
                response.Success = false;
                return response;
            }
            Course course = _context.Courses.Include(c => c.Instructors)
                .FirstOrDefault(c => c.Id == submission.CourseId);

            if (course == null || course.Instructors.FirstOrDefault(i => i.UserId == GetUserId()) == null)
            {
                response.Data = "Not allowed";
                response.Message = "You are not allowed to post file for this comment";
                response.Success = false;
                return response;
            }
            Comment comment = submission.Comments.FirstOrDefault(c => c.Id == file.CommentId);
            if (comment == null || comment.Id != file.CommentId || file.CommentFile == null)
            {
                response.Data = "Bad Request";
                response.Message = "There is no comment to attach any file or you are not authorized";
                response.Success = false;
                return response;
            }
            if (Path.GetExtension(file.CommentFile.FileName) != ".pdf")
            {
                response.Data = "Bad Request";
                response.Message = "Comments should be pdf";
                response.Success = false;
                return response;
            }
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/Feedbacks", submission.CourseId,
                submission.SectionId, file.SubmissionId, user.Id));
            Directory.CreateDirectory(target);
            if (file.CommentFile.Length <= 0) response.Success = false;
            else
            {
                string oldfile = Directory.GetFiles(target).FirstOrDefault();
                if (File.Exists(oldfile))
                {
                    File.Delete(oldfile);
                }
                var filePath = Path.Combine(target, user.Name.Trim().Replace(" ", "_") + "_Feedback.pdf");
                comment.FilePath = filePath;
                comment.FileAttachmentAvailability = true;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CommentFile.CopyToAsync(stream);
                }
                response.Data = target;
                response.Message = "file succesfully saved.";
                comment.CreatedAt = DateTime.Now;
            }
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<ServiceResponse<string>> DeleteFile(DeleteCommentFileDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Comment comment = await _context.Comments.Include(c => c.CommentedSubmission).FirstOrDefaultAsync(s => s.Id == dto.CommentId);
            if (comment == null)
            {
                response.Data = null;
                response.Message = "There is no comment with this Id";
                response.Success = false;
                return response;
            }
            if (comment.CommentedUserId != GetUserId())
            {
                response.Data = null;
                response.Message = "You are not authorized for this endpoint";
                response.Success = false;
                return response;
            }
            if (!comment.FileAttachmentAvailability)
            {
                response.Data = null;
                response.Message = "This user has not yet submitted his comment as a file for this group yet";
                response.Success = false;
                return response;
            }

            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/Feedbacks", comment.CommentedSubmission.CourseId,
                comment.CommentedSubmission.SectionId, comment.CommentedSubmissionId, user.Id));
            Directory.CreateDirectory(target);
            var filePath = Path.Combine(target, user.Name.Trim().Replace(" ", "_") + "_Feedback.pdf");
            comment.FilePath = null;
            comment.FileAttachmentAvailability = false;
            File.Delete(filePath);
            response.Data = target;
            response.Message = "file succesfully deleted.";
            comment.CreatedAt = DateTime.MinValue;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return response;
        }
    }
}