
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

        public async Task<ServiceResponse<string>> DownloadAllCommentFiles(GetAllCommentFilesDto dto)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<string>> DownloadCommentFile(GetCommentFileDto dto)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<string>> SubmitCommentFile(AddCommentFileDto file)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.ProjectGroups).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == file.SubmissionId);
            Course course = _context.Courses.Include(c => c.Instructors)
                .FirstOrDefault(c => c.Id == submission.CourseId);

            if (submission == null || course == null || (user.UserType == UserTypeClass.Student &&
                course.Instructors.FirstOrDefault(i => i.UserId == GetUserId()) == null))
            {
                response.Data = "Not allowed";
                response.Message = "You are not allowed to post comment for this group";
                response.Success = false;
                return response;
            }
            Comment comment = submission.Comments.FirstOrDefault(c => c.Id == file.CommentId);
            if (comment == null || comment.Id != file.CommentId || !comment.FileAttachmentAvailability)
            {
                response.Data = "Bad Request";
                response.Message = "There is no comment to attach any file or you are not authorized";
                response.Success = false;
                return response;
            }
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}",
                "StaticFiles/Feedbacks", submission.CourseId,
                submission.SectionId, file.SubmissionId));
            Directory.CreateDirectory(target);
            if (file.CommentFile.Length <= 0) response.Success = false;
            else
            {
                var filePath = Path.Combine(target, file.CommentFile.FileName);
                submission.FilePath = filePath;
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
    }
}