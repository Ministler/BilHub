using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using backend.Models;
using backend.Data;
using backend.Dtos.JoinRequest;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Text;


namespace backend.Services.JoinRequestServices
{
    public class JoinRequestService : IJoinRequestService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;

        public JoinRequestService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<AddJoinRequestDto>> SendJoinRequest(AddJoinRequestDto newJoinRequest)
        {
            ServiceResponse<AddJoinRequestDto> response = new ServiceResponse<AddJoinRequestDto>();
            User user = await _context.Users.Include(u => u.ProjectGroups)
                                .ThenInclude(g => g.ProjectGroup)
                                .ThenInclude(g => g.AffiliatedSection)
                                .Include(u => u.OutgoingJoinRequests)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup requestedGroup = await _context.ProjectGroups.Include(g => g.AffiliatedSection)
                                            .Include(g => g.GroupMembers).Include(g => g.IncomingJoinRequests)
                                            .FirstOrDefaultAsync(rg => rg.Id == newJoinRequest.RequestedGroupId);

            if (requestedGroup == null)
            {
                response.Data = null;
                response.Message = "There is no group with this id";
                response.Success = false;
                return response;
            }

            Section section = await _context.Sections.FirstOrDefaultAsync(s => (s.Id == requestedGroup.AffiliatedSection.Id));

            if (user == null)
            {
                response.Data = null;
                response.Message = "There is no user with this Id";
                response.Success = false;
                return response;
            }
            if (section == null)
            {
                response.Data = null;
                response.Message = "There is no section with this Id ";
                response.Success = false;
                return response;
            }


            ProjectGroup userGroup = new ProjectGroup { AffiliatedSectionId = -1 };

            foreach (ProjectGroupUser pgu in user.ProjectGroups)
            {
                if (pgu.ProjectGroup.AffiliatedCourseId == requestedGroup.AffiliatedCourseId && pgu.ProjectGroup.AffiliatedSectionId == requestedGroup.AffiliatedSectionId)
                {
                    userGroup = pgu.ProjectGroup;
                    break;
                }
            }
            if (userGroup.AffiliatedSectionId == -1)
            {
                response.Data = null;
                response.Message = "You are not in the same section with this group";
                response.Success = false;
                return response;
            }
            if (userGroup.Id == requestedGroup.Id)
            {
                response.Data = null;
                response.Message = "You are already in this group";
                response.Success = false;
                return response;
            }

            if (userGroup.ConfirmationState == true)
            {
                response.Data = null;
                response.Message = "Your group is finalized.";
                response.Success = false;
                return response;
            }
            if (requestedGroup.ConfirmationState == true)
            {
                response.Data = null;
                response.Message = "The group you want to join is finalized.";
                response.Success = false;
                return response;
            }
            if (requestedGroup.GroupMembers.Count >= _context.Courses.FirstOrDefault(c => c.Id == userGroup.AffiliatedCourseId).MaxGroupSize)
            {
                response.Data = null;
                response.Message = "The group you want to join is full.";
                response.Success = false;
                return response;
            }
            if (requestedGroup.GroupMembers.Count == 0)
            {
                response.Data = null;
                response.Message = "The group is empty";
                response.Success = false;
                return response;
            }


            foreach (JoinRequest jr in requestedGroup.IncomingJoinRequests)
            {
                if (jr.RequestingStudentId == GetUserId() && !jr.Resolved)
                {
                    response.Data = null;
                    response.Message = "You already sent a join request to this group that is not resolved yet.";
                    response.Success = false;
                    return response;
                }
            }

            JoinRequest createdJoinRequest = new JoinRequest
            {
                RequestingStudent = user,
                RequestingStudentId = user.Id,
                RequestedGroup = requestedGroup,
                RequestedGroupId = requestedGroup.Id,
                CreatedAt = newJoinRequest.CreatedAt,
                AcceptedNumber = 0,
                Accepted = false,
                Resolved = false,
                VotedStudents = "",
                Description = newJoinRequest.Description
            };

            requestedGroup.IncomingJoinRequests.Add(createdJoinRequest);
            user.OutgoingJoinRequests.Add(createdJoinRequest);

            _context.ProjectGroups.Update(requestedGroup);
            await _context.SaveChangesAsync();
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            _context.JoinRequests.Update(createdJoinRequest);
            await _context.SaveChangesAsync();

            response.Data = newJoinRequest;
            response.Message = "Join request is successfully sent";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<string>> CancelJoinRequest(CancelJoinRequestDto joinRequestDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            User user = await _context.Users
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            JoinRequest joinRequest = await _context.JoinRequests
                                            .FirstOrDefaultAsync(jr => jr.Id == joinRequestDto.Id);

            if (joinRequest == null)
            {
                response.Data = "Not allowed";
                response.Message = "There is no such join request with this Id";
                response.Success = false;
                return response;
            }


            if ( user == null || ( user != null && joinRequest.RequestingStudentId != GetUserId() ) )
            {
                response.Data = "Not allowed";
                response.Message = "You are not authorized to cancel this join request";
                response.Success = false;
                return response;
            }

            if (joinRequest.Accepted)
            {
                response.Data = "Not allowed";
                response.Message = "The join request is already accepted. So you can't cancel it";
                response.Success = false;
                return response;
            }

            if (joinRequest.Resolved)
            {
                response.Data = "Not allowed";
                response.Message = "The join request is already resolved. So you can't cancel it";
                response.Success = false;
                return response;
            }


            ProjectGroup requestedGroup = await _context.ProjectGroups
                                            .FirstOrDefaultAsync( rg =>  rg.Id == joinRequest.RequestedGroupId);
            

            _context.JoinRequests.Remove( joinRequest );
            await _context.SaveChangesAsync();

            if( user != null )
            {
                _context.Users.Update( user );
                await _context.SaveChangesAsync();
            }
            
            if( requestedGroup != null )
            {
                _context.ProjectGroups.Update( requestedGroup );
                await _context.SaveChangesAsync();
            }

            response.Data = "Successful";
            response.Message = "Join request is successfully cancelled";
            response.Success = true;

            return response;
        }

        // bugli durum: user oyladi cikti gruptan accepted number yuksek kaldi
        // grupta sifir kisi kaldi gecmis olsun
        public async Task<ServiceResponse<JoinRequestInfoDto>> Vote(VoteJoinRequestDto joinRequestDto)
        {
            ServiceResponse<JoinRequestInfoDto> response = new ServiceResponse<JoinRequestInfoDto>();
            JoinRequest joinRequest = await _context.JoinRequests.Include(jr => jr.RequestedGroup)
                                    .ThenInclude(pg => pg.GroupMembers)
                                .FirstOrDefaultAsync(jr => jr.Id == joinRequestDto.Id);
            User user = await _context.Users
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());

            if (joinRequest == null)
            {
                response.Data = null;
                response.Message = "There is no such join request";
                response.Success = false;
                return response;
            }
            if (joinRequest.Resolved)
            {
                response.Data = new JoinRequestInfoDto { Id = joinRequestDto.Id, Accepted = joinRequest.Accepted, Resolved = joinRequest.Resolved, VotedStudents = joinRequest.VotedStudents };
                response.Message = "You cannot vote because the join request is resolved already";
                response.Success = false;
                return response;
            }
            if (!joinRequest.RequestedGroup.GroupMembers.Any(pgu => pgu.UserId == GetUserId()))
            {
                response.Data = null;
                response.Message = "You cannot vote because you are not in this group";
                response.Success = false;
                return response;
            }
            if( joinRequest.Accepted )
            {
                response.Data = null;
                response.Message = "The join request is accepted already";
                response.Success = false;
                return response;
            }

            bool voteChanged = false;

            if( joinRequest.VotedStudents != "" )
            {
                string[] voters = joinRequest.VotedStudents.Split(' ');

                string idString = "";
                idString += user.Id;


                foreach( string s in voters )
                {
                    if( string.Compare( idString, s) == 0 )
                    {
                        if( joinRequestDto.accept )
                        {
                            response.Data = new JoinRequestInfoDto {Id = joinRequestDto.Id, Accepted = joinRequest.Accepted, Resolved = joinRequest.Resolved, AcceptedNumber = joinRequest.AcceptedNumber, VotedStudents = joinRequest.VotedStudents};
                            response.Message = "You have already voted";
                            response.Success = false;
                            return response;
                        }
                        else
                        {
                            joinRequest.AcceptedNumber--;
                            voteChanged = true;
                        }
                    }
                }       
            }
            if( joinRequestDto.accept ) 
            {
                joinRequest.AcceptedNumber++;
                if( joinRequest.AcceptedNumber >= joinRequest.RequestedGroup.GroupMembers.Count )
                {
                    joinRequest.Accepted = true;    
                    if( joinRequest.RequestedGroup.GroupMembers.Count + 1 >= joinRequest.RequestedGroup.ConfirmedUserNumber )
                    {
                        await DeleteAllJoinRequestsOfGroup( new DeleteAllJoinRequestsGroupDto { projectGroupId =  joinRequest.RequestedGroupId } );
                    }
                    await DeleteAllJoinRequestsOfUser( new DeleteAllJoinRequestsUserDto { userId =  joinRequest.RequestingStudentId } );
                }
            }
            else
            {
                joinRequest.Resolved = true;
                // bildirim nasi silcem
            }
            
            // call ozconun metodu

            if( !voteChanged )
            {
                joinRequest.VotedStudents = joinRequest.VotedStudents + user.Id + ' ';
            }
            response.Data = new JoinRequestInfoDto {Id = joinRequestDto.Id, Accepted = joinRequest.Accepted, Resolved = joinRequest.Resolved, AcceptedNumber = joinRequest.AcceptedNumber, VotedStudents = joinRequest.VotedStudents};
            response.Message = "You succesfully voted" ;
            response.Success = true;

            
            
            _context.JoinRequests.Update( joinRequest );
            await _context.SaveChangesAsync();
            

            return response;
        } 
        
        public async Task<ServiceResponse<string>> DeleteAllJoinRequestsOfGroup(DeleteAllJoinRequestsGroupDto deleteAllJoinRequestsGroupDto) 
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups
                                            .Include( g => g.GroupMembers).Include( g => g.IncomingJoinRequests )
                                            .FirstOrDefaultAsync( jr => jr.Id == deleteAllJoinRequestsGroupDto.projectGroupId );
                                    
            if( projectGroup == null )
            {
                response.Data = "Not allowed";
                response.Message = "There is no group with this Id";
                response.Success = false;
                return response;
            }

            if( projectGroup.IncomingJoinRequests.Count == 0 )
            {
                response.Data = "Not allowed";
                response.Message = "There is no incoming join requests";
                response.Success = false;
                return response;
            }

            /* bu olmaz gibi cunku user tetiklemiyo bunu
            if( !projectGroup.GroupMembers.Contains(user) )
            {
                response.Data = "Not allowed";
                response.Message = "You are not in this group";
                response.Success = false;
                return response;
            }
            */

            foreach( JoinRequest jr in projectGroup.IncomingJoinRequests )
            {
                if( !jr.Accepted && !jr.Resolved )
                    _context.JoinRequests.Remove( jr );
            }

            
            await _context.SaveChangesAsync();

            response.Data = "Successful";
            response.Message = "Join requests are successfully deleted";
            response.Success = true;

            return response;
        }
       
        public async Task<ServiceResponse<string>> DeleteAllJoinRequestsOfUser(DeleteAllJoinRequestsUserDto deleteAllJoinRequestsUserDto) 
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include( u => u.OutgoingJoinRequests )
                                            .FirstOrDefaultAsync( jr => jr.Id == deleteAllJoinRequestsUserDto.userId );
                                    
            if( user == null )
            {
                response.Data = "Not allowed";
                response.Message = "There is no user with this Id";
                response.Success = false;
                return response;
            }

            if( user.OutgoingJoinRequests.Count == 0 )
            {
                response.Data = "Not allowed";
                response.Message = "There is no outgoing join requests";
                response.Success = false;
                return response;
            }

            foreach( JoinRequest jr in user.OutgoingJoinRequests )
            {
                if( !jr.Accepted && !jr.Resolved )
                    _context.JoinRequests.Remove( jr );
            }

            
            await _context.SaveChangesAsync();

            response.Data = "Successful";
            response.Message = "Join requests are successfully deleted";
            response.Success = true;

            return response;
        }
        public async Task<ServiceResponse<GetJoinRequestDto>> GetJoinRequestById(int joinRequestId) {
            ServiceResponse<GetJoinRequestDto> response = new ServiceResponse<GetJoinRequestDto>();
            JoinRequest joinRequest = await _context.JoinRequests
                .Include( jr => jr.RequestingStudent )
                .Include( jr => jr.RequestedGroup )
                .FirstOrDefaultAsync( jr => jr.Id == joinRequestId );
            
            if( joinRequest == null)
            {
                response.Data = null;
                response.Message = "There is no join request with this id";
                response.Success =false;
                return response;
            }
            if( joinRequest.RequestingStudent == null)
            {
                response.Data = null;
                response.Message = "There is no student with this id";
                response.Success =false;
                return response;
            }
            if( joinRequest.RequestedGroup == null)
            {
                response.Data = null;
                response.Message = "There is no group with this id";
                response.Success =false;
                return response;
            }
            
            GetJoinRequestDto dto = new GetJoinRequestDto
            {
                Id = joinRequestId,
                RequestingStudent = new UserInJoinRequestDto
                {
                    Id = joinRequest.RequestingStudent.Id,
                    email = joinRequest.RequestingStudent.Email,
                    name = joinRequest.RequestingStudent.Name
                },
                RequestingStudentId = joinRequest.RequestingStudentId,
                RequestedGroup = new ProjectGroupInJoinRequestDto
                {
                    Id = joinRequest.RequestedGroup.Id,
                    AffiliatedSectionId = joinRequest.RequestedGroup.AffiliatedSectionId,
                    AffiliatedCourseId = joinRequest.RequestedGroup.AffiliatedCourseId,
                    ConfirmationState = joinRequest.RequestedGroup.ConfirmationState,
                    ConfirmedUserNumber = joinRequest.RequestedGroup.ConfirmedUserNumber,
                    ProjectInformation = joinRequest.RequestedGroup.ProjectInformation,
                    ConfirmedGroupMembers = joinRequest.RequestedGroup.ConfirmedGroupMembers,
                },
                RequestedGroupId = joinRequest.RequestedGroupId,
                CreatedAt = joinRequest.CreatedAt,
                AcceptedNumber = joinRequest.AcceptedNumber,
                Accepted = joinRequest.Accepted,
                Resolved = joinRequest.Resolved,
                VotedStudents = joinRequest.VotedStudents,
                Description = joinRequest.Description
            };

            response.Data = dto;
            response.Message = "Success";
            response.Success = true;

            return response;
        }

        
       
        // bi sekilde grupta 0 insan kalmasi durumu
        // rejectlenirse unvotedlarda gorunmeyec -- resolved
        // method : cancelAllRequests

    }
}