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


        public async Task<ServiceResponse<string>> SubmitAssignmentFile(AddAssignmentFileDto file)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.InstructedCourses).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Assignment assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == file.AssignmentId);
            if (file.File == null || assignment == null)
            {
                response.Data = "No submission";
                response.Message = "There is no submission under this id or you didnt provide a file to submit";
                response.Success = false;
                return response;
            }
            Course course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == assignment.CourseId);
            Section section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == file.SectionId);
            if (course == null || section == null)
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
                assignment.HasFile = true;
            }
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<ServiceResponse<string>> DownloadAssignmentFile(GetAssignmentFileDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.ProjectGroups).ThenInclude(pgu => pgu.ProjectGroup).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Assignment assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == dto.AssignmentId);
            if (assignment == null || assignment.FilePath == null)
            {
                response.Data = "No submission";
                response.Message = "There is no submission under this id";
                response.Success = false;
                return response;
            }
            Course course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == assignment.CourseId);
            ProjectGroup projectGroup = await _context.ProjectGroups.FirstOrDefaultAsync(pg => pg.AffiliatedCourseId == course.Id);
            if (course == null || projectGroup == null)
            {
                response.Data = "Not allowed";
                response.Message = "You are not allowed to post file for this assignment";
                response.Success = false;
                return response;
            }
            response.Data = assignment.FilePath;
            return response;
        }


        public async Task<ServiceResponse<string>> DeleteFile(DeleteAssignmentFileDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.ProjectGroups).ThenInclude(pgu => pgu.ProjectGroup).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Assignment assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == dto.AssignmentId);
            if (assignment == null || assignment.FilePath == null)
            {
                response.Data = "No submission";
                response.Message = "There is no submission under this id";
                response.Success = false;
                return response;
            }
            Course course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == assignment.CourseId);
            ProjectGroup projectGroup = await _context.ProjectGroups.FirstOrDefaultAsync(pg => pg.AffiliatedCourseId == course.Id);
            if (course == null || projectGroup == null)
            {
                response.Data = "Not allowed";
                response.Message = "You are not allowed to post file for this assignment";
                response.Success = false;
                return response;
            }
            File.Delete(assignment.FilePath);
            assignment.FilePath = null;
            assignment.CreatedAt = DateTime.MinValue;
            assignment.HasFile = false;
            response.Message = "file succesfully deleted.";
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
            return response;
        }
    }
}