using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.Assignment;
using backend.Dtos.ProjectGroup;
using backend.Dtos.Submission;
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
                response.Message = "You are not allowed to see file for this assignment";
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
            Course course = await _context.Courses.Include(c => c.Instructors).Include(c => c.Sections).ThenInclude(s => s.ProjectGroups)
                .FirstOrDefaultAsync(c => c.Id == assignmentDto.CourseId);
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

            foreach (Section s in course.Sections)
            {
                foreach (ProjectGroup pg in s.ProjectGroups)
                {
                    await _submissionService.AddSubmission(new AddSubmissionDto
                    {
                        AssignmentId = assignment.Id,
                        ProjectGroupId = pg.Id
                    });
                }
            }

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
                response.Data = "No assignment";
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
        public async Task<ServiceResponse<GetAssignmentDto>> GetAssignment(int assignmentId)
        {
            ServiceResponse<GetAssignmentDto> response = new ServiceResponse<GetAssignmentDto>();
            Assignment assignment = await _context.Assignments
                .Include(a => a.Submissions).Include(a => a.AfilliatedCourse)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);
            if (assignment == null)
            {
                response.Data = null;
                response.Message = "There is no submission under this id";
                response.Success = false;
                return response;
            }
            User user = await _context.Users.Include(u => u.InstructedCourses)
                .Include(u => u.ProjectGroups).ThenInclude(pgu => pgu.ProjectGroup)
                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            if (!user.InstructedCourses.Any(c => c.CourseId == assignment.AfilliatedCourseId) && user.UserType == UserTypeClass.Student
                && !user.ProjectGroups.Any(pgu => pgu.ProjectGroup.AffiliatedCourseId == assignment.AfilliatedCourseId)
                )
            {
                response.Data = null;
                response.Message = "You are not allowed to see for this assignment";
                response.Success = false;
                return response;
            }
            response.Data = _mapper.Map<GetAssignmentDto>(assignment);
            response.Data.SubmissionIds = assignment.Submissions.Select(s => s.Id).ToList();
            response.Data.publisher = assignment.AfilliatedCourse.Name;
            return response;
        }
        public async Task<ServiceResponse<GetAssignmentDto>> UpdateAssignment(UpdateAssignmentDto dto)
        {
            ServiceResponse<GetAssignmentDto> response = new ServiceResponse<GetAssignmentDto>();
            Assignment assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == dto.AssignmentId);
            if (assignment == null)
            {
                response.Data = null;
                response.Message = "There is no submission under this id";
                response.Success = false;
                return response;
            }
            User user = await _context.Users.Include(u => u.InstructedCourses)
                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            if (!user.InstructedCourses.Any(c => c.CourseId == assignment.AfilliatedCourseId))
            {
                response.Data = null;
                response.Message = "You are not allowed to see for this assignment";
                response.Success = false;
                return response;
            }
            assignment.AssignmentDescription = dto.AssignmenDescription;
            assignment.DueDate = dto.DueDate;
            assignment.IsItGraded = dto.IsItGraded;
            assignment.Title = dto.Title;
            assignment.AcceptedTypes = dto.AcceptedTypes;
            assignment.MaxFileSizeInBytes = dto.MaxFileSizeInBytes;
            assignment.VisibilityOfSubmission = dto.VisibiltyOfSubmission;
            assignment.CanBeGradedByStudents = dto.CanBeGradedByStudents;
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetAssignmentDto>(assignment);
            return response;
        }
        public async Task<ServiceResponse<string>> DeleteWithForce(int assignmentId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            Assignment assignment = await _context.Assignments.Include(s => s.Submissions).FirstOrDefaultAsync(s => s.Id == assignmentId);
            if (assignment == null)
            {
                response.Data = "Bad Request";
                response.Message = "There is no submission with this Id";
                response.Success = false;
                return response;
            }
            foreach (int submissionId in assignment.Submissions.Select(c => c.Id))
                await _submissionService.DeleteWithForce(submissionId);
            if (!assignment.HasFile)
            {
                response.Data = "Not found";
                response.Message = "There is no submission file for this submission";
                response.Success = true;
                return response;
            }

            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}",
                "StaticFiles/Assignments", assignment.AfilliatedCourseId, assignment.Id));
            var filePath = Directory.GetFiles(target).FirstOrDefault();
            assignment.FilePath = null;
            assignment.HasFile = false;
            File.Delete(filePath);
            response.Data = target;
            response.Message = "file succesfully deleted.";
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
            return response;
        }
        public async Task<ServiceResponse<GetOzgurDto>> GetOzgur(int groupId)
        {
            ServiceResponse<GetOzgurDto> response = new ServiceResponse<GetOzgurDto>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Assignment assignment = await _context.Assignments
                .Include(pg => pg.Submissions).ThenInclude(s => s.Comments).ThenInclude(c => c.CommentedUser)
                .Include(pg => pg.Submissions).ThenInclude(s => s.AffiliatedAssignment)
                .Include(pg => pg.Submissions).ThenInclude(s => s.AffiliatedGroup)
                .Include(pg => pg.AfilliatedCourse).ThenInclude(c => c.Instructors)
                .FirstOrDefaultAsync(s => s.Id == groupId);
            if (assignment == null)
            {
                response.Data = null;
                response.Message = "There is no such project group";
                response.Success = false;
                return response;
            }
            List<int> instrs = assignment.AfilliatedCourse.Instructors.Select(i => i.UserId).ToList();
            List<Submission> submissions = assignment.Submissions.ToList();
            HashSet<int> graderIds = new HashSet<int>();
            foreach (Submission s in submissions)
            {
                List<Comment> comments = s.Comments.ToList();
                foreach (Comment c in comments)
                {
                    if (instrs.Contains(c.CommentedUserId))
                        graderIds.Add(c.CommentedUserId);
                }
            }
            List<string> graderNames = new List<string>();
            foreach (int id in graderIds)
            {
                graderNames.Add(_context.Users.FirstOrDefault(u => u.Id == id).Name);
            }
            graderNames.Add("Students");
            graderIds.Add(-1);

            GetOzgurDto getOzgurDto = new GetOzgurDto();
            getOzgurDto.graders = graderNames;
            List<OzgurStatDto> ozgurStatDtos = new List<OzgurStatDto>();
            foreach (Submission s in submissions)
            {
                OzgurStatDto ozgurStatDto = new OzgurStatDto();
                ozgurStatDto.StatName = s.AffiliatedGroup.Name;
                ozgurStatDto.Grades = new List<decimal>();
                foreach (int id in graderIds)
                {
                    ServiceResponse<decimal> grade = await _submissionService.GetGradeWithGraderId(s.Id, id);
                    ozgurStatDto.Grades.Add(grade.Data);
                }
                ozgurStatDtos.Add(ozgurStatDto);
            }
            getOzgurDto.ozgurStatDtos = ozgurStatDtos;
            // List<int> graderIds = submission.Comments.Select(c => c.CommentedUserId).ToList();
            response.Data = getOzgurDto;
            return response;
        }
        public async Task<ServiceResponse<List<GetFeedItemDto>>> GetFeeds()
        {
            ServiceResponse<List<GetFeedItemDto>> response = new ServiceResponse<List<GetFeedItemDto>>();
            User user = await _context.Users
            .Include(u => u.ProjectGroups).ThenInclude(pg => pg.ProjectGroup).ThenInclude(pg => pg.Submissions)
                .ThenInclude(s => s.AffiliatedAssignment).ThenInclude(a => a.AfilliatedCourse)
                .Include(u => u.ProjectGroups).ThenInclude(pg => pg.ProjectGroup).ThenInclude(pg => pg.AffiliatedCourse).ThenInclude(c => c.Assignments)
            .Include(u => u.InstructedCourses).ThenInclude(cu => cu.Course).ThenInclude(c => c.Assignments).ThenInclude(a => a.Submissions)
            .FirstOrDefaultAsync(u => u.Id == GetUserId());

            List<GetFeedItemDto> data = new List<GetFeedItemDto>();
            List<int> submittedAssignments = new List<int>();
            if (user.ProjectGroups != null)
            {
                foreach (ProjectGroup pg in user.ProjectGroups.Select(pgu => pgu.ProjectGroup))
                {
                    if (pg.Submissions != null)
                    {
                        foreach (Submission s in pg.Submissions)
                        {
                            if (s.AffiliatedAssignment != null && pg.AffiliatedCourse != null)
                            {
                                submittedAssignments.Add(s.AffiliatedAssignment.Id);
                                data.Add(
                                    new GetFeedItemDto
                                    {
                                        title = s.AffiliatedAssignment.Title,
                                        status = s.HasSubmission ? s.IsGraded ? 1 : 2 : 3,
                                        caption = s.AffiliatedAssignment.AssignmentDescription,
                                        publisher = pg.AffiliatedCourse.Name,
                                        publisherId = pg.AffiliatedCourse.Id,
                                        publishmentDate = s.AffiliatedAssignment.CreatedAt,
                                        dueDate = s.AffiliatedAssignment.DueDate,
                                        hasFile = s.AffiliatedAssignment.HasFile,
                                        fileEndpoint = "Assignment/File/" + s.AffiliatedAssignment.Id,
                                        projectId = pg.Id,
                                        submissionId = s.Id,
                                        assignmentId = s.AffiliatedAssignmentId
                                    }
                                );
                            }
                        }
                    }
                }
            }



            List<Assignment> goingAssignments = new List<Assignment>();

            if (user.InstructedCourses != null)
            {
                foreach (Course c in user.InstructedCourses.Select(cu => cu.Course))
                {
                    if (c.Assignments != null)
                    {
                        foreach (Assignment a in c.Assignments)
                        {
                            data.Add(
                                new GetFeedItemDto
                                {
                                    title = a.Title,
                                    status =
                                    !a.Submissions.Any(s => !s.HasSubmission) ? !a.Submissions.Any(s => !s.IsGraded) ? 1 : 2 :
                                    (a.Submissions.Where(s => s.HasSubmission).Count() == 0 || a.Submissions.Where(s => s.HasSubmission).Any(s => s.IsGraded)) ? 3 : 4,

                                    caption = a.AssignmentDescription,
                                    publisher = a.AfilliatedCourse.Name,
                                    publisherId = a.AfilliatedCourse.Id,
                                    publishmentDate = a.CreatedAt,
                                    dueDate = a.DueDate,
                                    hasFile = a.HasFile,
                                    fileEndpoint = "Assignment/File/" + a.Id,
                                    courseId = c.Id,
                                    assignmentId = a.Id
                                }
                            );
                        }
                    }
                }
            }
            data.OrderBy(i => i.dueDate);

            response.Data = data;

            return response;
        }
        public async Task<ServiceResponse<List<NotGradedAsssignmentsDto>>> GetNotGradedAssignments()
        {
            ServiceResponse<List<NotGradedAsssignmentsDto>> response = new ServiceResponse<List<NotGradedAsssignmentsDto>>();
            User user = await _context.Users
                .Include(u => u.ProjectGroups)
                    .ThenInclude(pg => pg.ProjectGroup).ThenInclude(pg => pg.Submissions).ThenInclude(s => s.AffiliatedAssignment).ThenInclude(a => a.AfilliatedCourse)
                .Include(u => u.ProjectGroups).ThenInclude(pg => pg.ProjectGroup).ThenInclude(pg => pg.AffiliatedCourse)
                .Include(u => u.InstructedCourses).ThenInclude(cu => cu.Course).ThenInclude(c => c.Assignments).ThenInclude(a => a.Submissions)
            .FirstOrDefaultAsync(u => u.Id == GetUserId());
            List<NotGradedAsssignmentsDto> data = new List<NotGradedAsssignmentsDto>();
            if (user.ProjectGroups != null)
            {
                foreach (ProjectGroup pg in user.ProjectGroups.Select(pgu => pgu.ProjectGroup))
                {
                    if (pg.Submissions != null)
                    {
                        foreach (Submission s in pg.Submissions)
                        {
                            if (s.AffiliatedAssignment != null && pg.AffiliatedCourse != null && s.HasSubmission && !s.IsGraded)
                            {
                                Course course = s.AffiliatedAssignment.AfilliatedCourse;
                                data.Add(
                                    new NotGradedAsssignmentsDto
                                    {
                                        courseCode = string.Format("{0}_{1}_{2}", course.Name, course.CourseSemester, course.Year),
                                        assignmentName = s.AffiliatedAssignment.Title,
                                        dueDate = s.AffiliatedAssignment.DueDate,
                                        courseId = s.AffiliatedAssignment.AfilliatedCourseId,
                                        assignmentId = s.AffiliatedAssignmentId
                                    }
                                );
                            }
                        }
                    }
                }
            }
            if (user.InstructedCourses != null)
            {
                foreach (Course c in user.InstructedCourses.Select(cu => cu.Course))
                {
                    if (c.Assignments != null)
                    {
                        foreach (Assignment a in c.Assignments)
                        {
                            if (a.Submissions.Any(s => !s.IsGraded))
                            {
                                data.Add(
                                   new NotGradedAsssignmentsDto
                                   {
                                       courseCode = string.Format("{0}_{1}_{2}", c.Name, c.CourseSemester, c.Year),
                                       assignmentName = a.Title,
                                       dueDate = a.DueDate,
                                       courseId = c.Id,
                                       assignmentId = a.Id
                                   }
                               );
                            }
                        }
                    }
                }
            }

            data.OrderBy(i => i.dueDate);
            response.Data = data;
            return response;
        }
        public async Task<ServiceResponse<List<UpcomingAssignmentsDto>>> GetUpcomingAssignments()
        {
            ServiceResponse<List<UpcomingAssignmentsDto>> response = new ServiceResponse<List<UpcomingAssignmentsDto>>();
            User user = await _context.Users
                .Include(u => u.ProjectGroups)
                    .ThenInclude(pg => pg.ProjectGroup).ThenInclude(pg => pg.Submissions).ThenInclude(s => s.AffiliatedAssignment).ThenInclude(a => a.AfilliatedCourse)
                .Include(u => u.ProjectGroups).ThenInclude(pg => pg.ProjectGroup).ThenInclude(pg => pg.AffiliatedCourse)
            .FirstOrDefaultAsync(u => u.Id == GetUserId());
            List<UpcomingAssignmentsDto> data = new List<UpcomingAssignmentsDto>();
            if (user.ProjectGroups != null)
            {
                foreach (ProjectGroup pg in user.ProjectGroups.Select(pgu => pgu.ProjectGroup))
                {
                    if (pg.Submissions != null)
                    {
                        foreach (Submission s in pg.Submissions)
                        {
                            if (s.AffiliatedAssignment != null && pg.AffiliatedCourse != null && !s.HasSubmission)
                            {
                                Course course = s.AffiliatedAssignment.AfilliatedCourse;
                                data.Add(
                                    new UpcomingAssignmentsDto
                                    {
                                        courseCode = string.Format("{0}_{1}_{2}", course.Name, course.CourseSemester, course.Year),
                                        assignmentName = s.AffiliatedAssignment.Title,
                                        dueDate = s.AffiliatedAssignment.DueDate,
                                        projectId = s.AffiliatedGroupId,
                                        submissionId = s.Id
                                    }
                                );
                            }
                        }
                    }
                }
            }

            data.OrderBy(i => i.dueDate);
            response.Data = data;
            return response;
        }

    }
}