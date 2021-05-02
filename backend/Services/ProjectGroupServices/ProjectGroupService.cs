using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.ProjectGrade;
using backend.Dtos.ProjectGroup;
using backend.Models;
using backend.Services.AssignmentServices;
using backend.Services.ProjectGradeServices;
using backend.Services.SubmissionServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.ProjectGroupServices
{
    public class ProjectGroupService : IProjectGroupService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISubmissionService _submissionService;
        private readonly IAssignmentService _assignment;
        private readonly IProjectGradeService _projectGradeService;

        public ProjectGroupService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, ISubmissionService submissionService, IAssignmentService assignment, IProjectGradeService projectGradeService)
        {
            _httpContextAccessor = httpContextAccessor;
            _submissionService = submissionService;
            _assignment = assignment;
            _projectGradeService = projectGradeService;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        private bool IsUserInGroup(ProjectGroup projectGroup, int userId)
        {
            foreach (var i in projectGroup.GroupMembers)
            {
                if (i.UserId == userId)
                    return true;
            }
            return false;
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

        private async Task<bool> isUserInstructorOfGroup(int projectGroupId)
        {

            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                    .Include(c => c.AffiliatedCourse)
                    .FirstOrDefaultAsync(c => c.Id == projectGroupId);

            if (dbProjectGroup == null)
                return false;

            User instructorControl = await _context.Users
                .Include(c => c.InstructedCourses)
                .FirstOrDefaultAsync(c => (c.Id == GetUserId()) && c.InstructedCourses.Any(inst => inst.CourseId == dbProjectGroup.AffiliatedCourseId));

            if (instructorControl == null)
                return false;
            return true;
        }
        public async Task<ServiceResponse<GetProjectGroupDto>> GetProjectGroupById(int id)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .Include(c => c.GroupMembers).ThenInclude(cs => cs.User)
                .Include(c => c.AffiliatedCourse)
                .Include(c => c.AffiliatedSection)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (dbProjectGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Project group not found.";
                return serviceResponse;
            }
            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbProjectGroup);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProjectGroupDto>> UpdateProjectGroupInformation(UpdateProjectGroupDto updateProjectGroupDto)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .Include(c => c.GroupMembers).ThenInclude(cs => cs.User)
                .Include(c => c.AffiliatedCourse)
                .Include(c => c.AffiliatedSection)
                .FirstOrDefaultAsync(c => c.Id == updateProjectGroupDto.Id);
            if (dbProjectGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Project group not found.";
                return serviceResponse;
            }

            if (!IsUserInGroup(dbProjectGroup, GetUserId()))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not in project group";
                return serviceResponse;
            }

            dbProjectGroup.ProjectInformation = updateProjectGroupDto.ProjectInformation;
            _context.ProjectGroups.Update(dbProjectGroup);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbProjectGroup);
            serviceResponse.Message = "Successfully updated project group information";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProjectGroupDto>> ConfirmationOfStudent(ConfirmationAnswerDto confirmationAnswerDto)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .Include(c => c.GroupMembers).ThenInclude(cs => cs.User)
                .Include(c => c.AffiliatedCourse)
                .Include(c => c.AffiliatedSection)
                .FirstOrDefaultAsync(c => c.Id == confirmationAnswerDto.ProjectGroupId);
            if (dbProjectGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Project group not found.";
                return serviceResponse;
            }

            if (!IsUserInGroup(dbProjectGroup, GetUserId()))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not in project group";
                return serviceResponse;
            }

            if (!IsUserInString(dbProjectGroup.ConfirmedGroupMembers, GetUserId()) && confirmationAnswerDto.ConfirmationState == false)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User confirmation state is already not confirmed";
                return serviceResponse;
            }

            if (IsUserInString(dbProjectGroup.ConfirmedGroupMembers, GetUserId()) && confirmationAnswerDto.ConfirmationState)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User confirmation state is already confirmed";
                return serviceResponse;
            }


            string newConfirmedGroupMembers = "";
            if (confirmationAnswerDto.ConfirmationState)
            {
                newConfirmedGroupMembers = AddUserToString(dbProjectGroup.ConfirmedGroupMembers, GetUserId());
                dbProjectGroup.ConfirmedUserNumber++;
                
                if ( dbProjectGroup.ConfirmedUserNumber == dbProjectGroup.GroupMembers.Count 
                    && dbProjectGroup.ConfirmedUserNumber >= dbProjectGroup.AffiliatedCourse.MinGroupSize
                    && dbProjectGroup.ConfirmedUserNumber <= dbProjectGroup.AffiliatedCourse.MaxGroupSize ) {

                    dbProjectGroup.ConfirmationState = true;
                    _context.JoinRequests.RemoveRange ( _context.JoinRequests.Where( c => c.RequestedGroupId == dbProjectGroup.Id ) );
                    _context.MergeRequests.RemoveRange ( _context.MergeRequests.Where( c => c.ReceiverGroupId == dbProjectGroup.Id || c.SenderGroupId == dbProjectGroup.Id ) );
                    foreach ( var i in dbProjectGroup.GroupMembers )
                    {
                        _context.JoinRequests.RemoveRange ( _context.JoinRequests.Where( c => c.RequestingStudentId == i.UserId ) );
                    }
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                newConfirmedGroupMembers = RemoveUserFromString(dbProjectGroup.ConfirmedGroupMembers, GetUserId());
                dbProjectGroup.ConfirmedUserNumber--;
                dbProjectGroup.ConfirmationState = false;
            }
            dbProjectGroup.ConfirmedGroupMembers = newConfirmedGroupMembers;

            _context.ProjectGroups.Update(dbProjectGroup);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbProjectGroup);
            serviceResponse.Message = "Successfully applied the operation";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProjectGroupDto>> LeaveGroup(int projectGroupId)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .Include(c => c.GroupMembers).ThenInclude(cs => cs.User)
                .Include(c => c.AffiliatedCourse)
                .Include(c => c.AffiliatedSection)
                .FirstOrDefaultAsync(c => c.Id == projectGroupId);
            if (dbProjectGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Project group not found.";
                return serviceResponse;
            }

            if (!IsUserInGroup(dbProjectGroup, GetUserId()))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not in project group";
                return serviceResponse;
            }

            if (dbProjectGroup.GroupMembers.Count == 1)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "There is only one user in the group. Thus, the user cannot leave this group.";
                return serviceResponse;
            }

            await ConfirmationOfStudent(new ConfirmationAnswerDto { ConfirmationState = false, ProjectGroupId = projectGroupId });
            dbProjectGroup.ConfirmationState = false;
            dbProjectGroup.ConfirmedUserNumber = 0;
            dbProjectGroup.ConfirmedGroupMembers = "";

            foreach (var i in dbProjectGroup.GroupMembers)
            {
                if (i.UserId == GetUserId())
                {
                    _context.ProjectGroupUsers.Remove(i);
                    break;
                }
            }

            ProjectGroup newProjectGroup = new ProjectGroup
            {
                AffiliatedCourse = dbProjectGroup.AffiliatedCourse,
                AffiliatedCourseId = dbProjectGroup.AffiliatedCourseId,
                AffiliatedSection = dbProjectGroup.AffiliatedSection,
                AffiliatedSectionId = dbProjectGroup.AffiliatedSectionId,
                ConfirmationState = false,
                ConfirmedUserNumber = 0,
                ProjectInformation = "",
                ConfirmedGroupMembers = ""
            };
            ProjectGroupUser newProjectGroupUser = new ProjectGroupUser
            {
                User = await _context.Users
                    .FirstOrDefaultAsync(c => c.Id == GetUserId()),
                UserId = GetUserId(),
                ProjectGroup = newProjectGroup,
                ProjectGroupId = newProjectGroup.Id
            };
            newProjectGroup.GroupMembers.Add(newProjectGroupUser);
            _context.ProjectGroups.Add(newProjectGroup);

            _context.ProjectGroups.Update(dbProjectGroup);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbProjectGroup);
            serviceResponse.Message = "Successfully applied the leaving operation";
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetProjectGroupDto>>> GetProjectGroupsOfSection(int sectionId)
        {
            ServiceResponse<List<GetProjectGroupDto>> serviceResponse = new ServiceResponse<List<GetProjectGroupDto>>();
            Section dbSection = await _context.Sections
                .Include(c => c.ProjectGroups).ThenInclude(cs => cs.GroupMembers).ThenInclude(css => css.User)
                .FirstOrDefaultAsync(c => c.Id == sectionId);
            if (dbSection == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Section not found.";
                return serviceResponse;
            }

            serviceResponse.Data = dbSection.ProjectGroups.Select(c => _mapper.Map<GetProjectGroupDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetProjectGroupDto>>> GetProjectGroupsOfUser(int userId)
        {
            ServiceResponse<List<GetProjectGroupDto>> serviceResponse = new ServiceResponse<List<GetProjectGroupDto>>();
            User dbUser = await _context.Users
                .Include(c => c.ProjectGroups).ThenInclude(cs => cs.ProjectGroup).ThenInclude(cs => cs.GroupMembers).ThenInclude(css => css.User)
                .FirstOrDefaultAsync(c => c.Id == userId);
            if (dbUser == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found.";
                return serviceResponse;
            }

            serviceResponse.Data = dbUser.ProjectGroups.Select(c => _mapper.Map<GetProjectGroupDto>(c.ProjectGroup)).ToList();
            return serviceResponse;
        }

        // This method cannot be called via api.
        public async Task<ServiceResponse<string>> DeleteProjectGroup(int projectGroupId)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                ProjectGroup dbProjectGroup = await _context.ProjectGroups
                    .Include ( c => c.IncomingJoinRequests )
                    .Include ( c => c.IncomingMergeRequest )
                    .Include ( c => c.OutgoingMergeRequest )
                    .Include(pg => pg.ProjectGrades)
                    .FirstOrDefaultAsync(c => c.Id == projectGroupId);

                if (dbProjectGroup == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No such group exists";
                }
                else
                {
                    foreach (ProjectGrade pg in dbProjectGroup.ProjectGrades)
                    {
                        await _projectGradeService.DeleteWithForce(pg.Id);
                    }
                    _context.ProjectGrades.RemoveRange(_context.ProjectGrades.Where(c => c.GradedProjectGroupID == dbProjectGroup.Id));
                    _context.Submissions.RemoveRange(_context.Submissions.Where(c => c.AffiliatedGroupId == dbProjectGroup.Id));
                    _context.JoinRequests.RemoveRange(_context.JoinRequests.Where(c => c.RequestedGroupId == dbProjectGroup.Id));
                    _context.MergeRequests.RemoveRange(_context.MergeRequests.Where(c => c.ReceiverGroupId == dbProjectGroup.Id || c.SenderGroupId == dbProjectGroup.Id));

                    int cnt = 0;
                    while (dbProjectGroup.GroupMembers.Count > 0)
                    {
                        int tmp = dbProjectGroup.GroupMembers.ElementAt(0).UserId;
                        _context.ProjectGroupUsers.Remove(dbProjectGroup.GroupMembers.ElementAt(0));
                        if ( dbProjectGroup.GroupMembers.Count > 0 && dbProjectGroup.GroupMembers.ElementAt(0).UserId == tmp )
                            dbProjectGroup.GroupMembers.Remove(dbProjectGroup.GroupMembers.ElementAt(0));
                        cnt++;
                        if (cnt > 15)
                        {
                            Console.WriteLine("Aga bug aga");
                            break;
                        }
                    }

                    _context.ProjectGroups.Remove(dbProjectGroup);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = "Project Group Deleted";
                }
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProjectGroupDto>> CreateNewProjectGroupForStudentInSection(int userId, int sectionId)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();
            Section dbSection = await _context.Sections
                .Include(c => c.AffiliatedCourse)
                .FirstOrDefaultAsync(c => c.Id == sectionId);

            if (dbSection == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No such section is found.";
                return serviceResponse;
            }
            Course dbCourse = dbSection.AffiliatedCourse;
            if (dbCourse == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No such course is found.";
                return serviceResponse;
            }

            ProjectGroup newProjectGroup = new ProjectGroup
            {
                AffiliatedCourse = dbCourse,
                AffiliatedCourseId = dbCourse.Id,
                AffiliatedSection = dbSection,
                AffiliatedSectionId = dbSection.Id,
                ConfirmationState = false,
                ConfirmedUserNumber = 0,
                ProjectInformation = "",
                ConfirmedGroupMembers = ""
            };
            ProjectGroupUser newProjectGroupUser = new ProjectGroupUser
            {
                User = await _context.Users
                    .FirstOrDefaultAsync(c => c.Id == userId),
                UserId = userId,
                ProjectGroup = newProjectGroup,
                ProjectGroupId = newProjectGroup.Id
            };
            newProjectGroup.GroupMembers.Add(newProjectGroupUser);
            _context.ProjectGroups.Add(newProjectGroup);

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(newProjectGroup);
            serviceResponse.Message = "New project group for the student is created.";
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> ForceCancelGroup(int projectGroupId)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                    .Include(c => c.AffiliatedCourse)
                    .Include(c => c.GroupMembers).ThenInclude(cs => cs.User)
                    .FirstOrDefaultAsync(c => c.Id == projectGroupId);

            if (dbProjectGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No such group exists";
                return serviceResponse;
            }

            if (!(await isUserInstructorOfGroup(projectGroupId)))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not an instructor of this group's course";
                return serviceResponse;
            }

            while (dbProjectGroup.GroupMembers.Count > 1)
            {
                await KickStudentFromGroup ( projectGroupId, dbProjectGroup.GroupMembers.ElementAt(0).UserId, true );
            }

            int lastUserId = dbProjectGroup.GroupMembers.ElementAt(0).UserId, sectionIdForNew = dbProjectGroup.AffiliatedSectionId;

            await DeleteProjectGroup(projectGroupId);

            await CreateNewProjectGroupForStudentInSection(lastUserId, sectionIdForNew);

            serviceResponse.Message = "Group is canceled by the instructor/TA.";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProjectGroupDto>> KickStudentFromGroup(int projectGroupId, int userId, bool fromInside)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .Include(c => c.GroupMembers).ThenInclude(cs => cs.User)
                .Include(c => c.AffiliatedCourse)
                .Include(c => c.AffiliatedSection)
                .FirstOrDefaultAsync(c => c.Id == projectGroupId);
            if (dbProjectGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Project group not found.";
                return serviceResponse;
            }

            if ( !(await isUserInstructorOfGroup ( projectGroupId )) && !fromInside ) {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not authorized to kick the student from this group.";
                return serviceResponse;
            }

            if (!IsUserInGroup(dbProjectGroup, userId))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not in project group";
                return serviceResponse;
            }

            if (dbProjectGroup.GroupMembers.Count == 1)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "There is only one user in the group. Thus, the user cannot leave this group.";
                return serviceResponse;
            }

            dbProjectGroup.ConfirmationState = false;
            dbProjectGroup.ConfirmedUserNumber = 0;
            dbProjectGroup.ConfirmedGroupMembers = "";

            foreach (var i in dbProjectGroup.GroupMembers)
            {
                if (i.UserId == userId)
                {
                    _context.ProjectGroupUsers.Remove(i);
                    break;
                }
            }

            ProjectGroup newProjectGroup = new ProjectGroup
            {
                AffiliatedCourse = dbProjectGroup.AffiliatedCourse,
                AffiliatedCourseId = dbProjectGroup.AffiliatedCourseId,
                AffiliatedSection = dbProjectGroup.AffiliatedSection,
                AffiliatedSectionId = dbProjectGroup.AffiliatedSectionId,
                ConfirmationState = false,
                ConfirmedUserNumber = 0,
                ProjectInformation = "",
                ConfirmedGroupMembers = ""
            };
            ProjectGroupUser newProjectGroupUser = new ProjectGroupUser
            {
                User = await _context.Users
                    .FirstOrDefaultAsync(c => c.Id == userId),
                UserId = userId,
                ProjectGroup = newProjectGroup,
                ProjectGroupId = newProjectGroup.Id
            };
            newProjectGroup.GroupMembers.Add(newProjectGroupUser);
            _context.ProjectGroups.Add(newProjectGroup);

            _context.ProjectGroups.Update(dbProjectGroup);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbProjectGroup);
            serviceResponse.Message = "Successfully applied the kicking operation";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProjectGroupDto>> CompleteJoinRequest(int joinRequestId)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();

            JoinRequest dbJoinRequest = await _context.JoinRequests
                .Include(c => c.RequestedGroup).ThenInclude(cs => cs.GroupMembers).ThenInclude(css => css.User)
                .Include(c => c.RequestingStudent)
                .FirstOrDefaultAsync(c => c.Id == joinRequestId);

            if (dbJoinRequest == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Cannot find join request by the given id.";
                return serviceResponse;
            }

            if (!dbJoinRequest.Accepted && !(await isUserInstructorOfGroup(dbJoinRequest.RequestedGroupId)))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Neither user is not an instructor of this course nor the request is accepted by all people.";
                return serviceResponse;
            }

            if (((dbJoinRequest.RequestedGroup.GroupMembers.Count + 1) > (await _context.Courses.FirstOrDefaultAsync(c => c.Id == dbJoinRequest.RequestedGroup.AffiliatedCourseId)).MaxGroupSize)
                // && !(await isUserInstructorOfGroup( dbJoinRequest.RequestedGroupId ))
                )
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "This operation exceeds the maximum group size allowed. Thus, it is not permitted.";
                return serviceResponse;
            }

            ProjectGroup singleUsersGroup = await _context.ProjectGroups
                .Include(c => c.GroupMembers)
                .FirstOrDefaultAsync(c => c.AffiliatedCourseId == dbJoinRequest.RequestedGroup.AffiliatedCourseId && c.GroupMembers.Any(cs => cs.UserId == dbJoinRequest.RequestingStudentId));

            if (singleUsersGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "An error occured while trying to find user's group.";
                return serviceResponse;
            }

            if ( singleUsersGroup.GroupMembers.Count == 1 )
                await DeleteProjectGroup ( singleUsersGroup.Id );
            else {
                singleUsersGroup.ConfirmationState = false;
                singleUsersGroup.ConfirmedUserNumber = 0;
                singleUsersGroup.ConfirmedGroupMembers = "";

                foreach ( var i in singleUsersGroup.GroupMembers ) {
                    if ( i.UserId == dbJoinRequest.RequestingStudentId ) {
                        _context.ProjectGroupUsers.Remove(i);
                        break;
                    }
                }
                _context.ProjectGroups.Update ( singleUsersGroup );
            }

            ProjectGroupUser newProjectGroupUser = new ProjectGroupUser
            {
                User = dbJoinRequest.RequestingStudent,
                UserId = dbJoinRequest.RequestingStudentId,
                ProjectGroup = dbJoinRequest.RequestedGroup,
                ProjectGroupId = dbJoinRequest.RequestedGroup.Id
            };
            dbJoinRequest.RequestedGroup.GroupMembers.Add(newProjectGroupUser);
            _context.ProjectGroups.Update(dbJoinRequest.RequestedGroup);

            List<JoinRequest> resetedJoinRequests = await _context.JoinRequests
                .Where( c => c.RequestedGroupId == dbJoinRequest.RequestedGroupId  ).ToListAsync() ;
            
            foreach ( var i in resetedJoinRequests )
            {
                i.VotedStudents = "";
                i.AcceptedNumber = 0;
                i.Accepted = false;
            }
            _context.JoinRequests.UpdateRange ( resetedJoinRequests );

            List<MergeRequest> resetedMergeRequests = await _context.MergeRequests
                .Where( c => c.ReceiverGroupId == dbJoinRequest.RequestedGroupId || c.SenderGroupId == dbJoinRequest.RequestedGroupId  ).ToListAsync() ;
            
            foreach ( var i in resetedMergeRequests )
            {
                i.VotedStudents = "";
                i.Accepted = false;
            }
            _context.MergeRequests.UpdateRange ( resetedMergeRequests );

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbJoinRequest.RequestedGroup);
            serviceResponse.Message = "Join request is completed.";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProjectGroupDto>> CompleteMergeRequest(int mergeRequestId)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();

            MergeRequest dbMergeRequest = await _context.MergeRequests
                .Include(c => c.SenderGroup).ThenInclude(cs => cs.GroupMembers).ThenInclude(css => css.User)
                .Include(c => c.ReceiverGroup).ThenInclude(cs => cs.GroupMembers).ThenInclude(css => css.User)
                .FirstOrDefaultAsync(c => c.Id == mergeRequestId);

            if (dbMergeRequest == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Cannot find merge request by the given id.";
                return serviceResponse;
            }

            if (dbMergeRequest.SenderGroup == null || dbMergeRequest.ReceiverGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Cannot find the merge request groups. Make sure the MergeRequest object is in correct form.";
                return serviceResponse;
            }

            if (!dbMergeRequest.Accepted && !(await isUserInstructorOfGroup(dbMergeRequest.ReceiverGroupId)))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Neither user is not an instructor of this course nor the request is accepted by all people.";
                return serviceResponse;
            }

            if (((dbMergeRequest.ReceiverGroup.GroupMembers.Count + dbMergeRequest.SenderGroup.GroupMembers.Count) > (await _context.Courses.FirstOrDefaultAsync(c => c.Id == dbMergeRequest.ReceiverGroup.AffiliatedCourseId)).MaxGroupSize)
                // && !(await isUserInstructorOfGroup( dbMergeRequest.ReceiverGroupId ))
                )
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "This operation exceeds the maximum group size allowed. Thus, it is not permitted.";
                return serviceResponse;
            }

            while (dbMergeRequest.SenderGroup.GroupMembers.Count > 1)
            {
                int currentUserId = dbMergeRequest.SenderGroup.GroupMembers.ElementAt(0).User.Id;
                User tmpUser = dbMergeRequest.SenderGroup.GroupMembers.ElementAt(0).User;

                dbMergeRequest.SenderGroup.ConfirmationState = false;
                dbMergeRequest.SenderGroup.ConfirmedUserNumber = 0;
                dbMergeRequest.SenderGroup.ConfirmedGroupMembers = "";

                foreach ( var i in dbMergeRequest.SenderGroup.GroupMembers ) {
                    if ( i.UserId == dbMergeRequest.SenderGroup.GroupMembers.ElementAt(0).UserId ) {
                        _context.ProjectGroupUsers.Remove(i);
                        break;
                    }
                }

                if ( dbMergeRequest.SenderGroup.GroupMembers.ElementAt(0).User.Id == currentUserId )
                    dbMergeRequest.SenderGroup.GroupMembers.Remove ( dbMergeRequest.SenderGroup.GroupMembers.ElementAt(0) );

                ProjectGroupUser newProjectGroupUserTmp = new ProjectGroupUser {
                    User = tmpUser,
                    UserId = tmpUser.Id,
                    ProjectGroup = dbMergeRequest.ReceiverGroup,
                    ProjectGroupId = dbMergeRequest.ReceiverGroupId
                };
                dbMergeRequest.ReceiverGroup.GroupMembers.Add(newProjectGroupUserTmp);
                _context.ProjectGroups.Update(dbMergeRequest.ReceiverGroup);
            }

            User lastUser = dbMergeRequest.SenderGroup.GroupMembers.ElementAt(0).User;
            await DeleteProjectGroup(dbMergeRequest.SenderGroup.Id);

            ProjectGroupUser newProjectGroupUser = new ProjectGroupUser
            {
                User = lastUser,
                UserId = lastUser.Id,
                ProjectGroup = dbMergeRequest.ReceiverGroup,
                ProjectGroupId = dbMergeRequest.ReceiverGroupId
            };
            dbMergeRequest.ReceiverGroup.GroupMembers.Add(newProjectGroupUser);
            _context.ProjectGroups.Update(dbMergeRequest.ReceiverGroup);

            // TEST ETMEK LAZIM
            List<JoinRequest> resetedJoinRequests = await _context.JoinRequests
                .Where( c => c.RequestedGroupId == dbMergeRequest.SenderGroupId || c.RequestedGroupId == dbMergeRequest.ReceiverGroupId  ).ToListAsync() ;
            
            foreach ( var i in resetedJoinRequests )
            {
                i.VotedStudents = "";
                i.AcceptedNumber = 0;
                i.Accepted = false;
            }
            _context.JoinRequests.UpdateRange ( resetedJoinRequests );

            List<MergeRequest> resetedMergeRequests = await _context.MergeRequests
                .Where( c => c.ReceiverGroupId == dbMergeRequest.ReceiverGroupId 
                    || c.ReceiverGroupId == dbMergeRequest.SenderGroupId
                    || c.SenderGroupId == dbMergeRequest.ReceiverGroupId 
                    || c.SenderGroupId == dbMergeRequest.SenderGroupId ).ToListAsync() ;
            
            foreach ( var i in resetedMergeRequests )
            {
                i.VotedStudents = "";
                i.Accepted = false;
            }
            _context.MergeRequests.UpdateRange ( resetedMergeRequests );
            //

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbMergeRequest.ReceiverGroup);
            serviceResponse.Message = "Merge request is completed.";
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ProjectGradeInfoDto>>> GetInstructorComments(int projectGroupId)
        {
            ServiceResponse<List<ProjectGradeInfoDto>> response = new ServiceResponse<List<ProjectGradeInfoDto>>();
            ProjectGroup projectGroup = await _context.ProjectGroups.Include(s => s.AffiliatedCourse).ThenInclude(pg => pg.Instructors)
                .Include(s => s.ProjectGrades).Include(s => s.AffiliatedSection).FirstOrDefaultAsync(s => s.Id == projectGroupId);
            if (projectGroup == null)
            {
                response.Data = null;
                response.Message = "There is no such project Group";
                response.Success = false;
                return response;
            }

            List<ProjectGradeInfoDto> projectGrades = _context.ProjectGrades
                .Include(c => c.GradingUser).Where(c => c.GradingUser.UserType == UserTypeClass.Instructor)
                .Select(c => new ProjectGradeInfoDto
                {
                    Id = c.Id,
                    MaxGrade = c.MaxGrade,
                    Grade = c.Grade,
                    Comment = c.Description,
                    CreatedAt = c.CreatedAt,
                    userInProjectGradeDto = new UserInProjectGradeDto
                    {
                        Id = c.GradingUser.Id,
                        email = c.GradingUser.Email,
                        name = c.GradingUser.Name
                    },
                    GradingUserId = c.GradingUserId,

                    FileEndpoint = string.Format("ProjectGrade/DownloadById/{0}", c.Id),
                    GradedProjectGroupID = c.GradedProjectGroup.Id
                }).ToList();

            response.Data = projectGrades;
            return response;
        }

        public async Task<ServiceResponse<List<ProjectGradeInfoDto>>> GetTaComments(int projectGroupId)
        {
            ServiceResponse<List<ProjectGradeInfoDto>> response = new ServiceResponse<List<ProjectGradeInfoDto>>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups.Include(s => s.AffiliatedCourse).ThenInclude(pg => pg.Instructors)
                .Include(s => s.ProjectGrades).Include(s => s.AffiliatedSection).FirstOrDefaultAsync(s => s.Id == projectGroupId);
            if (projectGroup == null)
            {
                response.Data = null;
                response.Message = "There is no such project Group";
                response.Success = false;
                return response;
            }
            Course course = projectGroup.AffiliatedCourse;
            List<int> intrs = new List<int>();
            intrs = course.Instructors.Select(cu => cu.UserId).ToList();
            List<ProjectGradeInfoDto> projectGrades = _context.ProjectGrades
                .Include(c => c.GradingUser)
                .Where(c => c.GradingUser.UserType == UserTypeClass.Instructor)
                .Where(c => c.GradingUser.UserType == UserTypeClass.Student)
                .Where(c => intrs.Contains(c.GradingUserId))
                .Select(c => new ProjectGradeInfoDto
                {
                    Id = c.Id,
                    MaxGrade = c.MaxGrade,
                    Grade = c.Grade,
                    Comment = c.Description,
                    CreatedAt = c.CreatedAt,
                    userInProjectGradeDto = new UserInProjectGradeDto
                    {
                        Id = c.GradingUser.Id,
                        email = c.GradingUser.Email,
                        name = c.GradingUser.Name
                    },
                    GradingUserId = c.GradingUserId,

                    FileEndpoint = string.Format("ProjectGrade/DownloadById/{0}", c.Id),
                    GradedProjectGroupID = c.GradedProjectGroup.Id
                }).ToList();

            response.Data = projectGrades;
            return response;
        }

        public async Task<ServiceResponse<List<ProjectGradeInfoDto>>> GetStudentComments(int projectGroupId)
        {
            ServiceResponse<List<ProjectGradeInfoDto>> response = new ServiceResponse<List<ProjectGradeInfoDto>>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups.Include(s => s.AffiliatedCourse).ThenInclude(pg => pg.Instructors)
                .Include(s => s.ProjectGrades).Include(s => s.AffiliatedSection).FirstOrDefaultAsync(s => s.Id == projectGroupId);
            if (projectGroup == null)
            {
                response.Data = null;
                response.Message = "There is no such project Group";
                response.Success = false;
                return response;
            }
            Course course = projectGroup.AffiliatedCourse;
            List<int> intrs = new List<int>();
            intrs = course.Instructors.Select(cu => cu.UserId).ToList();
            List<ProjectGradeInfoDto> projectGrades = _context.ProjectGrades
                .Include(c => c.GradingUser)
                .Where(c => c.GradingUser.UserType == UserTypeClass.Instructor)
                .Where(c => c.GradingUser.UserType == UserTypeClass.Student)
                .Where(c => !intrs.Contains(c.GradingUserId))
                .Select(c => new ProjectGradeInfoDto
                {
                    Id = c.Id,
                    MaxGrade = c.MaxGrade,
                    Grade = c.Grade,
                    Comment = c.Description,
                    CreatedAt = c.CreatedAt,
                    userInProjectGradeDto = new UserInProjectGradeDto
                    {
                        Id = c.GradingUser.Id,
                        email = c.GradingUser.Email,
                        name = c.GradingUser.Name
                    },
                    GradingUserId = c.GradingUserId,

                    FileEndpoint = string.Format("ProjectGrade/DownloadById/{0}", c.Id),
                    GradedProjectGroupID = c.GradedProjectGroup.Id
                }).ToList();

            response.Data = projectGrades;
            return response;
        }

        public async
         Task<ServiceResponse<decimal>> GetGradeWithGraderId(int projectGroupId, int graderId)
        {
            ServiceResponse<decimal> response = new ServiceResponse<decimal>();
            ProjectGroup projectGroup = await _context.ProjectGroups.Include(s => s.ProjectGrades).FirstOrDefaultAsync(c => c.Id == projectGroupId);
            if (projectGroup == null)
            {
                response.Data = -1;
                response.Message = "There is no project grade with this Id";
                response.Success = false;
                return response;
            }
            if (graderId == -1)
            {
                return await GetStudentsAverage(projectGroupId);
            }
            else
            {
                User grader = await _context.Users.FirstOrDefaultAsync(c => c.Id == graderId);
                if (grader == null)
                {
                    response.Data = -1;
                    response.Message = "There is no user with this Id";
                    response.Success = false;
                    return response;
                }
                ProjectGrade projectGrade = projectGroup.ProjectGrades.FirstOrDefault(c => c.GradingUserId == grader.Id);
                if (projectGrade == null || projectGrade.MaxGrade == 0)
                {
                    response.Data = -1;
                    response.Message = "This user did not send his feedback grade for this assignment";
                    response.Success = false;
                    return response;
                }
                response.Data = Decimal.Round(projectGrade.Grade / projectGrade.MaxGrade * 100, 2);
                return response;
            }

        }

        public async Task<ServiceResponse<decimal>> GetStudentsAverage(int projectGroupId)
        {
            ServiceResponse<decimal> response = new ServiceResponse<decimal>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups.Include(s => s.AffiliatedCourse).ThenInclude(pg => pg.Instructors)
                .Include(s => s.ProjectGrades).Include(s => s.AffiliatedSection).FirstOrDefaultAsync(s => s.Id == projectGroupId);
            if (projectGroup == null)
            {
                response.Data = -1;
                response.Message = "There is no such project Group";
                response.Success = false;
                return response;
            }
            Course course = projectGroup.AffiliatedCourse;
            List<int> intrs = new List<int>();
            intrs = course.Instructors.Select(cu => cu.UserId).ToList();
            List<decimal> projectGrades = _context.ProjectGrades
                .Include(c => c.GradingUser)
                .Where(c => c.GradingUser.UserType == UserTypeClass.Instructor)
                .Where(c => c.GradingUser.UserType == UserTypeClass.Student)
                .Where(c => !intrs.Contains(c.GradingUserId))
                .Where(c => c.MaxGrade > 0)
                .Select(c => decimal.Round(c.Grade / c.MaxGrade * 100, 2)).ToList();


            if (projectGrades.Count > 0)
                response.Data = projectGrades.Average();
            else
                response.Data = -1;
            return response;
        }

        public async Task<ServiceResponse<GetProjectGroupDto>> UpdateSrsGrade(UpdateSrsGradeDto updateSrsGradeDto)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .FirstOrDefaultAsync(c => c.Id == updateSrsGradeDto.ProjectGroupId);
            if (dbProjectGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Project group not found.";
                return serviceResponse;
            }

            if ( !(await isUserInstructorOfGroup(updateSrsGradeDto.ProjectGroupId)))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not an instructor of this group.";
                return serviceResponse;
            }

            dbProjectGroup.SrsGrade = updateSrsGradeDto.SrsGrade;
            dbProjectGroup.IsGraded = true;
            _context.ProjectGroups.Update(dbProjectGroup);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbProjectGroup);
            serviceResponse.Message = "Successfully updated Srs grade";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetSrsGradeDto>> GetSrsGrade(int projectGroupId)
        {
            ServiceResponse<GetSrsGradeDto> serviceResponse = new ServiceResponse<GetSrsGradeDto>();
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .Include( c => c.GroupMembers )
                .FirstOrDefaultAsync(c => c.Id == projectGroupId);

            if (dbProjectGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Project group not found.";
                return serviceResponse;
            }

            if ( !(await isUserInstructorOfGroup(projectGroupId)) && !( dbProjectGroup.GroupMembers.Any( c => c.UserId == GetUserId() ) ))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not an instructor of this group and not a part of this project group.";
                return serviceResponse;
            }

            GetSrsGradeDto data = new GetSrsGradeDto
            { 
                SrsGrade = dbProjectGroup.SrsGrade,
                IsGraded = dbProjectGroup.IsGraded
            };

            serviceResponse.Data = data;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProjectGroupDto>> DeleteSrsGrade(DeleteSrsGradeDto deleteSrsGradeDto)
        {
            ServiceResponse<GetProjectGroupDto> serviceResponse = new ServiceResponse<GetProjectGroupDto>();
            ProjectGroup dbProjectGroup = await _context.ProjectGroups
                .FirstOrDefaultAsync(c => c.Id == deleteSrsGradeDto.ProjectGroupId);
            if (dbProjectGroup == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Project group not found.";
                return serviceResponse;
            }

            if ( !(await isUserInstructorOfGroup(deleteSrsGradeDto.ProjectGroupId)))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User is not an instructor of this group.";
                return serviceResponse;
            }

            dbProjectGroup.SrsGrade = 0;
            dbProjectGroup.IsGraded = false;
            _context.ProjectGroups.Update(dbProjectGroup);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbProjectGroup);
            serviceResponse.Message = "Successfully deleted the Srs grade";
            return serviceResponse;
        }
    }
}