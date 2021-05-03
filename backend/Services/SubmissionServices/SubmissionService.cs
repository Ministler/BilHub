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
using System.Collections.Generic;
using System.Web;
using backend.Services.CommentServices;
using backend.Dtos.Comment;
using System.Collections.ObjectModel;
using backend.Dtos.Comment.FeedbackItems;

namespace backend.Services.SubmissionServices
{
    public class SubmissionService : ISubmissionService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICommentService _commentservice;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        public SubmissionService(IMapper mapper, ICommentService commentservice, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _commentservice = commentservice;
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
            Submission submission = await _context.Submissions.Include(s => s.AffiliatedGroup)
                .ThenInclude(pg => pg.GroupMembers).FirstOrDefaultAsync(s => s.Id == dto.SubmissionId);
            if (submission == null)
            {
                response.Data = null;
                response.Message = "There is no such submission";
                response.Success = false;
                return response;
            }
            if (!submission.HasFile)
            {
                response.Data = null;
                response.Message = "This group has not yet submitted their file for this assigment";
                response.Success = false;
                return response;
            }
            Course course = await _context.Courses.Include(c => c.Instructors)
                .FirstOrDefaultAsync(c => c.Instructors.Any(cu => cu.UserId == GetUserId()
                    && cu.CourseId == submission.AffiliatedAssignment.AfilliatedCourseId));
            if (!submission.AffiliatedAssignment.VisibilityOfSubmission
                && course == null && submission.AffiliatedGroup.GroupMembers.Any(u => u.UserId == GetUserId()))
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
            Submission submission = await _context.Submissions.Include(s => s.AffiliatedAssignment).FirstOrDefaultAsync(s => s.Id == dto.SubmissionId);
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
            if (submission.HasFile && DateTime.Compare(submission.AffiliatedAssignment.DueDate, DateTime.Now) < 0)
            {
                response.Data = "Not allowed";
                response.Message = "You can not change your submission after deadline";
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
                Course course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == submission.CourseId);
                Section section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == submission.SectionId);
                var filePath = Path.Combine(target, string.Format("{0}_Section{1}_Group{2}_{3}"
                    , course.Name.Trim().Replace(" ", "_"), section.SectionNo, submission.AffiliatedGroupId,
                    submission.AffiliatedAssignment.Title.Trim().Replace(" ", "_")) + extension);
                submission.FilePath = filePath;
                submission.UpdatedAt = DateTime.Now;
                submission.HasFile = true;
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

        public async Task<ServiceResponse<string>> DeleteSubmissionFile(DeleteSubmissionFileDto dto)
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
            if (!submission.HasFile)
            {
                response.Data = "Not found";
                response.Message = "There is no submission file for this submission";
                response.Success = false;
                return response;
            }

            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/Submissions", submission.CourseId,
                submission.SectionId, submission.AffiliatedAssignmentId, submission.AffiliatedGroupId));
            var filePath = Directory.GetFiles(target).FirstOrDefault();
            submission.FilePath = null;
            submission.HasFile = false;
            File.Delete(filePath);
            response.Data = target;
            response.Message = "file succesfully deleted.";
            submission.UpdatedAt = DateTime.Now;
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteWithForce(int submissionId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            Submission submission = await _context.Submissions.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == submissionId);
            if (submission == null)
            {
                response.Data = "Bad Request";
                response.Message = "There is no submission with this Id";
                response.Success = false;
                return response;
            }
            foreach (int commentId in submission.Comments.Select(c => c.Id))
                await _commentservice.DeleteWithForce(commentId);
            if (!submission.HasFile)
            {
                response.Data = "Not found";
                response.Message = "There is no submission file for this submission";
                response.Success = true;
                return response;
            }

            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/Submissions", submission.CourseId,
                submission.SectionId, submission.AffiliatedAssignmentId, submission.AffiliatedGroupId));
            var filePath = Directory.GetFiles(target).FirstOrDefault();
            submission.FilePath = null;
            submission.HasFile = false;
            File.Delete(filePath);
            response.Data = target;
            response.Message = "file succesfully deleted.";
            submission.UpdatedAt = DateTime.Now;
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<string>>> DownloadNotGradedSubmissions(GetUngradedSubmissionFilesDto dto)
        {
            ServiceResponse<IEnumerable<string>> response = new ServiceResponse<IEnumerable<string>>();
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
            response.Data = _context.Submissions.Include(s => s.AffiliatedAssignment)
                .Where(s => !s.IsGraded && s.AffiliatedAssignment.IsItGraded).Select(s => s.FilePath);
            return response;
        }

        public async Task<ServiceResponse<GetSubmissionDto>> AddSubmission(AddSubmissionDto addSubmissionDto)
        {
            ServiceResponse<GetSubmissionDto> response = new ServiceResponse<GetSubmissionDto>();
            ProjectGroup projectGroup = await _context.ProjectGroups
                .FirstOrDefaultAsync(u => u.Id == addSubmissionDto.ProjectGroupId);
            Assignment assignment = await _context.Assignments//.Include(a => a.AfilliatedCourse)
                .FirstOrDefaultAsync(a => a.Id == addSubmissionDto.AssignmentId);
            if (assignment == null || projectGroup == null)
            {
                response.Data = null;
                response.Message = "There is etiher no assgnment or no project group with given Id";
                response.Success = false;
                return response;
            }
            Submission submission = new Submission
            {
                HasSubmission = false,
                HasFile = false,
                AffiliatedAssignmentId = assignment.Id,
                CourseId = assignment.AfilliatedCourseId,
                IsGraded = false,
                SrsGrade = 0,
                SectionId = projectGroup.AffiliatedSectionId,
                Description = "",
                AffiliatedGroupId = projectGroup.Id,
                UpdatedAt = DateTime.Now,
                FilePath = "",

            };
            _context.Submissions.Add(submission);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetSubmissionDto>(submission);
            response.Data.FileEndpoint = string.Format("Submission/File/{0}", submission.Id);
            response.Data.FileName = submission.HasFile ? submission.FilePath.Split('/').Last() : "";
            return response;
        }

        public async Task<ServiceResponse<GetSubmissionDto>> UpdateSubmission(UpdateSubmissionDto submissionDto)
        {
            ServiceResponse<GetSubmissionDto> response = new ServiceResponse<GetSubmissionDto>();
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions.Include(s => s.AffiliatedGroup)
                .ThenInclude(pg => pg.GroupMembers).FirstOrDefaultAsync(s => s.Id == submissionDto.SubmissionId);
            if (submission == null)
            {
                response.Data = null;
                response.Message = "There is no submission under this Id";
                response.Success = false;
                return response;
            }
            if (!submission.AffiliatedGroup.GroupMembers.Any(pgu => pgu.UserId == user.Id))
            {
                response.Data = null;
                response.Message = "You don't have a submission for this assignment";
                response.Success = false;
                return response;
            }
            submission.Description = submissionDto.Description;
            submission.UpdatedAt = DateTime.Now;
            submission.HasSubmission = true;
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetSubmissionDto>(submission);
            response.Data.FileEndpoint = string.Format("Submission/File/{0}", submission.Id);
            response.Data.FileName = submission.HasFile ? submission.FilePath.Split('/').Last() : "";
            return response;
        }

        public async Task<ServiceResponse<string>> Delete(int submissionId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            Submission submission = await _context.Submissions
            .Include(s => s.AffiliatedAssignment)
            .FirstOrDefaultAsync(s => s.Id == submissionId);
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
            if (submission.AffiliatedAssignment.DueDate < DateTime.Now)
            {
                response.Data = "Not allowed";
                response.Message = "You can not delete this submission since the deadline is passed";
                response.Success = false;
                return response;
            }
            await DeleteWithForce(submissionId);
            submission.HasSubmission = false;
            submission.Description = "";
            submission.UpdatedAt = DateTime.Now;
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<GetSubmissionDto>> GetSubmission(int submissionId)
        {
            ServiceResponse<GetSubmissionDto> response = new ServiceResponse<GetSubmissionDto>();
            Submission submission = await _context.Submissions
                .Include(s => s.AffiliatedAssignment).Include(s => s.AffiliatedGroup).ThenInclude(pg => pg.GroupMembers).ThenInclude(pgu => pgu.User)
                .Include(s => s.AffiliatedAssignment).Include(s => s.AffiliatedGroup).ThenInclude(pg => pg.AffiliatedSection)
                .FirstOrDefaultAsync(s => s.Id == submissionId);
            if (submission == null)
            {
                response.Data = null;
                response.Message = "There is no such submission";
                response.Success = false;
                return response;
            }
            Course course = await _context.Courses.Include(c => c.Instructors)
                .FirstOrDefaultAsync(c => c.Instructors.Any(cu => cu.UserId == GetUserId() && cu.CourseId == submission.AffiliatedAssignment.AfilliatedCourseId));
            if (!submission.AffiliatedAssignment.VisibilityOfSubmission
                && course == null && !submission.AffiliatedGroup.GroupMembers.Any(u => u.UserId == GetUserId()))
            {
                response.Data = null;
                response.Message = "You are not authorized to see this submission";
                response.Success = false;
                return response;
            }
            response.Data = _mapper.Map<GetSubmissionDto>(submission);
            response.Data.FileEndpoint = string.Format("Submission/File/{0}", submission.Id);
            response.Data.SectionNumber = submission.AffiliatedGroup.AffiliatedSection.SectionNo;
            response.Data.FileName = submission.HasFile ? submission.FilePath.Split('/').Last() : "";
            return response;
        }

        public async Task<ServiceResponse<List<GetCommentDto>>> GetInstructorComments(int submissionId)
        {
            ServiceResponse<List<GetCommentDto>> response = new ServiceResponse<List<GetCommentDto>>();
            Submission submission = await _context.Submissions.Include(s => s.AffiliatedGroup).ThenInclude(pg => pg.GroupMembers)
                .Include(s => s.Comments).Include(s => s.AffiliatedAssignment).FirstOrDefaultAsync(s => s.Id == submissionId);
            if (submission == null)
            {
                response.Data = null;
                response.Message = "There is no such submission";
                response.Success = false;
                return response;
            }
            Course course = await _context.Courses.Include(c => c.Instructors)
                .FirstOrDefaultAsync(c => c.Instructors.Any(cu => cu.UserId == GetUserId()
                    && cu.CourseId == submission.AffiliatedAssignment.AfilliatedCourseId));
            if (!submission.AffiliatedAssignment.VisibilityOfSubmission
                && course == null && submission.AffiliatedGroup.GroupMembers.Any(u => u.UserId == GetUserId()))
            {
                response.Data = null;
                response.Message = "You are not authorized to see this submission";
                response.Success = false;
                return response;
            }
            List<GetCommentDto> comments = _context.Comments
                .Include(c => c.CommentedUser)
                .Where(c => c.CommentedSubmissionId == submissionId)
                .Where(c => c.CommentedUser.UserType == UserTypeClass.Instructor)
                .Select(c => _mapper.Map<GetCommentDto>(c)).ToList();

            response.Data = comments;
            return response;
        }

        public async Task<ServiceResponse<List<GetCommentDto>>> GetTaComments(int submissionId)
        {
            ServiceResponse<List<GetCommentDto>> response = new ServiceResponse<List<GetCommentDto>>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions
                .Include(s => s.AffiliatedAssignment).ThenInclude(a => a.AfilliatedCourse).ThenInclude(c => c.Instructors)
                .Include(s => s.AffiliatedGroup).ThenInclude(pg => pg.GroupMembers)
                .Include(s => s.Comments).Include(s => s.AffiliatedAssignment).FirstOrDefaultAsync(s => s.Id == submissionId);
            if (submission == null)
            {
                response.Data = null;
                response.Message = "There is no such submission";
                response.Success = false;
                return response;
            }
            Course course = submission.AffiliatedAssignment.AfilliatedCourse;
            if (!submission.AffiliatedAssignment.VisibilityOfSubmission
                && course.Instructors.Any(cu => cu.UserId == GetUserId())
                && user.UserType == UserTypeClass.Student)
            {
                response.Data = null;
                response.Message = "You are not authorized for this endpoint";
                response.Success = false;
                return response;
            }
            List<int> intrs = new List<int>();
            intrs = course.Instructors.Select(cu => cu.UserId).ToList();
            List<GetCommentDto> comments = _context.Comments.Include(c => c.CommentedUser)
                .Where(c => c.CommentedSubmissionId == submissionId)
                .Where(c => c.CommentedUser.UserType == UserTypeClass.Student)
                .Where(c => intrs.Contains(c.CommentedUserId))
                .Select(c => _mapper.Map<GetCommentDto>(c)).ToList();

            response.Data = comments;
            return response;
        }

        public async Task<ServiceResponse<List<GetCommentDto>>> GetStudentComments(int submissionId)
        {
            ServiceResponse<List<GetCommentDto>> response = new ServiceResponse<List<GetCommentDto>>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions
                .Include(s => s.AffiliatedAssignment).ThenInclude(a => a.AfilliatedCourse).ThenInclude(c => c.Instructors)
                .Include(s => s.AffiliatedGroup).ThenInclude(pg => pg.GroupMembers)
                .Include(s => s.Comments).Include(s => s.AffiliatedAssignment).FirstOrDefaultAsync(s => s.Id == submissionId);
            if (submission == null)
            {
                response.Data = null;
                response.Message = "There is no such submission";
                response.Success = false;
                return response;
            }
            Course course = submission.AffiliatedAssignment.AfilliatedCourse;
            if (!submission.AffiliatedAssignment.VisibilityOfSubmission
                && course.Instructors.Any(cu => cu.UserId == GetUserId())
                && user.UserType == UserTypeClass.Student)
            {
                response.Data = null;
                response.Message = "You are not authorized for this endpoint";
                response.Success = false;
                return response;
            }
            List<int> intrs = new List<int>();
            intrs = course.Instructors.Select(cu => cu.UserId).ToList();
            List<GetCommentDto> comments = _context.Comments.Include(c => c.CommentedUser)
                .Where(c => c.CommentedSubmissionId == submissionId)
                .Where(c => c.CommentedUser.UserType == UserTypeClass.Student)
                .Where(c => !intrs.Contains(c.CommentedUserId))
                .Select(c => _mapper.Map<GetCommentDto>(c)).ToList();

            response.Data = comments;
            return response;
        }

        public async Task<ServiceResponse<decimal>> GetGradeWithGraderId(int submissionId, int graderId)
        {
            ServiceResponse<decimal> response = new ServiceResponse<decimal>();
            Submission submission = await _context.Submissions.Include(s => s.Comments).FirstOrDefaultAsync(c => c.Id == submissionId);
            if (submission == null)
            {
                response.Data = -1;
                response.Message = "There is no submission with this Id";
                response.Success = false;
                return response;
            }
            if (graderId == -1)
            {
                return await getStudentsAverage(submissionId);
            }
            else
            {
                User grader = await _context.Users.FirstOrDefaultAsync(c => c.Id == graderId);
                if (grader == null)
                {
                    response.Data = -1;
                    response.Message = "There is no user with this Id";
                    response.Success = false;
                    return response;
                }
                Comment comment = submission.Comments.FirstOrDefault(c => c.CommentedUserId == grader.Id);
                if (comment == null || comment.MaxGrade == 0)
                {
                    response.Data = -1;
                    response.Message = "This user did not send his feedback grade for this assignment";
                    response.Success = false;
                    return response;
                }
                response.Data = Decimal.Round(comment.Grade / comment.MaxGrade * 100, 2);
                return response;
            }
        }

        public async Task<ServiceResponse<decimal>> getStudentsAverage(int submissionId)
        {
            ServiceResponse<decimal> response = new ServiceResponse<decimal>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions
                .Include(s => s.AffiliatedAssignment).ThenInclude(a => a.AfilliatedCourse).ThenInclude(c => c.Instructors)
                .Include(s => s.AffiliatedGroup).ThenInclude(pg => pg.GroupMembers)
                .Include(s => s.Comments).Include(s => s.AffiliatedAssignment).FirstOrDefaultAsync(s => s.Id == submissionId);
            if (submission == null)
            {
                response.Data = -1;
                response.Message = "There is no such submission";
                response.Success = false;
                return response;
            }
            Course course = submission.AffiliatedAssignment.AfilliatedCourse;
            List<int> intrs = new List<int>();
            intrs = course.Instructors.Select(cu => cu.UserId).ToList();
            List<decimal> grades = _context.Comments.Include(c => c.CommentedUser)
                .Where(c => c.CommentedSubmissionId == submissionId)
                .Where(c => c.CommentedUser.UserType == UserTypeClass.Student)
                .Where(c => !intrs.Contains(c.CommentedUserId))
                .Where(c => c.MaxGrade != 0)
                .Select(c => c.Grade / c.MaxGrade * 100).ToList();
            if (grades.Count > 0)
            {
                response.Data = Decimal.Round(grades.Average(), 2);
                return response;
            }
            response.Data = -1;
            return response;
        }

        public async Task<ServiceResponse<string>> EnterSrsGrade(decimal srsGrade, int submissionId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions
            .Include(s => s.AffiliatedAssignment).ThenInclude(a => a.AfilliatedCourse).ThenInclude(c => c.Instructors)
            .FirstOrDefaultAsync(s => s.Id == submissionId);
            if (submission == null)
            {
                response.Data = "not found";
                response.Message = "There is no submission with this Id";
                response.Success = false;
                return response;
            }
            if (!submission.AffiliatedAssignment.AfilliatedCourse.Instructors.Any(i => i.UserId == user.Id))
            {
                response.Data = "not authorized";
                response.Message = "You are not authorized for this endpoint";
                response.Success = false;
                return response;
            }
            submission.IsGraded = true;
            submission.SrsGrade = srsGrade;
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync();
            response.Data = "succesfull";
            response.Message = "You succesfully entered srs grade for this submission";
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteSrsGrade(int submissionId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions
            .Include(s => s.AffiliatedAssignment).ThenInclude(a => a.AfilliatedCourse).ThenInclude(c => c.Instructors)
            .FirstOrDefaultAsync(s => s.Id == submissionId);
            if (submission == null)
            {
                response.Data = "not found";
                response.Message = "There is no submission with this Id";
                response.Success = false;
                return response;
            }
            if (!submission.AffiliatedAssignment.AfilliatedCourse.Instructors.Any(i => i.UserId == user.Id))
            {
                response.Data = "not authorized";
                response.Message = "You are not authorized for this endpoint";
                response.Success = false;
                return response;
            }
            submission.IsGraded = false;
            submission.SrsGrade = 0;
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync();
            response.Data = "succesfull";
            response.Message = "You succesfully deleted the srs grade for this submission";
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<decimal>> GetSrsGrade(int submissionId)
        {
            ServiceResponse<decimal> response = new ServiceResponse<decimal>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Submission submission = await _context.Submissions
            .Include(s => s.AffiliatedAssignment).ThenInclude(a => a.AfilliatedCourse).ThenInclude(c => c.Instructors)
            .Include(s => s.AffiliatedGroup)
            .FirstOrDefaultAsync(s => s.Id == submissionId);
            if (submission == null)
            {
                response.Data = 0;
                response.Message = "There is no submission with this Id";
                response.Success = false;
                return response;
            }
            Assignment assignment = submission.AffiliatedAssignment;
            Course course = assignment.AfilliatedCourse;
            ProjectGroup projectGroup = submission.AffiliatedGroup;

            if (!assignment.VisibilityOfSubmission &&
                !course.Instructors.Any(i => i.UserId == user.Id) &&
                !projectGroup.GroupMembers.Any(m => m.UserId == user.Id))
            {
                response.Data = 0;
                response.Message = "You are not authorized for this endpoint";
                response.Success = false;
                return response;
            }
            response.Data = submission.SrsGrade;
            response.Message = "You succesfully deleted the srs grade for this submission";
            response.Success = true;
            return response;
        }
        public async Task<ServiceResponse<FeedbacksDto>> GetNewFeedbacks()
        {
            ServiceResponse<FeedbacksDto> response = new ServiceResponse<FeedbacksDto>();
            FeedbacksDto data = new FeedbacksDto();
            List<NewFeedbacksDto> InstructorComments = new List<NewFeedbacksDto>();
            List<NewFeedbacksDto> TAComments = new List<NewFeedbacksDto>();
            List<NewFeedbacksDto> StudentComments = new List<NewFeedbacksDto>();
            User user = await _context.Users
                .Include(u => u.ProjectGroups).ThenInclude(pgu => pgu.ProjectGroup).ThenInclude(pg => pg.Submissions).ThenInclude(s => s.AffiliatedAssignment)
                .Include(u => u.ProjectGroups).ThenInclude(pgu => pgu.ProjectGroup).ThenInclude(pg => pg.AffiliatedCourse)
                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            foreach (ProjectGroup pg in user.ProjectGroups.Select(pgu => pgu.ProjectGroup))
            {
                if (pg != null)
                {
                    foreach (Submission s in pg.Submissions)
                    {
                        ServiceResponse<List<GetCommentDto>> comments = await GetInstructorComments(s.Id);
                        InstructorComments.AddRange(
                               comments.Data.Select(comment => new NewFeedbacksDto
                               {
                                   submission = new FeedbackSubmissionItem
                                   {
                                       submissionId = s.Id,
                                       assignmentName = s.AffiliatedAssignment.Title,
                                       projectId = pg.Id
                                   },
                                   course = new FeedbackCourseItem
                                   {
                                       courseName = pg.AffiliatedCourse.Name
                                   },
                                   feedback = new FeedbackItemDto
                                   {
                                       caption = comment.CommentText,
                                       date = comment.CreatedAt,
                                       commentId = comment.Id,
                                       grade = decimal.Round(comment.Grade / comment.MaxGrade * 10, 2)
                                   },
                                   user = new UserFeedbackDto
                                   {
                                       name = user.Name
                                   }
                               })
                        );
                        ServiceResponse<List<GetCommentDto>> comments2 = await GetTaComments(s.Id);
                        TAComments.AddRange(
                               comments.Data.Select(comment => new NewFeedbacksDto
                               {
                                   submission = new FeedbackSubmissionItem
                                   {
                                       submissionId = s.Id,
                                       assignmentName = s.AffiliatedAssignment.Title,
                                       projectId = pg.Id
                                   },
                                   course = new FeedbackCourseItem
                                   {
                                       courseName = pg.AffiliatedCourse.Name
                                   },
                                   feedback = new FeedbackItemDto
                                   {
                                       caption = comment.CommentText,
                                       date = comment.CreatedAt,
                                       commentId = comment.Id,
                                       grade = decimal.Round(comment.Grade / comment.MaxGrade * 10, 2)
                                   },
                                   user = new UserFeedbackDto
                                   {
                                       name = user.Name
                                   }
                               })
                        );
                        ServiceResponse<List<GetCommentDto>> comments3 = await GetStudentComments(s.Id);
                        StudentComments.AddRange(
                               comments.Data.Select(comment => new NewFeedbacksDto
                               {
                                   submission = new FeedbackSubmissionItem
                                   {
                                       submissionId = s.Id,
                                       assignmentName = s.AffiliatedAssignment.Title,
                                       projectId = pg.Id
                                   },
                                   course = new FeedbackCourseItem
                                   {
                                       courseName = pg.AffiliatedCourse.Name
                                   },
                                   feedback = new FeedbackItemDto
                                   {
                                       caption = comment.CommentText,
                                       date = comment.CreatedAt,
                                       commentId = comment.Id,
                                       grade = decimal.Round(comment.Grade / comment.MaxGrade * 10, 2)
                                   },
                                   user = new UserFeedbackDto
                                   {
                                       name = user.Name
                                   }
                               })
                        );
                    }
                }
            }
            data.InstructorComments = InstructorComments;
            data.TAFeedbacks = TAComments;
            data.StudentsFeedbacks = StudentComments;
            response.Data = data;
            return response;
        }
    }
}