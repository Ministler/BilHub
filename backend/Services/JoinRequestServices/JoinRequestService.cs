using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using BilHub.Models;
using BilHub.Data;
using BilHub.Dtos.JoinRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.JoinRequestServices
{
    public class JoinRequestService : IJoinRequestService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public JoinRequestService( DataContext context, IHttpContextAccessor httpContextAccessor ) 
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<string>> SendJoinRequest(AddJoinRequestDto newJoinRequest) 
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup requestedGroup = await _context.ProjectGroups.FirstOrDefaultAsync( rg =>  rg.Id == newJoinRequest.ProjectGroupId);
            //Section sectionOfUser = await _context.Sections.FirstOrDefaultAsync(s => ( s.Id == requestedGroup.AffiliatedSection.Id && s.Students.Contains ));
            

            // a
            //if( requestedGroup == null || user == null || sectionOfUser == null ) 

            return response;
        }

    }
}