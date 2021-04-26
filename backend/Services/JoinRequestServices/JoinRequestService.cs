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
                VotedStudents = ""
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


            if (user != null && joinRequest.RequestingStudentId != GetUserId())
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
                                            .FirstOrDefaultAsync(rg => rg.Id == joinRequest.RequestedGroupId);

            /*          

                        if( user == null )
                        {
                            response.Data = "Not allowed";
                            response.Message = "There is no user with this Id";
                            response.Success = false;
                            return response;
                        }

                        if( requestedGroup == null )
                        {
                            response.Data = "Not allowed";
                            response.Message = "There is no group with this Id";
                            response.Success = false;
                            return response;
                        }

                        if( requestedGroup == null )
                        {
                            response.Data = "Not allowed";
                            response.Message = "There is no group with this Id";
                            response.Success = false;
                            return response;
                        }
            */
            /*
            bool groupHasJr = false;

            foreach( JoinRequest jr in requestedGroup.IncomingJoinRequests )
            {
                if( jr.Id == joinRequestDto.Id )
                {
                    requestedGroup.IncomingJoinRequests.Contains(jr);
                    groupHasJr = true;
                }
            }     

            if( !groupHasJr )
            {
                response.Data = "Not allowed";
                response.Message = "Group does not have such join request";
                response.Success = false;
                return response;
            }

            bool userHasJr = false;

            foreach( JoinRequest jr in user.OutgoingJoinRequests )
            {
                if( jr.Id == joinRequestDto.Id )
                {
                    user.OutgoingJoinRequests.Contains(jr);
                    userHasJr = true;
                }
            }     

            if( !userHasJr )
            {
                response.Data = "Not allowed";
                response.Message = "User does not have such join request";
                response.Success = false;
                return response;
            }
            
            _context.ProjectGroups.Update(requestedGroup);
            await _context.SaveChangesAsync();
            */


            _context.JoinRequests.Remove(joinRequest);
            await _context.SaveChangesAsync();

            if (user != null)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            if (requestedGroup != null)
            {
                _context.ProjectGroups.Update(requestedGroup);
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

            if (joinRequest.VotedStudents != "")
            {
                string[] voters = joinRequest.VotedStudents.Split(' ');

                string idString = "";
                idString += user.Id;


                foreach (string s in voters)
                {
                    if (string.Compare(idString, s) == 0)
                    {
                        response.Data = new JoinRequestInfoDto { Id = joinRequestDto.Id, Accepted = joinRequest.Accepted, Resolved = joinRequest.Resolved, VotedStudents = joinRequest.VotedStudents };
                        response.Message = "You have already voted";
                        response.Success = false;
                        return response;
                    }
                }
            }
            if (joinRequestDto.accept)
            {
                joinRequest.AcceptedNumber++;
                if (joinRequest.AcceptedNumber >= joinRequest.RequestedGroup.GroupMembers.Count)
                {
                    joinRequest.Accepted = true;
                }
            }
            else
            {
                joinRequest.Resolved = true;
                // bildirim nasi silcem
            }

            // call ozconun metodu

            joinRequest.VotedStudents = joinRequest.VotedStudents + user.Id + ' ';
            response.Data = new JoinRequestInfoDto { Id = joinRequestDto.Id, Accepted = joinRequest.Accepted, Resolved = joinRequest.Resolved, VotedStudents = joinRequest.VotedStudents };
            response.Message = "You succesfully voted" + joinRequestDto.accept;
            response.Success = true;


            _context.JoinRequests.Update(joinRequest);
            await _context.SaveChangesAsync();

            return response;
        }


        // bi sekilde grupta 0 insan kalmasi durumu
        // rejectlenirse unvotedlarda gorunmeyec -- resolved
        // method : cancelAllRequests

    }
}