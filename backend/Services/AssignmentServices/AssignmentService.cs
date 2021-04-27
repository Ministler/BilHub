using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.Assignment;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.AssignmentServices
{
    public class AssignmentService : IAssignmentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        public AssignmentService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public Task<ServiceResponse<string>> DeleteFile(DeleteAssignmentFileDto dto)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceResponse<string>> DownloadCommentFile(GetAssignmentFileDto dto)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<string>> SubmitAssignmentFile(AddAssignmentFileDto file)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.InstructedCourses).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Assignment assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == file.AssignmentId);
            Course course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == assignment.CourseId);
            Section section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == file.SectionId);
            if (assignment == null || file.File == null || course == null || section == null)
            {
                response.Data = "No submission";
                response.Message = "There is no submission under this name";
                response.Success = false;
                return response;
            }
            if (course.Instructors.FirstOrDefault(c => c.UserId == user.Id) == null)
            {
                response.Data = "Not allowed";
                response.Message = "You are not allowed to post file for this assignment";
                response.Success = false;
                return response;
            }
            if (Path.GetExtension(file.File.FileName) != ".pdf")
            {
                response.Data = "Bad Request";
                response.Message = "Assignments should be pdf";
                response.Success = false;
                return response;
            }
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}",
                "StaticFiles/Assignments", assignment.CourseId,
                assignment.AffiliatedSectionId, assignment.Id));
            Directory.CreateDirectory(target);
            if (file.File.Length <= 0) response.Success = false;
            else
            {
                string oldfile = Directory.GetFiles(target).FirstOrDefault();
                if (File.Exists(oldfile))
                {
                    File.Delete(oldfile);
                }
                var filePath = Path.Combine(target, string.Format("Course{0}_Section{1}_Assignment{2}",
                assignment.CourseId,
                assignment.AffiliatedSectionId,
                assignment.Id) + ".pdf");
                assignment.FilePath = filePath;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.File.CopyToAsync(stream);
                }
                response.Data = target;
                response.Message = "file succesfully saved.";
                assignment.CreatedAt = DateTime.Now;
            }
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();

            return response;
        }

        // public async Task<ServiceResponse<string>> DownloadCommentFile(GetCommentFileDto dto)
        // {
        //     ServiceResponse<string> response = new ServiceResponse<string>();
        //     User user = await _context.Users.Include(u => u.ProjectGroups).ThenInclude(pgu => pgu.ProjectGroup)
        //         .ThenInclude(pg => pg.Submissions).FirstOrDefaultAsync(u => u.Id == GetUserId());
        //     Comment comment = await _context.Comments.FirstOrDefaultAsync(s => s.Id == dto.CommentId);
        //     if (comment == null)
        //     {
        //         response.Data = null;
        //         response.Message = "There is no comment with this Id";
        //         response.Success = false;
        //         return response;
        //     }
        //     if (user.ProjectGroups.FirstOrDefault(pgu => pgu.ProjectGroup.Submissions.Any(s => s.Id == comment.CommentedSubmissionId)) == null && user.UserType == UserTypeClass.Student)
        //     {
        //         response.Data = null;
        //         response.Message = "You are not authorized for this endpoint";
        //         response.Success = false;
        //         return response;
        //     }
        //     if (!comment.FileAttachmentAvailability)
        //     {
        //         response.Data = null;
        //         response.Message = "This user has not yet submitted his comment as a file for this group yet";
        //         response.Success = false;
        //         return response;
        //     }
        //     response.Data = comment.FilePath;
        //     return response;
        // }


        // public async Task<ServiceResponse<string>> DeleteFile(DeleteCommentFileDto dto)
        // {
        //     ServiceResponse<string> response = new ServiceResponse<string>();
        //     User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        //     Comment comment = await _context.Comments.Include(c => c.CommentedSubmission).FirstOrDefaultAsync(s => s.Id == dto.CommentId);
        //     if (comment == null)
        //     {
        //         response.Data = null;
        //         response.Message = "There is no comment with this Id";
        //         response.Success = false;
        //         return response;
        //     }
        //     if (comment.CommentedUserId != GetUserId())
        //     {
        //         response.Data = null;
        //         response.Message = "You are not authorized for this endpoint";
        //         response.Success = false;
        //         return response;
        //     }
        //     if (!comment.FileAttachmentAvailability)
        //     {
        //         response.Data = null;
        //         response.Message = "This user has not yet submitted his comment as a file for this group yet";
        //         response.Success = false;
        //         return response;
        //     }

        //     var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
        //         "StaticFiles/Feedbacks", comment.CommentedSubmission.CourseId,
        //         comment.CommentedSubmission.SectionId, comment.CommentedSubmissionId, user.Id));
        //     Directory.CreateDirectory(target);
        //     var filePath = Path.Combine(target, user.Name.Trim().Replace(" ", "_") + "_Feedback.pdf");
        //     comment.FilePath = null;
        //     comment.FileAttachmentAvailability = false;
        //     File.Delete(filePath);
        //     response.Data = target;
        //     response.Message = "file succesfully deleted.";
        //     comment.CreatedAt = DateTime.MinValue;
        //     _context.Comments.Update(comment);
        //     await _context.SaveChangesAsync();
        //     return response;
        // }
    }
}