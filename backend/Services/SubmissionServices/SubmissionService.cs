using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Models;
using backend.Dtos.Submission;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace backend.Services.SubmissionServices
{
    public class SubmissionService : ISubmissionService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        public SubmissionService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<string>> DownloadAllSubmissions(GetSubmissionsFileDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.ProjectGroups).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Course course = await _context.Courses.Include(u => u.Instructors).FirstOrDefaultAsync(c => c.Id == dto.CourseId);
            if (course == null)
            {
                response.Data = null;
                response.Message = "There is no course under this Id";
                response.Success = false;
                return response;
            }
            if (course.Instructors.FirstOrDefault(i => i.UserId == user.Id) == null && user.UserType == UserTypeClass.Student)
            {
                response.Data = null;
                response.Message = "You are not authorized for this endpoint";
                response.Success = false;
                return response;
            }
            if (dto.SectionId == -1)
                response.Data = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}",
                "StaticFiles/Submissions", dto.CourseId));
            else if (dto.AssignmentId == -1)
                response.Data = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/",
                "StaticFiles/Submissions", dto.CourseId, dto.SectionId));
            else
                response.Data = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}",
                "StaticFiles/Submissions", dto.CourseId, dto.SectionId, dto.AssignmentId));
            return response;
        }
        public async Task<ServiceResponse<string>> DownloadSubmission(GetSubmissionFileDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            Submission submission = await _context.Submissions.Include(s => s.AffiliatedAssignment).FirstOrDefaultAsync(s => s.Id == dto.SubmissionId);
            if (submission == null)
            {
                response.Data = null;
                response.Message = "There is no such submission";
                response.Success = false;
                return response;
            }
            if (!submission.HasSubmission)
            {
                response.Data = null;
                response.Message = "This group has not yet submitted their file for this assigment";
                response.Success = false;
                return response;
            }
            User user = await _context.Users.Include(u => u.ProjectGroups).ThenInclude(pgu => pgu.ProjectGroup).FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = user.ProjectGroups.FirstOrDefault(pgu => pgu.ProjectGroupId == submission.AffiliatedGroupId).ProjectGroup;
            if (!submission.AffiliatedAssignment.VisibilityOfSubmission && projectGroup == null && user.UserType == UserTypeClass.Student)
            {
                response.Data = null;
                response.Message = "You are not authorized to see this submission";
                response.Success = false;
                return response;
            }
            response.Data = submission.FilePath;
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<string>> SubmitAssignment(AddSubmissionFileDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            Submission submission = await _context.Submissions.FirstOrDefaultAsync(s => s.Id == dto.SubmissionId);
            if (submission == null)
            {
                response.Data = "Bad Request";
                response.Message = "There is no submission with this Id";
                response.Success = false;
                return response;
            }
            User user = await _context.Users.Include(u => u.ProjectGroups).FirstOrDefaultAsync(ss => ss.Id == GetUserId());
            if (!user.ProjectGroups.Any(pgu => pgu.ProjectGroupId == submission.AffiliatedGroupId))
            {
                response.Data = "Not allowed";
                response.Message = "You are not allowed to submit file to this assignment";
                response.Success = false;
                return response;
            }
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/Submissions", submission.CourseId,
                submission.SectionId, submission.AffiliatedAssignmentId, submission.AffiliatedGroupId));

            Directory.CreateDirectory(target);
            if (dto.File.Length <= 0) response.Success = false;
            else
            {
                string oldfile = Directory.GetFiles(target).FirstOrDefault();
                string extension = Path.GetExtension(dto.File.FileName);
                var filePath = Path.Combine(target, string.Format("Course{0}-Section{1}-Group{2}-Assignment{3}"
                    , submission.CourseId, submission.SectionId, submission.AffiliatedGroupId, submission.AffiliatedAssignmentId) + extension);
                submission.FilePath = filePath;
                submission.UpdatedAt = DateTime.Now;
                submission.HasSubmission = true;
                if (File.Exists(oldfile))
                {
                    File.Delete(oldfile);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }
                response.Data = target;
                response.Message = "file succesfully saved.";
                _context.Submissions.Update(submission);
                await _context.SaveChangesAsync();
            }

            return response;
        }

        public async Task<ServiceResponse<string>> DeleteSubmssion(DeleteSubmissionFIleDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            Submission submission = await _context.Submissions.FirstOrDefaultAsync(s => s.Id == dto.SubmissionId);
            if (submission == null)
            {
                response.Data = "Bad Request";
                response.Message = "There is no submission with this Id";
                response.Success = false;
                return response;
            }
            User user = await _context.Users.Include(u => u.ProjectGroups).FirstOrDefaultAsync(ss => ss.Id == GetUserId());
            if (!user.ProjectGroups.Any(pgu => pgu.ProjectGroupId == submission.AffiliatedGroupId))
            {
                response.Data = "Not allowed";
                response.Message = "You are not allowed to delete file for this submission";
                response.Success = false;
                return response;
            }
            if (!submission.HasSubmission)
            {
                response.Data = "Not found";
                response.Message = "There is no submission file for this submission";
                response.Success = false;
                return response;
            }

            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/Submissions", submission.CourseId,
                submission.SectionId, submission.CourseId, submission.AffiliatedAssignmentId));
            var filePath = Directory.GetFiles(target).FirstOrDefault();
            submission.FilePath = null;
            submission.HasSubmission = false;
            File.Delete(filePath);
            response.Data = target;
            response.Message = "file succesfully deleted.";
            submission.UpdatedAt = DateTime.Now;
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync();
            return response;
        }
    }
}