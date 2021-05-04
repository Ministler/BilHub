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
using backend.Services.ProjectGroupServices;

namespace backend.Services.JoinRequestServices
{
    public class JoinRequestService : IJoinRequestService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IProjectGroupService _projectGroupService;

        public JoinRequestService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, IProjectGroupService projectGroupService)
        {
            _projectGroupService = projectGroupService;
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

            Section section = await _context.Sections.FirstOrDefaultAsync(s => (s.Id == requestedGroup.AffiliatedSectionId));

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
            _context.Users.Update(user);
            _context.JoinRequests.Add(createdJoinRequest);
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


            if (user == null || (user != null && joinRequest.RequestingStudentId != GetUserId()))
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

        private bool IsUserInString(string s, int userId)
        {
            if (s.Length == 0)
                return false;

            string[] confirmers = s.Split(' ');
            foreach (var i in confirmers)
            {
                if (i.Equals(userId.ToString()))
                    return true;
            }
            return false;
        }

        private string AddUserToString(string s, int userId)
        {
            if (s.Length == 0)
                return userId.ToString();
            if (IsUserInString(s, userId))
                return s;
            s = s + " " + userId.ToString();
            return s;
        }

        private string RemoveUserFromString(string s, int userId)
        {
            if (s.Length == 0 || !IsUserInString(s, userId))
                return s;
            string[] confirmers = s.Split(' ');
            string ret = "";
            foreach (var i in confirmers)
            {
                if (i.Equals(userId.ToString()))
                    continue;
                if (ret.Length == 0)
                    ret = ret + i;
                else
                    ret = ret + " " + i;
            }
            return ret;
        }

        // bugli durum: user oyladi cikti gruptan accepted number yuksek kaldi
        // grupta sifir kisi kaldi gecmis olsun
        public async Task<ServiceResponse<JoinRequestInfoDto>> Vote(VoteJoinRequestDto joinRequestDto)
        {
            ServiceResponse<JoinRequestInfoDto> response = new ServiceResponse<JoinRequestInfoDto>();
            JoinRequest joinRequest = await _context.JoinRequests
                .Include (jr => jr.RequestingStudent )
                .Include(jr => jr.RequestedGroup).ThenInclude(pg => pg.GroupMembers).ThenInclude( css => css.User )
                .Include(jr => jr.RequestedGroup).ThenInclude(pg => pg.AffiliatedCourse)
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
            if (joinRequest.Accepted)
            {
                response.Data = null;
                response.Message = "The join request is accepted already";
                response.Success = false;
                return response;
            }

            if (!IsUserInString(joinRequest.VotedStudents, GetUserId()) && joinRequestDto.accept == false)
            {
                joinRequest.Resolved = true;
                _context.JoinRequests.Update(joinRequest);
                await _context.SaveChangesAsync();
                response.Message = "Join request is cancelled by the negative vote";
                return response;
            }

            if (IsUserInString(joinRequest.VotedStudents, GetUserId()) && joinRequestDto.accept == false)
            {
                joinRequest.VotedStudents = RemoveUserFromString(joinRequest.VotedStudents, GetUserId());
                joinRequest.AcceptedNumber--;
                _context.JoinRequests.Update(joinRequest);
                await _context.SaveChangesAsync();
                response.Message = "Your vote is taken back successfully.";
                return response;
            }
            if (IsUserInString(joinRequest.VotedStudents, GetUserId()) && joinRequestDto.accept == true)
            {
                response.Success = false;
                response.Message = "User is already voted accepted for this request";
                return response;
            }

            int maxSize = (await _context.Courses.FirstOrDefaultAsync(c => c.Id == joinRequest.RequestedGroup.AffiliatedCourseId)).MaxGroupSize;

            joinRequest.AcceptedNumber++;
            joinRequest.VotedStudents = AddUserToString(joinRequest.VotedStudents, GetUserId());

            if (joinRequest.AcceptedNumber >= joinRequest.RequestedGroup.GroupMembers.Count && joinRequest.RequestedGroup.GroupMembers.Count + 1 <= maxSize)
            {
                int newSize = joinRequest.RequestedGroup.GroupMembers.Count + 1;
                joinRequest.Accepted = true;
                _context.JoinRequests.Update(joinRequest);
                await _context.SaveChangesAsync();

                int reqId = joinRequest.RequestedGroupId;

                var tmp = await _projectGroupService.CompleteJoinRequest(joinRequest.Id);

                if (newSize >= maxSize)
                {
                    await DeleteAllJoinRequestsOfGroup(new DeleteAllJoinRequestsGroupDto { projectGroupId = reqId });
                }
                // await DeleteAllJoinRequestsOfUser(new DeleteAllJoinRequestsUserDto { userId = joinRequest.RequestingStudentId });
                // _context.JoinRequests.Remove ( joinRequest );
                await _context.SaveChangesAsync();

                response.Message = "Join request is accepted by all members. The user joined the group successfully";
                if ( !tmp.Success )
                    response.Message = tmp.Message;
                return response;
            }


            response.Data = new JoinRequestInfoDto
            {
                Id = joinRequestDto.Id,
                Accepted = joinRequest.Accepted,
                Resolved = joinRequest.Resolved,
                AcceptedNumber = joinRequest.AcceptedNumber,
                VotedStudents = joinRequest.VotedStudents
            };

            response.Message = "You succesfully voted";
            response.Success = true;

            _context.JoinRequests.Update(joinRequest);
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<ServiceResponse<string>> DeleteAllJoinRequestsOfGroup(DeleteAllJoinRequestsGroupDto deleteAllJoinRequestsGroupDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups
                                            .Include(g => g.GroupMembers).Include(g => g.IncomingJoinRequests)
                                            .FirstOrDefaultAsync(jr => jr.Id == deleteAllJoinRequestsGroupDto.projectGroupId);

            if (projectGroup == null)
            {
                response.Data = "Not allowed";
                response.Message = "There is no group with this Id";
                response.Success = false;
                return response;
            }

            if (projectGroup.IncomingJoinRequests.Count == 0)
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

            foreach (JoinRequest jr in projectGroup.IncomingJoinRequests)
            {
                if (!jr.Accepted && !jr.Resolved)
                    _context.JoinRequests.Remove(jr);
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
            User user = await _context.Users.Include(u => u.OutgoingJoinRequests)
                                            .FirstOrDefaultAsync(jr => jr.Id == deleteAllJoinRequestsUserDto.userId);

            if (user == null)
            {
                response.Data = "Not allowed";
                response.Message = "There is no user with this Id";
                response.Success = false;
                return response;
            }

            if (user.OutgoingJoinRequests.Count == 0)
            {
                response.Data = "Not allowed";
                response.Message = "There is no outgoing join requests";
                response.Success = false;
                return response;
            }

            foreach (JoinRequest jr in user.OutgoingJoinRequests)
            {
                if (!jr.Accepted && !jr.Resolved)
                    _context.JoinRequests.Remove(jr);
            }


            await _context.SaveChangesAsync();

            response.Data = "Successful";
            response.Message = "Join requests are successfully deleted";
            response.Success = true;

            return response;
        }
        public async Task<ServiceResponse<GetJoinRequestDto>> GetJoinRequestById(int joinRequestId)
        {
            ServiceResponse<GetJoinRequestDto> response = new ServiceResponse<GetJoinRequestDto>();
            JoinRequest joinRequest = await _context.JoinRequests
                .Include(jr => jr.RequestingStudent)
                .Include(jr => jr.RequestedGroup).ThenInclude(cs => cs.GroupMembers).ThenInclude(css => css.User)
                .Include(jr => jr.RequestedGroup).ThenInclude(cs => cs.AffiliatedCourse)
                .FirstOrDefaultAsync(jr => jr.Id == joinRequestId);

            if (joinRequest == null)
            {
                response.Data = null;
                response.Message = "There is no join request with this id";
                response.Success = false;
                return response;
            }
            if (joinRequest.RequestingStudent == null)
            {
                response.Data = null;
                response.Message = "There is no student with this id";
                response.Success = false;
                return response;
            }
            if (joinRequest.RequestedGroup == null)
            {
                response.Data = null;
                response.Message = "There is no group with this id";
                response.Success = false;
                return response;
            }

            response.Data = _mapper.Map<GetJoinRequestDto>(joinRequest);
            response.Data.LockDate = joinRequest.RequestedGroup.AffiliatedCourse.LockDate;
            response.Data.CourseName = joinRequest.RequestedGroup.AffiliatedCourse.Name;
            response.Data.CurrentUserVote = IsUserInString(joinRequest.VotedStudents, GetUserId());
            if (joinRequest.RequestingStudentId == GetUserId())
                response.Data.CurrentUserVote = true;
            response.Message = "Success";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<List<GetJoinRequestDto>>> GetOutgoingJoinRequestsOfUser()
        {
            ServiceResponse<List<GetJoinRequestDto>> serviceResponse = new ServiceResponse<List<GetJoinRequestDto>>();
            List<JoinRequest> dbJoinRequests = await _context.JoinRequests
                .Include(jr => jr.RequestingStudent)
                .Include(jr => jr.RequestedGroup).ThenInclude(cs => cs.GroupMembers).ThenInclude(css => css.User)
                .Include(jr => jr.RequestedGroup).ThenInclude(cs => cs.AffiliatedCourse)
                .Where(c => c.RequestingStudentId == GetUserId()).ToListAsync();

            List<GetJoinRequestDto> dtos = new List<GetJoinRequestDto>();
            foreach (var i in dbJoinRequests)
            {
                GetJoinRequestDto tmp = _mapper.Map<GetJoinRequestDto>(i);
                tmp.LockDate = i.RequestedGroup.AffiliatedCourse.LockDate;
                tmp.CourseName = i.RequestedGroup.AffiliatedCourse.Name;
                tmp.CurrentUserVote = IsUserInString(i.VotedStudents, GetUserId());
                if (i.RequestingStudentId == GetUserId())
                    tmp.CurrentUserVote = true;
                dtos.Add(tmp);
            }
            serviceResponse.Data = dtos;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetJoinRequestDto>>> GetIncomingJoinRequestsOfUser()
        {
            ServiceResponse<List<GetJoinRequestDto>> serviceResponse = new ServiceResponse<List<GetJoinRequestDto>>();
            List<JoinRequest> dbJoinRequests = await _context.JoinRequests
                .Include(jr => jr.RequestingStudent)
                .Include(jr => jr.RequestedGroup).ThenInclude(g => g.AffiliatedCourse)
                .Include(jr => jr.RequestedGroup).ThenInclude(cs => cs.GroupMembers).ThenInclude(css => css.User)
                .Where(c => c.RequestedGroup.GroupMembers.Any(cs => cs.UserId == GetUserId())).ToListAsync();

            List<GetJoinRequestDto> dtos = new List<GetJoinRequestDto>();
            foreach (var i in dbJoinRequests)
            {
                GetJoinRequestDto tmp = _mapper.Map<GetJoinRequestDto>(i);
                tmp.LockDate = i.RequestedGroup.AffiliatedCourse.LockDate;
                tmp.CourseName = i.RequestedGroup.AffiliatedCourse.Name;
                tmp.CurrentUserVote = IsUserInString(i.VotedStudents, GetUserId());
                if (i.RequestingStudentId == GetUserId())
                    tmp.CurrentUserVote = true;
                dtos.Add(tmp);
            }
            serviceResponse.Data = dtos;
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> GetVoteOfUser(int joinRequestId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            JoinRequest joinRequest = await _context.JoinRequests
                .Include(jr => jr.RequestedGroup).ThenInclude(cs => cs.GroupMembers).ThenInclude(css => css.User)
                .FirstOrDefaultAsync(jr => jr.Id == joinRequestId);

            if (joinRequest == null)
            {
                response.Data = null;
                response.Message = "There is no join request with this id";
                response.Success = false;
                return response;
            }

            if (!joinRequest.RequestedGroup.GroupMembers.Any(pgu => pgu.UserId == GetUserId()))
            {
                response.Data = null;
                response.Message = "You did not vote because you are not in this group";
                response.Success = false;
                return response;
            }

            response.Success = true;

            if (joinRequest.Resolved || joinRequest.Accepted)
            {
                response.Data = "Resolved";
                return response;
            }

            if (!IsUserInString(joinRequest.VotedStudents, GetUserId()))
            {
                response.Data = "Pending";
                return response;
            }


            response.Data = "Unresolved";
            return response;
        }


        //////// ADD LOCK DATE

        // bi sekilde grupta 0 insan kalmasi durumu
        // rejectlenirse unvotedlarda gorunmeyec -- resolved
        // method : cancelAllRequests

    }
}