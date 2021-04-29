using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.Section;
using backend.Models;
using backend.Services.ProjectGroupServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.SectionServices
{
    public class SectionService : ISectionService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProjectGroupService _projectGroupService;
        public SectionService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IProjectGroupService projectGroupService)
        {
            _projectGroupService = projectGroupService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<GetSectionDto>> AddStudentToSection(int userId, int sectionId)
        {
            ServiceResponse<GetSectionDto> serviceResponse = new ServiceResponse<GetSectionDto>();

            Section dbSection = await _context.Sections
                .Include(c => c.AffiliatedCourse).ThenInclude ( cs => cs.Instructors )
                .Include(c => c.ProjectGroups)
                .FirstOrDefaultAsync(c => c.Id == sectionId);

            if ( dbSection == null ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Section not found.";
                return serviceResponse;
            }

            if ( !dbSection.AffiliatedCourse.Instructors.Any ( c => c.UserId == GetUserId() ) ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not authorized to perform this operation.";
                return serviceResponse;
            }

            User dbUser = await _context.Users
                .Include ( c => c.ProjectGroups ).ThenInclude ( cs => cs.ProjectGroup )
                .FirstOrDefaultAsync ( c => c.Id == userId );
            
            if ( dbUser == null ) 
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found.";
                return serviceResponse;
            }

            if ( dbUser.ProjectGroups != null && dbUser.ProjectGroups.Any ( c => c.ProjectGroup.AffiliatedSectionId == sectionId ) )
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Student is already in the section.";
                return serviceResponse;
            }

            if ( dbUser.ProjectGroups != null && dbUser.ProjectGroups.Any ( c => c.ProjectGroup.AffiliatedCourseId == dbSection.AffiliatedCourseId ) )
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Student is in another section of this course. In order to add the student to this section, you must first remove student from the current section and apply this operation again";
                return serviceResponse;
            }

            await _projectGroupService.CreateNewProjectGroupForStudentInSection ( userId, sectionId );

            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .Include ( c => c.GroupMembers )
                .FirstOrDefaultAsync ( c => c.AffiliatedSectionId == sectionId && c.GroupMembers.Any ( cs => cs.UserId == userId ) );
            
            if ( dbProjectGroup == null )
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "There was an error trying to add student to the section. Please try again and if problem occurs again, please contact the devs.";
                return serviceResponse;
            } 

            serviceResponse.Data = _mapper.Map<GetSectionDto> ( dbSection );
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetSectionDto>> RemoveStudentFromSection(int userId, int sectionId)
        {
            ServiceResponse<GetSectionDto> serviceResponse = new ServiceResponse<GetSectionDto>();

            Section dbSection = await _context.Sections
                .Include(c => c.AffiliatedCourse).ThenInclude ( cs => cs.Instructors )
                .Include(c => c.ProjectGroups)
                .FirstOrDefaultAsync(c => c.Id == sectionId);

            if ( dbSection == null ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Section not found.";
                return serviceResponse;
            }

            if ( !dbSection.AffiliatedCourse.Instructors.Any ( c => c.UserId == GetUserId() ) ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not authorized to perform this operation.";
                return serviceResponse;
            }

            User dbUser = await _context.Users
                .Include ( c => c.ProjectGroups ).ThenInclude ( cs => cs.ProjectGroup )
                .FirstOrDefaultAsync ( c => c.Id == userId );
            
            if ( dbUser == null ) 
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found.";
                return serviceResponse;
            }

            if (  !dbUser.ProjectGroups.Any ( c => c.ProjectGroup.AffiliatedSectionId == sectionId ) )
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Student is not in this section.";
                return serviceResponse;
            }

            int currentProjectGroupId = dbUser.ProjectGroups.FirstOrDefault ( c => c.ProjectGroup.AffiliatedSectionId == sectionId ).ProjectGroupId ;

            await _projectGroupService.KickStudentFromGroup ( userId, currentProjectGroupId );

            int newProjectGroupId = dbUser.ProjectGroups.FirstOrDefault ( c => c.ProjectGroup.AffiliatedSectionId == sectionId ).ProjectGroupId ;

            await _projectGroupService.DeleteProjectGroup ( newProjectGroupId );

            _context.Sections.Update ( dbSection );
            await _context.SaveChangesAsync();
            
            serviceResponse.Data = _mapper.Map<GetSectionDto> ( dbSection );
            return serviceResponse;
        }
    }
}