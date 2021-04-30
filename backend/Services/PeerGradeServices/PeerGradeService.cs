using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using backend.Models;
using backend.Data;
using backend.Dtos.PeerGrade;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Text;
using backend.Services.ProjectGroupServices;

// edit will be edited

namespace backend.Services.PeerGradeServices
{
    public class PeerGradeService : IPeerGradeService
    {
        // peer grade due date doesn't exist
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;

        public PeerGradeService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        private bool IsUserInGroup ( ProjectGroup projectGroup, int userId ) 
        {
            foreach ( var i in projectGroup.GroupMembers ) {
                if ( i.UserId == userId )
                    return true;
            }
            return false;
        }

        private bool doesUserInstruct ( User user, int courseId ) 
        {
            foreach ( var i in user.InstructedCourses ) {
                if ( i.CourseId == courseId )
                    return true;
            }
            return false;
        }

        public async Task<ServiceResponse<AddPeerGradeDto>> AddPeerGrade(AddPeerGradeDto addPeerGradeDto)
        {
            ServiceResponse<AddPeerGradeDto> response = new ServiceResponse<AddPeerGradeDto>();
            User user = await _context.Users.Include(u => u.ProjectGroups)
                                .ThenInclude(g => g.ProjectGroup)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups.Include(g => g.AffiliatedCourse).ThenInclude( c => c.PeerGradeAssignment)
                                            .Include(g => g.GroupMembers)
                                            .FirstOrDefaultAsync(rg => rg.Id == addPeerGradeDto.ProjectGroupId );

            if( projectGroup == null )
            {
                response.Data = null;
                response.Message = "There is no project group with this id";
                response.Success = false;
                return response;
            }

            if( !IsUserInGroup( projectGroup, GetUserId() ) )
            {
                response.Data = null;
                response.Message = "You are not in this group";
                response.Success = false;
                return response;
            }

            if( !IsUserInGroup( projectGroup, addPeerGradeDto.RevieweeId ) )
            {
                response.Data = null;
                response.Message = "You are not in the same group with the reviewee ";
                response.Success = false;
                return response;
            }

            if( projectGroup.AffiliatedCourse.PeerGradeAssignment == null )
            {
                response.Data = null;
                response.Message = "Course doesn't have a peer grade assignment";
                response.Success = false;
                return response;
            }
            
            if( addPeerGradeDto.MaxGrade < addPeerGradeDto.Grade )
            {
                response.Data = null;
                response.Message = "Grade should be less than or equal to the max grade";
                response.Success = false;
                return response;
            }

            if( addPeerGradeDto.MaxGrade < 0 )
            {
                response.Data = null;
                response.Message = "Max grade should not be negative";
                response.Success = false;
                return response;
            }

            PeerGrade peerGrade = await _context.PeerGrades
                .FirstOrDefaultAsync( pg => pg.ReviewerId == GetUserId() && pg.RevieweeId == addPeerGradeDto.RevieweeId && pg.ProjectGroupId == addPeerGradeDto.ProjectGroupId );

            if( peerGrade != null) {
                response.Data = null;
                response.Message = "You have already peer graded this group member";
                response.Success = false;
                return response;
            }

            PeerGrade createdPeerGrade = new PeerGrade
            {
                MaxGrade = addPeerGradeDto.MaxGrade,
                Grade = addPeerGradeDto.Grade,
                Comment = addPeerGradeDto.Comment,
                CreatedAt = addPeerGradeDto.CreatedAt,
                ProjectGroupId = addPeerGradeDto.ProjectGroupId,
                ReviewerId = GetUserId(),
                RevieweeId = addPeerGradeDto.RevieweeId
            };

            _context.PeerGrades.Add( createdPeerGrade );
            await _context.SaveChangesAsync();

            response.Data = addPeerGradeDto;
            response.Message = "You successfully entered peer grade";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<PeerGradeInfoDto>> EditPeerGrade(EditPeerGradeDto editPeerGradeDto)
        {
            ServiceResponse<PeerGradeInfoDto> response = new ServiceResponse<PeerGradeInfoDto>();
            User user = await _context.Users.Include(u => u.ProjectGroups)
                                .ThenInclude(g => g.ProjectGroup)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            /*ProjectGroup projectGroup = await _context.ProjectGroups.Include(g => g.AffiliatedCourse).ThenInclude( c => c.PeerGradeAssignment)
                                            .Include(g => g.GroupMembers)
                                            .FirstOrDefaultAsync(rg => rg.Id == editPeerGradeDto.ProjectGroupId );

            if( projectGroup == null )
            {
                response.Data = null;
                response.Message = "There is no project group with this id";
                response.Success = false;
                return response;
            }

            if( !IsUserInGroup( projectGroup, GetUserId() ) )
            {
                response.Data = null;
                response.Message = "You are not in this group";
                response.Success = false;
                return response;
            }

            if( !IsUserInGroup( projectGroup, editPeerGradeDto.RevieweeId ) )
            {
                response.Data = null;
                response.Message = "You are not in the same group with the reviewee ";
                response.Success = false;
                return response;
            }

            if( projectGroup.AffiliatedCourse.PeerGradeAssignment == null )
            {
                response.Data = null;
                response.Message = "Course doesn't have a peer grade assignment";
                response.Success = false;
                return response;
            }
            

            PeerGrade peerGrade = await _context.PeerGrades
                .FirstOrDefaultAsync( pg => pg.ReviewerId == GetUserId() && pg.RevieweeId == editPeerGradeDto.RevieweeId && pg.ProjectGroupId == editPeerGradeDto.ProjectGroupId );
            */

            PeerGrade peerGrade = await _context.PeerGrades.FirstOrDefaultAsync( pg => pg.Id == editPeerGradeDto.Id );

            if( peerGrade == null) {
                response.Data = null;
                response.Message = "There is no peer grade with this Id.";
                response.Success = false;
                return response;
            }

             if( editPeerGradeDto.MaxGrade < editPeerGradeDto.Grade )
            {
                response.Data = null;
                response.Message = "Grade should be less than or equal to the max grade";
                response.Success = false;
                return response;
            }

            peerGrade.MaxGrade = editPeerGradeDto.MaxGrade; // ask
            peerGrade.Grade = editPeerGradeDto.Grade;
            if( editPeerGradeDto.Comment != null)
                peerGrade.Comment = editPeerGradeDto.Comment;
            peerGrade.CreatedAt = editPeerGradeDto.CreatedAt; // ask
            
            _context.PeerGrades.Update( peerGrade );
            await _context.SaveChangesAsync();

           

            PeerGradeInfoDto peerGradeInfoDto = new PeerGradeInfoDto
            {
                Id = peerGrade.Id,
                ProjectGroupId = peerGrade.ProjectGroupId,
                ReviewerId = peerGrade.ReviewerId,
                RevieweeId = peerGrade.RevieweeId,
                MaxGrade = peerGrade.MaxGrade,
                Grade = peerGrade.Grade,
                Comment = peerGrade.Comment,
                CreatedAt = peerGrade.CreatedAt
            };

            response.Data = peerGradeInfoDto;
            response.Message = "You successfully entered peer grade";
            response.Success = true;



            return response;
        }

        public async Task<ServiceResponse<string>> DeletePeerGrade(DeletePeerGradeDto deletePeerGradeDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            PeerGrade peerGrade = await _context.PeerGrades
                                            .FirstOrDefaultAsync(pg => pg.Id == deletePeerGradeDto.Id);

            if (peerGrade == null)
            {
                response.Data = "Not allowed";
                response.Message = "There is no peer grade with this Id";
                response.Success = false;
                return response;
            }


            if ( user == null || ( user != null && peerGrade.ReviewerId != GetUserId() ) )
            {
                response.Data = "Not allowed";
                response.Message = "You are not authorized to delete this peer grade";
                response.Success = false;
                return response;
            }

            // due date olursa kontrol

            _context.PeerGrades.Remove( peerGrade );
            await _context.SaveChangesAsync();

            response.Data = "Successful";
            response.Message = "Peer grade is successfully cancelled";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<PeerGradeInfoDto>> GetPeerGradeById(GetPeerGradeByIdDto getPeerGradeDto)
        {
            ServiceResponse<PeerGradeInfoDto> response = new ServiceResponse<PeerGradeInfoDto>();
            User user = await _context.Users
                                .Include( u => u.InstructedCourses )
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            PeerGrade peerGrade = await _context.PeerGrades.FirstOrDefaultAsync( pg => pg.Id == getPeerGradeDto.Id );
            
            if( peerGrade == null)
            {
                response.Data = null;
                response.Message = "There is no peer grade with this id";
                response.Success =false;
                return response;
            }
            
            PeerGradeInfoDto dto = new PeerGradeInfoDto
            {
                Id = peerGrade.Id,
                ProjectGroupId = peerGrade.ProjectGroupId,
                ReviewerId = peerGrade.ReviewerId,
                RevieweeId = peerGrade.RevieweeId,
                MaxGrade = peerGrade.MaxGrade,
                Grade = peerGrade.Grade,
                Comment = peerGrade.Comment,
                CreatedAt = peerGrade.CreatedAt
            };

            ProjectGroup projectGroup = await _context.ProjectGroups
                                            .FirstOrDefaultAsync(rg => rg.Id == dto.ProjectGroupId );
            

            if( user == null || ( !doesUserInstruct( user, projectGroup.AffiliatedCourseId ) && user.Id != dto.ReviewerId ) )
            {
                response.Data = null;
                response.Message = "You are not authorized to see this peer grade";
                response.Success = false;
                return response;
            }

            response.Data = dto;
            response.Message = "Success";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<PeerGradeInfoDto>> GetPeerGradeByUsersAndGroup(GetPeerGradeDto getPeerGradeDto)
        {
            ServiceResponse<PeerGradeInfoDto> response = new ServiceResponse<PeerGradeInfoDto>();
            User user = await _context.Users
                                .Include( u => u.InstructedCourses )
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups.Include(g => g.AffiliatedCourse).ThenInclude( c => c.PeerGradeAssignment)
                                            .Include(g => g.GroupMembers)
                                            .FirstOrDefaultAsync(rg => rg.Id == getPeerGradeDto.ProjectGroupId );
            

            if( projectGroup == null )
            {
                response.Data = null;
                response.Message = "There is no project group with this id";
                response.Success = false;
                return response;
            }

            if( !IsUserInGroup( projectGroup, getPeerGradeDto.ReviewerId) )
            {
                response.Data = null;
                response.Message = "There is no user with reviewerId in group";
                response.Success = false;
                return response;
            }

            if( !IsUserInGroup( projectGroup, getPeerGradeDto.RevieweeId ) )
            {
                response.Data = null;
                response.Message = "There is no user with revieweeId in group";
                response.Success = false;
                return response;
            }

            if( projectGroup.AffiliatedCourse.PeerGradeAssignment == null )
            {
                response.Data = null;
                response.Message = "Course doesn't have a peer grade assignment";
                response.Success = false;
                return response;
            }


            
            if( user == null || ( !doesUserInstruct( user, projectGroup.AffiliatedCourseId ) && user.Id != getPeerGradeDto.ReviewerId ) )
            {
                response.Data = null;
                response.Message = "You are not authorized to see this peer grade";
                response.Success = false;
                return response;
            }
            

            PeerGrade peerGrade = await _context.PeerGrades
                .FirstOrDefaultAsync( pg => pg.ReviewerId == getPeerGradeDto.ReviewerId && pg.RevieweeId == getPeerGradeDto.RevieweeId && pg.ProjectGroupId == getPeerGradeDto.ProjectGroupId );

            if( peerGrade == null) {
                response.Data = null;
                response.Message = "There is no such peer grade";
                response.Success = false;
                return response;
            }
            
            PeerGradeInfoDto dto = new PeerGradeInfoDto
            {
                Id = peerGrade.Id,
                ProjectGroupId = peerGrade.ProjectGroupId,
                ReviewerId = peerGrade.ReviewerId,
                RevieweeId = peerGrade.RevieweeId,
                MaxGrade = peerGrade.MaxGrade,
                Grade = peerGrade.Grade,
                Comment = peerGrade.Comment,
                CreatedAt = peerGrade.CreatedAt
            };

            response.Data = dto;
            response.Message = "Success";
            response.Success = true;

            return response;
        }
    }
}