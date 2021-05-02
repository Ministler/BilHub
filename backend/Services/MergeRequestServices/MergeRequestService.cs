using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using backend.Models;
using backend.Data;
using backend.Dtos.MergeRequest;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Text;
using backend.Services.ProjectGroupServices;


// merge ve join birlestirme : join=1kisilikmerge
// todo: gonderilen her seyi cancel tek tek yerine

namespace backend.Services.MergeRequestServices
{
    public class MergeRequestService : IMergeRequestService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IProjectGroupService _projectGroupService;

        public MergeRequestService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, IProjectGroupService projectGroupService)
        {
            _projectGroupService = projectGroupService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<AddMergeRequestDto>> SendMergeRequest(AddMergeRequestDto newMergeRequest)
        {
            ServiceResponse<AddMergeRequestDto> response = new ServiceResponse<AddMergeRequestDto>();
            User user = await _context.Users.Include(u => u.ProjectGroups)
                                .ThenInclude(g => g.ProjectGroup)
                                .ThenInclude(g => g.AffiliatedSection)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup receiverGroup = await _context.ProjectGroups.Include(g => g.AffiliatedSection)
                                            .Include(g => g.GroupMembers).Include(g => g.IncomingMergeRequest)
                                            .Include(g => g.OutgoingMergeRequest)
                                            .FirstOrDefaultAsync(rg => rg.Id == newMergeRequest.ReceiverGroupId);

            if (receiverGroup == null)
            {
                response.Data = null;
                response.Message = "There is no group with receiverGroup's id";
                response.Success = false;
                return response;
            }

            Section section = await _context.Sections.FirstOrDefaultAsync(s => (s.Id == receiverGroup.AffiliatedSection.Id));

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


            ProjectGroup senderGroup = new ProjectGroup { AffiliatedSectionId = -1 };

            foreach (ProjectGroupUser pgu in user.ProjectGroups)
            {
                if (pgu.ProjectGroup.AffiliatedCourseId == receiverGroup.AffiliatedCourseId && pgu.ProjectGroup.AffiliatedSectionId == receiverGroup.AffiliatedSectionId)
                {
                    senderGroup = pgu.ProjectGroup;
                    break;
                }
            }
            if (senderGroup.AffiliatedSectionId == -1)
            {
                response.Data = null;
                response.Message = "You are not in the same section with this group";
                response.Success = false;
                return response;
            }
            if (senderGroup.Id == receiverGroup.Id)
            {
                response.Data = null;
                response.Message = "You are already in this group";
                response.Success = false;
                return response;
            }

            if (senderGroup.ConfirmationState == true)
            {
                response.Data = null;
                response.Message = "Your group is finalized.";
                response.Success = false;
                return response;
            }
            if (receiverGroup.ConfirmationState == true)
            {
                response.Data = null;
                response.Message = "The group you want to merge with is finalized.";
                response.Success = false;
                return response;
            }
            ProjectGroup senderGroupOriginal = await _context.ProjectGroups
                                            .Include(g => g.GroupMembers)
                                            .FirstOrDefaultAsync(rg => rg.Id == senderGroup.Id);

            int maxSize = (await _context.Courses.FirstOrDefaultAsync(c => c.Id == senderGroup.AffiliatedCourseId)).MaxGroupSize;
            if (receiverGroup.GroupMembers.Count + senderGroupOriginal.GroupMembers.Count > maxSize)
            {
                response.Data = null;
                response.Message = "You would exceed the max allowed group size if you merged.";
                response.Success = false;
                return response;
            }
            if (receiverGroup.GroupMembers.Count == 0)
            {
                response.Data = null;
                response.Message = "The group you want to merge with is empty";
                response.Success = false;
                return response;
            }


            foreach (MergeRequest mr in receiverGroup.IncomingMergeRequest)
            {
                if (mr.SenderGroupId == senderGroup.Id && !mr.Resolved)
                {
                    response.Data = null;
                    response.Message = "Your group has already sent a merge request to this group that is not resolved yet.";
                    response.Success = false;
                    return response;
                }
            }

            foreach (MergeRequest mr in receiverGroup.OutgoingMergeRequest)
            {
                if (mr.ReceiverGroupId == senderGroup.Id && !mr.Resolved)
                {
                    response.Data = null;
                    response.Message = "The group has already sent a merge request to your group that is not resolved yet.";
                    response.Success = false;
                    return response;
                }
            }

            MergeRequest createdMergeRequest = new MergeRequest
            {
                SenderGroup = senderGroup,
                SenderGroupId = senderGroup.Id,
                ReceiverGroup = receiverGroup,
                ReceiverGroupId = receiverGroup.Id,
                VotedStudents = GetUserId().ToString(),
                CreatedAt = newMergeRequest.CreatedAt,
                Accepted = false,
                Resolved = false,
                Description = newMergeRequest.Description
            };

            receiverGroup.IncomingMergeRequest.Add(createdMergeRequest);
            senderGroup.OutgoingMergeRequest.Add(createdMergeRequest);

            _context.ProjectGroups.Update(receiverGroup);
            _context.ProjectGroups.Update(senderGroup);
            _context.MergeRequests.Add(createdMergeRequest);
            await _context.SaveChangesAsync();

            response.Data = newMergeRequest;
            response.Message = "Merge request is successfully sent " + receiverGroup.GroupMembers.Count + " " + receiverGroup.GroupMembers.Count;
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<string>> CancelMergeRequest(CancelMergeRequestDto mergeRequestDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            User user = await _context.Users
                                .Include(u => u.ProjectGroups)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            MergeRequest mergeRequest = await _context.MergeRequests
                                            .FirstOrDefaultAsync(mr => mr.Id == mergeRequestDto.Id);

            if (mergeRequest == null)
            {
                response.Data = "Not allowed";
                response.Message = "There is no merge request with this Id";
                response.Success = false;
                return response;
            }

            if (user == null)
            {
                response.Data = "Not allowed";
                response.Message = "There is no user with this Id";
                response.Success = false;
                return response;
            }

            ProjectGroup senderGroup = await _context.ProjectGroups
                                            .FirstOrDefaultAsync(sg => sg.Id == mergeRequest.SenderGroupId);

            ProjectGroup receiverGroup = await _context.ProjectGroups
                                            .FirstOrDefaultAsync(rg => rg.Id == mergeRequest.ReceiverGroupId);

            bool isUserInSender = senderGroup.GroupMembers.Any(pgu => pgu.UserId == GetUserId());



            if (!isUserInSender)
            {
                response.Data = "Not allowed";
                response.Message = "You are not authorized to cancel this merge request";
                response.Success = false;
                return response;
            }

            if (mergeRequest.Accepted)
            {
                response.Data = "Not allowed";
                response.Message = "The merge request is already accepted. So you can't cancel it";
                response.Success = false;
                return response;
            }

            if (mergeRequest.Resolved)
            {
                response.Data = "Not allowed";
                response.Message = "The merge request is already resolved. So you can't cancel it";
                response.Success = false;
                return response;
            }

            _context.MergeRequests.Remove(mergeRequest);
            await _context.SaveChangesAsync();

            if (senderGroup != null)
            {
                _context.ProjectGroups.Update(senderGroup);
                await _context.SaveChangesAsync();
            }

            if (receiverGroup != null)
            {
                _context.ProjectGroups.Update(receiverGroup);
                await _context.SaveChangesAsync();
            }

            response.Data = "Successful";
            response.Message = "Merge request is successfully cancelled";
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
        
        public async Task<ServiceResponse<MergeRequestInfoDto>> Vote(VoteMergeRequestDto mergeRequestDto)
        {
            ServiceResponse<MergeRequestInfoDto> response = new ServiceResponse<MergeRequestInfoDto>();
            MergeRequest mergeRequest = await _context.MergeRequests.Include(mr => mr.ReceiverGroup)
                                    .ThenInclude(pg => pg.GroupMembers)
                                    .Include(mr => mr.SenderGroup)
                                    .ThenInclude(pg => pg.GroupMembers)
                                .FirstOrDefaultAsync(mr => mr.Id == mergeRequestDto.Id);
            User user = await _context.Users
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());

            if (mergeRequest == null)
            {
                response.Data = null;
                response.Message = "There is no such merge request";
                response.Success = false;
                return response;
            }
            if (mergeRequest.Resolved)
            {
                response.Data = null;
                response.Message = "You cannot vote because the merge request is resolved already";
                response.Success = false;
                return response;
            }
            if (mergeRequest.Accepted)
            {
                response.Data = null;
                response.Message = "The merge request is accepted already";
                response.Success = false;
                return response;
            }
            if (!mergeRequest.ReceiverGroup.GroupMembers.Any(pgu => pgu.UserId == GetUserId()) && !mergeRequest.SenderGroup.GroupMembers.Any(pgu => pgu.UserId == GetUserId()))
            {
                response.Data = null;
                response.Message = "You cannot vote because you are not in the sender or receiver group";
                response.Success = false;
                return response;
            }

            if (!IsUserInString(mergeRequest.VotedStudents, GetUserId()) && mergeRequestDto.accept == false)
            {
                mergeRequest.Resolved = true;
                _context.MergeRequests.Update(mergeRequest);
                await _context.SaveChangesAsync();
                response.Message = "Merge request is cancelled by the negative vote";
                return response;
            }

            if (IsUserInString(mergeRequest.VotedStudents, GetUserId()) && mergeRequestDto.accept == false)
            {
                mergeRequest.VotedStudents = RemoveUserFromString(mergeRequest.VotedStudents, GetUserId());
                _context.MergeRequests.Update(mergeRequest);
                await _context.SaveChangesAsync();
                response.Message = "Your vote is taken back successfully.";
                return response;
            }
            if (IsUserInString(mergeRequest.VotedStudents, GetUserId()) && mergeRequestDto.accept == true)
            {
                response.Success = false;
                response.Message = "User is already voted accepted for this request";
                return response;
            }

            int acceptedNumber = 0;
            string[] voters = mergeRequest.VotedStudents.Split(' ');
            foreach (string s in voters)
            {
                if (s != "" && s != " ")
                    acceptedNumber++;
            }

            int maxSize = (await _context.Courses.FirstOrDefaultAsync(c => c.Id == mergeRequest.SenderGroup.AffiliatedCourseId)).MaxGroupSize;

            acceptedNumber++;
            mergeRequest.VotedStudents = AddUserToString(mergeRequest.VotedStudents, GetUserId());

            if (acceptedNumber >= mergeRequest.ReceiverGroup.GroupMembers.Count + mergeRequest.SenderGroup.GroupMembers.Count && mergeRequest.ReceiverGroup.GroupMembers.Count + mergeRequest.SenderGroup.GroupMembers.Count <= maxSize)
            {
                int newSize = mergeRequest.ReceiverGroup.GroupMembers.Count + mergeRequest.SenderGroup.GroupMembers.Count;
                mergeRequest.Accepted = true;
                _context.MergeRequests.Update(mergeRequest);
                await _context.SaveChangesAsync();

                int recId = mergeRequest.ReceiverGroupId;
                int senId = mergeRequest.SenderGroupId;

                var tmp = await _projectGroupService.CompleteMergeRequest ( mergeRequest.Id );
                if ( newSize >= maxSize )
                {
                    await DeleteAllMergeRequests(new DeleteAllMergeRequestsDto { projectGroupId = recId });
                    await DeleteAllMergeRequests(new DeleteAllMergeRequestsDto { projectGroupId = senId });
                }
                // _context.MergeRequests.Remove ( mergeRequest );
                await _context.SaveChangesAsync();
                response.Message = "Merge request is accepted from all members and groups are merged.";
                return response;
            }

            _context.MergeRequests.Update(mergeRequest);
            await _context.SaveChangesAsync();

            response.Data = new MergeRequestInfoDto { Id = mergeRequestDto.Id, Accepted = mergeRequest.Accepted, Resolved = mergeRequest.Resolved, VotedStudents = mergeRequest.VotedStudents };
            response.Message = "You succesfully voted";

            return response;
        }

        public async Task<ServiceResponse<string>> DeleteAllMergeRequests(DeleteAllMergeRequestsDto deleteAllMergeRequestsDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            ProjectGroup projectGroup = await _context.ProjectGroups
                                            .Include(g => g.GroupMembers)
                                            .Include(g => g.IncomingMergeRequest)
                                            .Include(g => g.OutgoingMergeRequest)
                                            .FirstOrDefaultAsync(jr => jr.Id == deleteAllMergeRequestsDto.projectGroupId);

            if (projectGroup == null)
            {
                response.Data = "Not allowed";
                response.Message = "There is no group with this Id";
                response.Success = false;
                return response;
            }

            if (projectGroup.IncomingMergeRequest.Count != 0)
            {
                foreach (MergeRequest mr in projectGroup.IncomingMergeRequest)
                {
                    if (!mr.Accepted && !mr.Resolved)
                        _context.MergeRequests.Remove(mr);
                }
            }

            await _context.SaveChangesAsync();
            if (projectGroup.OutgoingMergeRequest.Count != 0)
            {
                foreach (MergeRequest mr in projectGroup.OutgoingMergeRequest)
                {
                    if (!mr.Accepted && !mr.Resolved)
                        _context.MergeRequests.Remove(mr);

                }
            }

            await _context.SaveChangesAsync();

            response.Data = "Successful";
            response.Message = "Merge requests are successfully deleted";
            response.Success = true;

            return response;
        }


        public async Task<ServiceResponse<GetMergeRequestDto>> GetMergeRequestById(int mergeRequestId)
        {
            ServiceResponse<GetMergeRequestDto> response = new ServiceResponse<GetMergeRequestDto>();
            MergeRequest mergeRequest = await _context.MergeRequests
                .Include(jr => jr.SenderGroup)
                .Include(jr => jr.ReceiverGroup)
                .FirstOrDefaultAsync(jr => jr.Id == mergeRequestId);

            if (mergeRequest == null)
            {
                response.Data = null;
                response.Message = "There is no merge request with this id";
                response.Success = false;
                return response;
            }
            if (mergeRequest.SenderGroup == null)
            {
                response.Data = null;
                response.Message = "There is no group with senderGroupId";
                response.Success = false;
                return response;
            }
            if (mergeRequest.ReceiverGroup == null)
            {
                response.Data = null;
                response.Message = "There is no group with receiverGroupId";
                response.Success = false;
                return response;
            }

            /*
            GetMergeRequestDto dto = new GetMergeRequestDto
            {
                Id = mergeRequestId,
                SenderGroup = new ProjectGroupInMergeRequestDto
                {
                    Id = mergeRequest.SenderGroup.Id,
                    AffiliatedSectionId = mergeRequest.SenderGroup.AffiliatedSectionId,
                    AffiliatedCourseId = mergeRequest.SenderGroup.AffiliatedCourseId,
                    ConfirmationState = mergeRequest.SenderGroup.ConfirmationState,
                    ConfirmedUserNumber = mergeRequest.SenderGroup.ConfirmedUserNumber,
                    ProjectInformation = mergeRequest.SenderGroup.ProjectInformation,
                    ConfirmedGroupMembers = mergeRequest.SenderGroup.ConfirmedGroupMembers,
                },
                SenderGroupId = mergeRequest.SenderGroupId,
                ReceiverGroup = new ProjectGroupInMergeRequestDto
                {
                    Id = mergeRequest.ReceiverGroup.Id,
                    AffiliatedSectionId = mergeRequest.ReceiverGroup.AffiliatedSectionId,
                    AffiliatedCourseId = mergeRequest.ReceiverGroup.AffiliatedCourseId,
                    ConfirmationState = mergeRequest.ReceiverGroup.ConfirmationState,
                    ConfirmedUserNumber = mergeRequest.ReceiverGroup.ConfirmedUserNumber,
                    ProjectInformation = mergeRequest.ReceiverGroup.ProjectInformation,
                    ConfirmedGroupMembers = mergeRequest.ReceiverGroup.ConfirmedGroupMembers,
                },
                ReceiverGroupId = mergeRequest.ReceiverGroupId,
                CreatedAt = mergeRequest.CreatedAt,
                Accepted = mergeRequest.Accepted,
                Resolved = mergeRequest.Resolved,
                VotedStudents = mergeRequest.VotedStudents,
                Description = mergeRequest.Description
            };
            */

            response.Data = _mapper.Map<GetMergeRequestDto> (mergeRequest);
            response.Message = "Success";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<List<GetMergeRequestDto>>> GetOutgoingMergeRequestsOfUser()
        {
            ServiceResponse<List<GetMergeRequestDto>> serviceResponse = new ServiceResponse<List<GetMergeRequestDto>> ();
            List<MergeRequest> dbMergeRequests = await _context.MergeRequests
                .Include(jr => jr.SenderGroup).ThenInclude( cs => cs.GroupMembers )
                .Include(jr => jr.ReceiverGroup).ThenInclude( cs => cs.GroupMembers )
                .Where ( c => c.SenderGroup.GroupMembers.Any ( cs => cs.UserId == GetUserId() )).ToListAsync();

            serviceResponse.Data = dbMergeRequests.Select(c => _mapper.Map<GetMergeRequestDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetMergeRequestDto>>> GetIncomingMergeRequestsOfUser()
        {
            ServiceResponse<List<GetMergeRequestDto>> serviceResponse = new ServiceResponse<List<GetMergeRequestDto>> ();
            List<MergeRequest> dbMergeRequests = await _context.MergeRequests
                .Include(jr => jr.SenderGroup).ThenInclude( cs => cs.GroupMembers )
                .Include(jr => jr.ReceiverGroup).ThenInclude( cs => cs.GroupMembers )
                .Where ( c => c.ReceiverGroup.GroupMembers.Any ( cs => cs.UserId == GetUserId() )).ToListAsync();

            serviceResponse.Data = dbMergeRequests.Select(c => _mapper.Map<GetMergeRequestDto>(c)).ToList();
            return serviceResponse;
        }
    }
}