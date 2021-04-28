using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.Assignment;
using backend.Models;
using backend.Services.SubmissionServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.AssignmentServices
{
    public class AssignmentService : IAssignmentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ISubmissionService _submissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        public AssignmentService(IMapper mapper, ISubmissionService submissionService, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _submissionService = submissionService;
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
            Course course = await _context.Courses.FirstOrDefaultAsync(s => s.Id == assignment.AfilliatedCourseId);
            if (course == null)
            {
                response.Data = "No submission";
                response.Message = "There is no course under this id";
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
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}",
                "StaticFiles/Assignments", assignment.AfilliatedCourseId, assignment.Id));
            Directory.CreateDirectory(target);
            if (file.File.Length <= 0) response.Success = false;
            else
            {
                string oldfile = Directory.GetFiles(target).FirstOrDefault();
                if (File.Exists(oldfile))
                {
                    File.Delete(oldfile);
                }
                var filePath = Path.Combine(target, string.Format("{0}_{1}",
                course.Name.Trim().Replace(" ", "_"),
                assignment.Title.Trim().Replace(" ", "_")) + ".pdf");
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
            Course course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == assignment.AfilliatedCourseId);
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
            Course course = await _context.Courses.Include(pg => pg.Instructors).FirstOrDefaultAsync(c => c.Id == assignment.AfilliatedCourseId && c.Instructors.Any(pgu => pgu.UserId == user.Id));
            if (course == null)
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

        public async Task<ServiceResponse<GetAssignmentDto>> SubmitAssignment(AddAssignmentDto assignmentDto)
        {
            ServiceResponse<GetAssignmentDto> response = new ServiceResponse<GetAssignmentDto>();
            Course course = await _context.Courses.Include(c => c.Instructors).FirstOrDefaultAsync(c => c.Id == assignmentDto.CourseId);
            if (course == null)
            {
                response.Data = null;
                response.Message = "There is no course under this id";
                response.Success = false;
                return response;
            }
            if (course.Instructors.FirstOrDefault(u => u.UserId == GetUserId()) == null)
            {
                response.Data = null;
                response.Message = "You are not allowed to post assignments for this course";
                response.Success = false;
                return response;
            }
            if (assignmentDto.Title == null)
            {
                response.Data = null;
                response.Message = "You should add a title to your submission";
                response.Success = false;
                return response;
            }
            if (assignmentDto.AssignmenDescription == null)
            {
                response.Data = null;
                response.Message = "You should add a description to your submission";
                response.Success = false;
                return response;
            }
            if ((assignmentDto.DueDate - DateTime.Now).TotalDays < 3)
            {
                response.Data = null;
                response.Message = "A deadline cannot be closer to publishment date less than 3 days";
                response.Success = false;
                return response;
            }
            Assignment assignment = new Assignment
            {
                CreatedAt = DateTime.Now,
                DueDate = assignmentDto.DueDate,
                AfilliatedCourseId = assignmentDto.CourseId,
                AcceptedTypes = assignmentDto.AcceptedTypes,
                AssignmentDescription = assignmentDto.AssignmenDescription,
                CanBeGradedByStudents = assignmentDto.CanBeGradedByStudents,
                IsItGraded = assignmentDto.IsItGraded,
                VisibilityOfSubmission = assignmentDto.VisibiltyOfSubmission,
                MaxFileSizeInBytes = assignmentDto.MaxFileSizeInBytes,
                HasFile = false,
                FilePath = "",
                Title = assignmentDto.Title
            };
            await _context.Assignments.AddAsync(assignment);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetAssignmentDto>(assignment);
            response.Data.FileEndpoint = string.Format("Assignment/File/{0}", assignment.Id);
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteAssignment(DeleteAssignmentDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.ProjectGroups).ThenInclude(pgu => pgu.ProjectGroup).FirstOrDefaultAsync(u => u.Id == GetUserId());
            Assignment assignment = await _context.Assignments.Include(a => a.Submissions).FirstOrDefaultAsync(a => a.Id == dto.AssignmentId);
            if (assignment == null)
            {
                response.Data = "No submission";
                response.Message = "There is no assignment under this id";
                response.Success = false;
                return response;
            }
            Course course = await _context.Courses.Include(pg => pg.Instructors).FirstOrDefaultAsync(c => c.Id == assignment.AfilliatedCourseId && c.Instructors.Any(pgu => pgu.UserId == user.Id));
            if (course == null)
            {
                response.Data = "Not allowed";
                response.Message = "You are not allowed to delete this assignment";
                response.Success = false;
                return response;
            }
            if (assignment.HasFile)
            {
                await DeleteFile(new DeleteAssignmentFileDto { AssignmentId = dto.AssignmentId });
            }
            foreach (Submission submission in assignment.Submissions)
            {
                await _submissionService.DeleteWithForce(submission.Id);
            }
            response.Data = assignment.FilePath;
            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return response;
        }

    }
}