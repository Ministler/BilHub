using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.Course;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.CourseServices
{
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CourseService( IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor )
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<GetCourseDto>> CreateCourse(CreateCourseDto createCourseDto)
        {
            ServiceResponse<GetCourseDto> serviceResponse = new ServiceResponse<GetCourseDto>();

            User dbUser = await _context.Users
                .Include ( c => c.InstructedCourses )
                .FirstOrDefaultAsync ( c => c.Id == GetUserId() );



            if ( dbUser == null || !Utility.CheckIfInstructorEmail (dbUser.Email) ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not in instructor list. If you think this is incorrect, please contact the devs.";
                return serviceResponse;
            }
            
            SemesterType semesterType = SemesterType.Spring;
            if ( createCourseDto.CourseSemester.Equals("Spring") )
                semesterType = SemesterType.Spring;
            else if ( createCourseDto.CourseSemester.Equals("Summer") )
                semesterType = SemesterType.Summer;
            else if ( createCourseDto.CourseSemester.Equals("Fall") )
                semesterType = SemesterType.Fall;
            else {
                serviceResponse.Success = false;
                serviceResponse.Message = "Semester type is given wrong.";
                return serviceResponse;
            }

            Course newCourse = new Course {
                Name = createCourseDto.Name,
                CourseSemester = semesterType,
                Year = createCourseDto.Year,
                CourseInformation = createCourseDto.CourseInformation,
                NumberOfSections = createCourseDto.NumberOfSections,
                LockDate = createCourseDto.LockDate,
                MinGroupSize = createCourseDto.MinGroupSize,
                MaxGroupSize = createCourseDto.MaxGroupSize,
                StartDate = DateTime.Now
            };

            CourseUser founderInstructor = new CourseUser {
                User = dbUser,
                UserId = dbUser.Id,
                Course = newCourse,
                CourseId = newCourse.Id
            };

            newCourse.Instructors.Add ( founderInstructor );

            for ( int i = 1 ; i <= createCourseDto.NumberOfSections ; i++ ) {
                Section newSection = new Section {
                    SectionNo = i,
                    AffiliatedCourse = newCourse,
                    AffiliatedCourseId = newCourse.Id
                };
                newCourse.Sections.Add ( newSection );
            }

            await _context.Courses.AddAsync ( newCourse );
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetCourseDto>(newCourse);

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCourseDto>> GetCourse(int courseId)
        {
            ServiceResponse<GetCourseDto> serviceResponse = new ServiceResponse<GetCourseDto>();

            Course dbCourse = await _context.Courses
                .Include ( c => c.Instructors ).ThenInclude( cs => cs.User )
                .Include ( c => c.Sections )
                .FirstOrDefaultAsync( c => c.Id == courseId );

            if ( dbCourse == null ) {
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
                .Include ( c => c.InstructedCourses )
                .FirstOrDefaultAsync ( c => c.Id == GetUserId() );

            if ( dbUser == null || !Utility.CheckIfInstructorEmail (dbUser.Email) ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not an instructor type user. If you think this is incorrect, please contact the devs.";
                return serviceResponse;
            }

            Course dbCourse = await _context.Courses
                .Include ( c => c.Instructors ).ThenInclude ( cs => cs.User )
                .Include ( c => c.Sections )
                .FirstOrDefaultAsync ( c => c.Id == editCourseDto.Id  );
            
            if ( dbCourse == null ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Course not found.";
                return serviceResponse;
            }
            if ( !dbCourse.Instructors.Any ( c => c.UserId == dbUser.Id ) ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User does not have authority on this course.";
                return serviceResponse;
            }

            SemesterType semesterType = SemesterType.Spring;
            if ( editCourseDto.CourseSemester.Equals("Spring") )
                semesterType = SemesterType.Spring;
            else if ( editCourseDto.CourseSemester.Equals("Summer") )
                semesterType = SemesterType.Summer;
            else if ( editCourseDto.CourseSemester.Equals("Fall") )
                semesterType = SemesterType.Fall;
            else {
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

            _context.Courses.Update ( dbCourse );
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetCourseDto>(dbCourse);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCourseDto>> AddInstructorToCourse(int userId, int courseId)
        {
            ServiceResponse<GetCourseDto> serviceResponse = new ServiceResponse<GetCourseDto>();

            User dbUser = await _context.Users
                .Include ( c => c.InstructedCourses )
                .FirstOrDefaultAsync ( c => c.Id == userId );

            Course dbCourse = await _context.Courses
                .Include ( c => c.Instructors ).ThenInclude ( cs => cs.User )
                .Include ( c => c.Sections )
                .FirstOrDefaultAsync ( c => c.Id == courseId  );
            
            if ( dbCourse == null ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Course not found.";
                return serviceResponse;
            }
            if ( dbUser == null ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found.";
                return serviceResponse;
            }
            if ( !dbCourse.Instructors.Any ( c => c.UserId == GetUserId() ) ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User does not have authority on this course.";
                return serviceResponse;
            }
            if ( dbCourse.Instructors.Any ( c => c.UserId == userId ) ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User already an instructor/TA in this course.";
                return serviceResponse;
            }

            CourseUser newInstructor = new CourseUser {
                User = dbUser,
                UserId = dbUser.Id,
                Course = dbCourse,
                CourseId = dbCourse.Id
            };
            dbCourse.Instructors.Add ( newInstructor );
            _context.Courses.Update ( dbCourse );

            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetCourseDto>(dbCourse);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCourseDto>> RemoveInstructorFromCourse(int userId, int courseId)
        {
            ServiceResponse<GetCourseDto> serviceResponse = new ServiceResponse<GetCourseDto>();

            User dbUser = await _context.Users
                .Include ( c => c.InstructedCourses )
                .FirstOrDefaultAsync ( c => c.Id == userId );

            Course dbCourse = await _context.Courses
                .Include ( c => c.Instructors ).ThenInclude ( cs => cs.User )
                .Include ( c => c.Sections )
                .FirstOrDefaultAsync ( c => c.Id == courseId  );
            
            if ( dbCourse == null ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Course not found.";
                return serviceResponse;
            }
            if ( dbUser == null ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found.";
                return serviceResponse;
            }
            if ( !dbCourse.Instructors.Any ( c => c.UserId == GetUserId() ) ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User does not have authority on this course.";
                return serviceResponse;
            }
            if ( !dbCourse.Instructors.Any ( c => c.UserId == userId ) ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User that is trying to be removed is not an instructor/TA in this course.";
                return serviceResponse;
            }
            if ( dbCourse.Instructors.Count == 1 ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "This user is the only instructor left in this course. Thus, it is not permitted to remove him from the course. Please try deleting the course if you want to.";
                return serviceResponse;
            }

            foreach ( var i in dbCourse.Instructors ) 
            {
                if ( i.UserId == userId ) {
                    _context.CourseUsers.Remove(i);
                    break;
                }
            }

            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetCourseDto>(dbCourse);
            return serviceResponse;
        }
    }
}