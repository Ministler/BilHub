using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.Assignment;
using backend.Dtos.Course;
using backend.Dtos.ProjectGroup;
using backend.Models;
using backend.Services.AssignmentServices;
using backend.Services.ProjectGroupServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.CourseServices
{
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAssignmentService _assignmentService;
        private readonly IProjectGroupService _projectGroupService;
        public CourseService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IAssignmentService assignmentService, IProjectGroupService projectGroupService)
        {
            _projectGroupService = projectGroupService;
            _assignmentService = assignmentService;
            _httpContextAccessor = httpContextAccessor;
            _projectGroupService = projectGroupService;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<GetCourseDto>> CreateCourse(CreateCourseDto createCourseDto)
        {
            ServiceResponse<GetCourseDto> serviceResponse = new ServiceResponse<GetCourseDto>();

            User dbUser = await _context.Users
                .Include(c => c.InstructedCourses)
                .FirstOrDefaultAsync(c => c.Id == GetUserId());



            if (dbUser == null || dbUser.UserType == UserTypeClass.Student)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not in instructor list. If you think this is incorrect, please contact the devs.";
                return serviceResponse;
            }
            if (createCourseDto.MaxGroupSize < 1 || createCourseDto.MinGroupSize > createCourseDto.MaxGroupSize)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Minumum maxgroupsize should be 1 and minGroupsize should be less than or equal to maxgroupsize";
                return serviceResponse;
            }

            SemesterType semesterType = SemesterType.Spring;
            if (createCourseDto.CourseSemester.Equals("Spring"))
                semesterType = SemesterType.Spring;
            else if (createCourseDto.CourseSemester.Equals("Summer"))
                semesterType = SemesterType.Summer;
            else if (createCourseDto.CourseSemester.Equals("Fall"))
                semesterType = SemesterType.Fall;
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Semester type is given wrong.";
                return serviceResponse;
            }

            Course newCourse = new Course
            {
                Name = createCourseDto.Name,
                CourseSemester = semesterType,
                Year = createCourseDto.Year,
                CourseInformation = createCourseDto.CourseInformation,
                NumberOfSections = createCourseDto.NumberOfSections,
                LockDate = createCourseDto.LockDate,
                MinGroupSize = createCourseDto.MinGroupSize,
                MaxGroupSize = createCourseDto.MaxGroupSize,
                StartDate = DateTime.Now,
                IsSectionless = createCourseDto.IsSectionless
            };

            CourseUser founderInstructor = new CourseUser
            {
                User = dbUser,
                UserId = dbUser.Id,
                Course = newCourse,
                CourseId = newCourse.Id
            };

            newCourse.Instructors.Add(founderInstructor);

            for (int i = 1; i <= createCourseDto.NumberOfSections; i++)
            {
                Section newSection = new Section
                {
                    SectionNo = i,
                    AffiliatedCourse = newCourse,
                    AffiliatedCourseId = newCourse.Id
                };
                newCourse.Sections.Add(newSection);
            }

            await _context.Courses.AddAsync(newCourse);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetCourseDto>(newCourse);

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCourseDto>> GetCourse(int courseId)
        {
            ServiceResponse<GetCourseDto> serviceResponse = new ServiceResponse<GetCourseDto>();

            Course dbCourse = await _context.Courses
                .Include(c => c.Instructors).ThenInclude(cs => cs.User)
                .Include(c => c.Sections)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (dbCourse == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Course with given courseId not found.";
                return serviceResponse;
            }

            serviceResponse.Data = _mapper.Map<GetCourseDto>(dbCourse);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCourseDto>> EditCourse(EditCourseDto editCourseDto)
        {
            ServiceResponse<GetCourseDto> serviceResponse = new ServiceResponse<GetCourseDto>();

            User dbUser = await _context.Users
                .Include(c => c.InstructedCourses)
                .FirstOrDefaultAsync(c => c.Id == GetUserId());

            if (dbUser == null || dbUser.UserType == UserTypeClass.Student)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not in instructor list. If you think this is incorrect, please contact the devs.";
                return serviceResponse;
            }
            if (editCourseDto.MaxGroupSize < 1 || editCourseDto.MinGroupSize > editCourseDto.MaxGroupSize)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Minumum maxgroupsize should be 1 and minGroupsize should be less than or equal to maxgroupsize";
                return serviceResponse;
            }

            Course dbCourse = await _context.Courses
                .Include(c => c.Instructors).ThenInclude(cs => cs.User)
                .Include(c => c.Sections)
                .FirstOrDefaultAsync(c => c.Id == editCourseDto.Id);

            if (dbCourse == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Course not found.";
                return serviceResponse;
            }
            if (!dbCourse.Instructors.Any(c => c.UserId == dbUser.Id))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User does not have authority on this course.";
                return serviceResponse;
            }

            SemesterType semesterType = SemesterType.Spring;
            if (editCourseDto.CourseSemester.Equals("Spring"))
                semesterType = SemesterType.Spring;
            else if (editCourseDto.CourseSemester.Equals("Summer"))
                semesterType = SemesterType.Summer;
            else if (editCourseDto.CourseSemester.Equals("Fall"))
                semesterType = SemesterType.Fall;
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Semester type is given wrong.";
                return serviceResponse;
            }

            dbCourse.Name = editCourseDto.Name;
            dbCourse.CourseSemester = semesterType;
            dbCourse.Year = editCourseDto.Year;
            dbCourse.CourseInformation = editCourseDto.CourseInformation;
            dbCourse.LockDate = editCourseDto.LockDate;
            dbCourse.MinGroupSize = editCourseDto.MinGroupSize;
            dbCourse.MaxGroupSize = editCourseDto.MaxGroupSize;

            _context.Courses.Update(dbCourse);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetCourseDto>(dbCourse);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCourseDto>> AddInstructorToCourse(int userId, int courseId)
        {
            ServiceResponse<GetCourseDto> serviceResponse = new ServiceResponse<GetCourseDto>();

            User dbUser = await _context.Users
                .Include(c => c.InstructedCourses)
                .FirstOrDefaultAsync(c => c.Id == userId);

            Course dbCourse = await _context.Courses
                .Include(c => c.Instructors).ThenInclude(cs => cs.User)
                .Include(c => c.Sections)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (dbCourse == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Course not found.";
                return serviceResponse;
            }
            if (dbUser == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found.";
                return serviceResponse;
            }
            if (!dbCourse.Instructors.Any(c => c.UserId == GetUserId()))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User does not have authority on this course.";
                return serviceResponse;
            }
            if (dbCourse.Instructors.Any(c => c.UserId == userId))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User already an instructor/TA in this course.";
                return serviceResponse;
            }

            CourseUser newInstructor = new CourseUser
            {
                User = dbUser,
                UserId = dbUser.Id,
                Course = dbCourse,
                CourseId = dbCourse.Id
            };
            dbCourse.Instructors.Add(newInstructor);
            _context.Courses.Update(dbCourse);

            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetCourseDto>(dbCourse);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCourseDto>> RemoveInstructorFromCourse(int userId, int courseId)
        {
            ServiceResponse<GetCourseDto> serviceResponse = new ServiceResponse<GetCourseDto>();

            User dbUser = await _context.Users
                .Include(c => c.InstructedCourses)
                .FirstOrDefaultAsync(c => c.Id == userId);

            Course dbCourse = await _context.Courses
                .Include(c => c.Instructors).ThenInclude(cs => cs.User)
                .Include(c => c.Sections)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (dbCourse == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Course not found.";
                return serviceResponse;
            }
            if (dbUser == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found.";
                return serviceResponse;
            }
            if (!dbCourse.Instructors.Any(c => c.UserId == GetUserId()))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User does not have authority on this course.";
                return serviceResponse;
            }
            if (!dbCourse.Instructors.Any(c => c.UserId == userId))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User that is trying to be removed is not an instructor/TA in this course.";
                return serviceResponse;
            }
            if (dbCourse.Instructors.Count == 1)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "This user is the only instructor left in this course. Thus, it is not permitted to remove him from the course. Please try deleting the course if you want to.";
                return serviceResponse;
            }

            foreach (var i in dbCourse.Instructors)
            {
                if (i.UserId == userId)
                {
                    _context.CourseUsers.Remove(i);
                    break;
                }
            }

            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetCourseDto>(dbCourse);
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> RemoveCourse(int courseId)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            Course dbCourse = await _context.Courses
                .Include(c => c.Instructors).ThenInclude(cs => cs.User)
                .Include(c => c.Sections)
                .Include(c => c.Assignments)
                .Include(c => c.PeerGradeAssignment)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (dbCourse == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Course not found.";
                return serviceResponse;
            }

            if (!dbCourse.Instructors.Any(c => c.UserId == GetUserId()))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User does not have authority on this course to remove the course.";
                return serviceResponse;
            }

            _context.PeerGradeAssignments.Remove(dbCourse.PeerGradeAssignment);

            foreach (var i in dbCourse.Instructors)
                _context.CourseUsers.Remove(i);

            foreach (var i in dbCourse.Assignments)
            {
                await _assignmentService.DeleteAssignment(new DeleteAssignmentDto
                {
                    AssignmentId = i.Id
                });
            }

            foreach (var i in dbCourse.Sections)
            {
                List<ProjectGroup> toBeDeletedGroups = await _context.ProjectGroups
                    .Where ( c => c.AffiliatedSectionId == i.Id ).ToListAsync();
                foreach ( var j in toBeDeletedGroups )
                {
                    await _projectGroupService.DeleteProjectGroup ( j.Id );
                }
            }
            _context.Sections.RemoveRange( dbCourse.Sections );

            _context.Courses.Remove(dbCourse);
            await _context.SaveChangesAsync();

            serviceResponse.Data = "Successfully deleted the course";
            serviceResponse.Message = "Successfully deleted the course";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetOzgurDto>> GetOzgur(int courseId)
        {
            ServiceResponse<GetOzgurDto> response = new ServiceResponse<GetOzgurDto>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            Course course = await _context.Courses
                .Include(pg => pg.Sections).ThenInclude(s => s.ProjectGroups).ThenInclude(c => c.ProjectGrades).ThenInclude(pg => pg.GradingUser)
                .Include(pg => pg.Instructors)//.ThenInclude(s => s.AffiliatedAssignment)
                .FirstOrDefaultAsync(s => s.Id == courseId);
            if (course == null)
            {
                response.Data = null;
                response.Message = "There is no such course";
                response.Success = false;
                return response;
            }
            List<Section> sections = course.Sections.ToList();
            List<ProjectGroup> projectGroups = new List<ProjectGroup>();
            foreach (Section s in sections)
            {
                List<ProjectGroup> tempGroups = s.ProjectGroups.ToList();
                projectGroups.AddRange(tempGroups);
            }

            List<int> instrs = course.Instructors.Select(i => i.UserId).ToList();
            HashSet<int> graderIds = new HashSet<int>();
            foreach (ProjectGroup pg in projectGroups)
            {
                List<ProjectGrade> projectGrades = pg.ProjectGrades.ToList();
                foreach (ProjectGrade pgrade in projectGrades)
                {
                    if (instrs.Contains(pgrade.GradingUserId))
                        graderIds.Add(pgrade.GradingUserId);
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
            foreach (ProjectGroup pg in projectGroups)
            {
                OzgurStatDto ozgurStatDto = new OzgurStatDto();
                ozgurStatDto.StatName = pg.Name;
                ozgurStatDto.Grades = new List<decimal>();
                foreach (int id in graderIds)
                {
                    ServiceResponse<decimal> grade = await _projectGroupService.GetGradeWithGraderId(pg.Id, id);
                    ozgurStatDto.Grades.Add(grade.Data);
                }
                ozgurStatDtos.Add(ozgurStatDto);
            }
            getOzgurDto.ozgurStatDtos = ozgurStatDtos;
            // List<int> graderIds = submission.Comments.Select(c => c.CommentedUserId).ToList();
            response.Data = getOzgurDto;
            return response;
        }
    }
}