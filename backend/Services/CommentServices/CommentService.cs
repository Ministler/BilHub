
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.Comment;
using backend.Dtos.Comment.FeedbackItems;
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
            response.Data = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}",
                "StaticFiles/Feedbacks", submission.CourseId, dto.SubmissionId));
            return response;
        }

        public async Task<ServiceResponse<string>> DownloadCommentFile(GetCommentFileDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.ProjectGroups).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Comment comment = await _context.Comments.Include(c => c.CommentedSubmission).FirstOrDefaultAsync(s => s.Id == dto.CommentId);
            if (comment == null)
            {
                response.Data = null;
                response.Message = "There is no comment with this Id";
                response.Success = false;
                return response;
            }
            Submission submission = comment.CommentedSubmission;
            Assignment assignment = await _context.Assignments.Include(a => a.AfilliatedCourse).ThenInclude(c => c.Instructors)
                .FirstOrDefaultAsync(a => a.Id == submission.AffiliatedAssignmentId);
            if (!assignment.VisibilityOfSubmission)
            {
                if (!user.ProjectGroups.Any(pgu => pgu.UserId == submission.AffiliatedGroupId) &&
                    !assignment.AfilliatedCourse.Instructors.Any(cu => cu.UserId == GetUserId()) && user.UserType == UserTypeClass.Student)
                {
                    response.Data = null;
                    response.Message = "You are not authorized for this endpoint";
                    response.Success = false;
                    return response;
                }
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
            Comment comment = await _context.Comments.Include(u => u.CommentedSubmission).FirstOrDefaultAsync(c => c.Id == file.CommentId);
            if (comment == null)
            {
                response.Data = "No comment";
                response.Message = "There is no comment under this id";
                response.Success = false;
                return response;
            }
            if (comment.CommentedUserId != user.Id || file.CommentFile == null)
            {
                response.Data = "Bad Request";
                response.Message = "There is no file to attach or you are not authorized";
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
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/",
                "StaticFiles/Feedbacks", comment.CommentedSubmission.CourseId, comment.CommentedSubmissionId, user.Id));
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

        public async Task<ServiceResponse<string>> DeleteWithForce(int commentId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Comment comment = await _context.Comments.Include(c => c.CommentedSubmission).FirstOrDefaultAsync(s => s.Id == commentId);
            if (comment == null)
            {
                response.Data = null;
                response.Message = "There is no comment with this Id";
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

            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}",
                "StaticFiles/Feedbacks", comment.CommentedSubmission.CourseId, comment.CommentedSubmissionId, user.Id));
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

        public async Task<ServiceResponse<string>> Delete(int commentId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Comment comment = await _context.Comments.Include(c => c.CommentedSubmission).FirstOrDefaultAsync(s => s.Id == commentId);
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

            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/Feedbacks", comment.CommentedSubmission.CourseId,
                comment.CommentedSubmission.SectionId, comment.CommentedSubmissionId, user.Id));
            Directory.CreateDirectory(target);
            var filePath = Path.Combine(target, user.Name.Trim().Replace(" ", "_") + "_Feedback.pdf");
            File.Delete(filePath);
            response.Data = target;
            response.Message = "Comment succesfully deleted.";
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<GetCommentDto>> Add(AddCommentDto addCommentDto)
        {
            ServiceResponse<GetCommentDto> response = new ServiceResponse<GetCommentDto>();
            if (addCommentDto == null)
            {
                response.Data = null;
                response.Message = "There is no data";
                response.Success = false;
                return response;
            }
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions.Include(s => s.AffiliatedAssignment)
                .Include(s => s.Comments)
                .FirstOrDefaultAsync(s => s.Id == addCommentDto.CommentedSubmissionId);
            if (submission == null)
            {
                response.Data = null;
                response.Message = "There is no comment with this Id";
                response.Success = false;
                return response;
            }
            if (submission.Comments.Any(c => c.CommentedUserId == GetUserId()))
            {
                response.Data = null;
                response.Message = "You already commented on this submission";
                response.Success = false;
                return response;
            }
            //user comment
            Course course = await _context.Courses.Include(c => c.Instructors).FirstOrDefaultAsync(c => c.Id == submission.AffiliatedAssignment.AfilliatedCourseId);
            if (course == null)
            {
                response.Data = null;
                response.Message = "There is a mistake about course";
                response.Success = false;
                return response;
            }
            //&& user.UserType == UserTypeClass.Student && submission.AffiliatedAssignment
            if (!course.Instructors.Any(cu => cu.UserId == user.Id) && !submission.AffiliatedAssignment.CanBeGradedByStudents)
            {
                response.Data = null;
                response.Message = "You are not authorized to make comment";
                response.Success = false;
                return response;
            }
            Comment comment = new Comment
            {
                CommentedSubmissionId = addCommentDto.CommentedSubmissionId,
                CommentText = addCommentDto.CommentText,
                CreatedAt = DateTime.Now,
                CommentedUserId = user.Id,
                FilePath = "",
                Grade = addCommentDto.Grade,
                MaxGrade = addCommentDto.MaxGrade,
                FileAttachmentAvailability = false
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCommentDto>(comment);
            response.Data.FileEndpoint = "Comment/File/" + comment.Id;
            response.Data.FileName = comment.FileAttachmentAvailability ? comment.FilePath.Split('/').Last() : "";
            return response;
        }

        public async Task<ServiceResponse<GetCommentDto>> Update(UpdateCommentDto updateCommentDto)
        {
            ServiceResponse<GetCommentDto> response = new ServiceResponse<GetCommentDto>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Comment comment = await _context.Comments.Include(c => c.CommentedUser).FirstOrDefaultAsync(s => s.Id == updateCommentDto.CommentId);
            if (user.Id != comment.CommentedUserId)
            {
                response.Data = null;
                response.Message = "You are not authorized to make comment";
                response.Success = false;
                return response;
            }
            comment.CommentText = updateCommentDto.CommentText;
            comment.MaxGrade = updateCommentDto.MaxGrade;
            comment.Grade = updateCommentDto.Grade;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCommentDto>(comment);
            response.Data.FileEndpoint = "Comment/File/" + comment.Id;
            response.Data.HasFile = comment.FileAttachmentAvailability;
            response.Data.FileName = comment.FileAttachmentAvailability ? comment.FilePath.Split('/').Last() : "";
            return response;

        }

        public async Task<ServiceResponse<GetCommentDto>> Get(int commentId)
        {
            ServiceResponse<GetCommentDto> response = new ServiceResponse<GetCommentDto>();
            User user = await _context.Users.Include(u => u.ProjectGroups).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Comment comment = await _context.Comments.Include(c => c.CommentedUser).Include(c => c.CommentedSubmission).FirstOrDefaultAsync(s => s.Id == commentId);
            if (comment == null)
            {
                response.Data = null;
                response.Message = "There is no comment with this Id";
                response.Success = false;
                return response;
            }

            Submission submission = comment.CommentedSubmission;
            Assignment assignment = await _context.Assignments.Include(a => a.AfilliatedCourse).ThenInclude(c => c.Instructors)
                .FirstOrDefaultAsync(a => a.Id == submission.AffiliatedAssignmentId);
            if (!assignment.VisibilityOfSubmission)
            {
                if (!user.ProjectGroups.Any(pgu => pgu.UserId == submission.AffiliatedGroupId) &&
                    !assignment.AfilliatedCourse.Instructors.Any(cu => cu.UserId == GetUserId()) && user.UserType == UserTypeClass.Student)
                {
                    response.Data = null;
                    response.Message = "You are not authorized for this endpoint";
                    response.Success = false;
                    return response;
                }
            }
            response.Data = _mapper.Map<GetCommentDto>(comment);
            response.Data.FileEndpoint = "Comment/File/" + comment.Id;
            response.Data.HasFile = comment.FileAttachmentAvailability;
            response.Data.FileName = comment.FileAttachmentAvailability ? comment.FilePath.Split('/').Last() : "";
            return response;

        }
    }
}