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
            ProjectGroup projectGroup = await _context.ProjectGroups.FirstOrDefaultAsync(pg => pg.Id == dto.ProjectGroupId);
            Assignment assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == dto.AssignmentId);
            if (projectGroup == null || assignment == null || projectGroup.AffiliatedSectionId != assignment.AffiliatedSectionId)
            {
                response.Data = null;
                response.Message = "There is a mistake in project and assignment ids you entered";
                response.Success = false;
                return response;
            }
            Submission submission = await _context.Submissions
            .Include(s => s.AffiliatedAssignment)
            .Include(s => s.AffiliatedGroup)
            .FirstOrDefaultAsync(s => s.AffiliatedAssignment.Id == dto.AssignmentId && s.AffiliatedGroup.Id == dto.ProjectGroupId);

            if (submission == null)
            {
                response.Data = null;
                response.Message = "This group has not yet submitted their work this assigment";
                response.Success = false;
                return response;
            }
            User user = await _context.Users.Include(u => u.ProjectGroups).FirstOrDefaultAsync(u => u.Id == GetUserId());
            if (!assignment.VisibilityOfSubmission && user.ProjectGroups.Any(pg => pg.ProjectGroupId != dto.ProjectGroupId) && user.UserType == UserTypeClass.Student)
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
            User user = await _context.Users.Include(u => u.ProjectGroups).ThenInclude(pgu => pgu.ProjectGroup).ThenInclude(pg => pg.Submissions).ThenInclude(s => s.AffiliatedAssignment).FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = _context.ProjectGroups.Include(pg => pg.Submissions).ThenInclude(s => s.AffiliatedAssignment).FirstOrDefault(pg => pg.Id == dto.ProjectGroupId);
            Assignment assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == dto.AssignmentId);
            if (projectGroup == null || assignment == null || projectGroup.AffiliatedSectionId != assignment.AffiliatedSectionId)
            {
                response.Data = "Not allowed";
                response.Message = "You are not allowed to post submission for this group";
                response.Success = false;
                return response;
            }
            Submission submission = projectGroup.Submissions.FirstOrDefault(s => s.AffiliatedAssignment.Id == dto.AssignmentId);
            if (submission == null)
            {
                response.Data = "Bad request";
                response.Message = "there is no submission to attach file";
                response.Success = false;
                return response;
            }
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/Submissions", assignment.CourseId,
                projectGroup.AffiliatedSectionId, dto.AssignmentId, dto.ProjectGroupId));

            Directory.CreateDirectory(target);
            if (dto.File.Length <= 0) response.Success = false;
            else
            {
                string oldfile = Directory.GetFiles(target)[0];
                var filePath = Path.Combine(target, dto.File.FileName);
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
            }
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync();

            return response;
        }


    }
}