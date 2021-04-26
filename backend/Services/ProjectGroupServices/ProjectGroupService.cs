using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.ProjectGroup;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.ProjectGroupServices
{
    public class ProjectGroupService : IProjectGroupService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectGroupService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<GetProjectGroupDto>> GetProjectGroupById(int id)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .Include(c => c.GroupMembers ).ThenInclude(cs => cs.User)
                .Include(c => c.AffiliatedCourse)
                .Include(c => c.AffiliatedSection)
                .FirstOrDefaultAsync( c => c.Id == id );
            if ( dbProjectGroup == null ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Project group not found.";
                return serviceResponse;
            }
            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbProjectGroup);
            return serviceResponse;
        }
    }
}