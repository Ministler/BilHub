using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.ProjectGroup;
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
                .Include(c => c.ProjectGroups).ThenInclude( cs => cs.GroupMembers ).ThenInclude( css => css.User )
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
            var tmp = await _projectGroupService.GetProjectGroupsOfSection ( dbSection.Id );
            if ( tmp.Success == false )
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "There might be some errors. Please check the information of section and user.";
                return serviceResponse;
            }

            serviceResponse.Data.ProjectGroups = tmp.Data;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetSectionDto>> RemoveStudentFromSection(int userId, int sectionId)
        {
            ServiceResponse<GetSectionDto> serviceResponse = new ServiceResponse<GetSectionDto>();

            Section dbSection = await _context.Sections
                .Include(c => c.AffiliatedCourse).ThenInclude ( cs => cs.Instructors )
                .Include(c => c.ProjectGroups).ThenInclude( cs => cs.GroupMembers ).ThenInclude( css => css.User )
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

            await _projectGroupService.KickStudentFromGroup ( userId, currentProjectGroupId, true );

            int newProjectGroupId = dbUser.ProjectGroups.FirstOrDefault ( c => c.ProjectGroup.AffiliatedSectionId == sectionId ).ProjectGroupId ;

            await _projectGroupService.DeleteProjectGroup ( newProjectGroupId );

            _context.Sections.Update ( dbSection );
            await _context.SaveChangesAsync();
            
            serviceResponse.Data = _mapper.Map<GetSectionDto> ( dbSection );
            var tmp = await _projectGroupService.GetProjectGroupsOfSection ( dbSection.Id );
            if ( tmp.Success == false )
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "There might be some errors. Please check the information of section and user.";
                return serviceResponse;
            }

            serviceResponse.Data.ProjectGroups = tmp.Data;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetSectionDto>> GetSectionInfo(int sectionId)
        {
            ServiceResponse<GetSectionDto> serviceResponse = new ServiceResponse<GetSectionDto>();

            Section dbSection = await _context.Sections
                .Include(c => c.AffiliatedCourse).ThenInclude ( cs => cs.Instructors )
                .Include(c => c.ProjectGroups).ThenInclude( cs => cs.GroupMembers ).ThenInclude( css => css.User )
                .FirstOrDefaultAsync(c => c.Id == sectionId);
            
            if ( dbSection == null ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Section not found.";
                return serviceResponse;
            }

            serviceResponse.Data = _mapper.Map<GetSectionDto> ( dbSection );
            var tmp = await _projectGroupService.GetProjectGroupsOfSection ( dbSection.Id );
            if ( tmp.Success == false )
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "There might be some errors. Please check the information of section and user.";
                return serviceResponse;
            }

            serviceResponse.Data.ProjectGroups = tmp.Data;
            return serviceResponse;
        }

        private async void ForceConfirm ( int projectGroupId )
        {
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .Include ( c => c.GroupMembers )
                .FirstOrDefaultAsync ( c => c.Id == projectGroupId );
            
            foreach ( var i in dbProjectGroup.GroupMembers )
                await _projectGroupService.ForceConfirmStudent(i.UserId, projectGroupId);
        }

        private async Task<int> SizeOfGroup ( int projectGroupId )
        {
            ProjectGroup tmp = await _context.ProjectGroups
                .Include(c =>c.GroupMembers)
                .FirstOrDefaultAsync( c => c.Id == projectGroupId);
            if ( tmp == null )
                return 0;
            return tmp.GroupMembers.Count;
        }
        public async Task<ServiceResponse<string>> LockGroupFormation(int sectionId)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string> ();
            
            Section dbSection = await _context.Sections
                .Include ( c => c.AffiliatedCourse )
                .Include ( c => c.ProjectGroups ).ThenInclude ( cs => cs.GroupMembers )
                .FirstOrDefaultAsync ( c => c.Id == sectionId );

            if ( dbSection == null ) 
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Section not found";
                return serviceResponse;
            }

            List<int> unformedGroups = new List<int>();
            List<int> formedGroups = new List<int>();
            foreach ( var i in dbSection.ProjectGroups ) {
                
                if ( i.GroupMembers.Count >= dbSection.AffiliatedCourse.MinGroupSize && i.GroupMembers.Count <= dbSection.AffiliatedCourse.MaxGroupSize )
                {
                    ForceConfirm( i.Id );
                    formedGroups.Add ( i.Id );
                }
                else {
                    if ( i.ConfirmationState )
                        formedGroups.Add ( i.Id );
                    else
                        unformedGroups.Add ( i.Id );
                }
            }
            
            while ( unformedGroups.Count > 1 )
            {
                bool flag = false;

                for ( int i = 0 ; i < unformedGroups.Count ; i++ )
                    for ( int j = i+1 ; j < unformedGroups.Count ; i++ ) {
                        int vali = unformedGroups.ElementAt(i);
                        int valj = unformedGroups.ElementAt(j);
                        int szi  = await SizeOfGroup( vali );
                        int szj  = await SizeOfGroup( valj );
                        if ( szi + szj <= dbSection.AffiliatedCourse.MaxGroupSize )
                        {
                            await _projectGroupService.ForceMerge ( vali, valj );
                            unformedGroups.Remove(vali);
                            if ( szi + szj >= dbSection.AffiliatedCourse.MaxGroupSize )
                            {
                                ForceConfirm(valj);
                                unformedGroups.Remove(valj);
                            }
                            flag = true;
                            break;
                        }
                    }

                if ( flag == false )
                    break;

            }

            serviceResponse.Data = "Possible merges are done.";
            return serviceResponse;
        }
    }
}