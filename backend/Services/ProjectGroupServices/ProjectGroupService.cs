using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Dtos.ProjectGroup;
using backend.Models;
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

        public ProjectGroupService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, ISubmissionService submissionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _submissionService = submissionService;
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
                if (dbProjectGroup.ConfirmedUserNumber == dbProjectGroup.GroupMembers.Count)
                {
                    dbProjectGroup.ConfirmationState = true;

                    // tum merge ve join requestleri cikarmayi implemente et

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

        // This method cannot be called via api. Only withdraw student method can call this.
        public async Task<ServiceResponse<string>> DeleteProjectGroup(int projectGroupId)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                ProjectGroup dbProjectGroup = await _context.ProjectGroups
                    .FirstOrDefaultAsync(c => c.Id == projectGroupId);

                if (dbProjectGroup == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No such group exists";
                }
                else
                {
                    _context.ProjectGrades.RemoveRange(_context.ProjectGrades.Where(c => c.GradedProjectGroupID == dbProjectGroup.Id));
                    _context.Submissions.RemoveRange(_context.Submissions.Where(c => c.AffiliatedGroupId == dbProjectGroup.Id));
                    _context.JoinRequests.RemoveRange(_context.JoinRequests.Where(c => c.RequestedGroupId == dbProjectGroup.Id));
                    _context.MergeRequests.RemoveRange(_context.MergeRequests.Where(c => c.ReceiverGroupId == dbProjectGroup.Id || c.SenderGroupId == dbProjectGroup.Id));

                    int cnt = 0;
                    while (dbProjectGroup.GroupMembers.Count > 0)
                    {
                        _context.ProjectGroupUsers.Remove(dbProjectGroup.GroupMembers.ElementAt(0));
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
                await KickStudentFromGroup(projectGroupId, dbProjectGroup.GroupMembers.ElementAt(0).UserId);
            }

            int lastUserId = dbProjectGroup.GroupMembers.ElementAt(0).UserId, sectionIdForNew = dbProjectGroup.AffiliatedSectionId;

            await DeleteProjectGroup(projectGroupId);

            await CreateNewProjectGroupForStudentInSection(lastUserId, sectionIdForNew);

            serviceResponse.Message = "Group is canceled by the instructor/TA.";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProjectGroupDto>> KickStudentFromGroup(int projectGroupId, int userId)
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

            if (!(await isUserInstructorOfGroup(projectGroupId)))
            {
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

            await DeleteProjectGroup(singleUsersGroup.Id);

            ProjectGroupUser newProjectGroupUser = new ProjectGroupUser
            {
                User = dbJoinRequest.RequestingStudent,
                UserId = dbJoinRequest.RequestingStudentId,
                ProjectGroup = dbJoinRequest.RequestedGroup,
                ProjectGroupId = dbJoinRequest.RequestedGroup.Id
            };
            dbJoinRequest.RequestedGroup.GroupMembers.Add(newProjectGroupUser);
            _context.ProjectGroups.Update(dbJoinRequest.RequestedGroup);

            // BUNDAN SONRA DIGER REQUESTLER VS NASIL OLACAK

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
                User tmpUser = dbMergeRequest.SenderGroup.GroupMembers.ElementAt(0).User;
                await KickStudentFromGroup(dbMergeRequest.SenderGroupId, dbMergeRequest.SenderGroup.GroupMembers.ElementAt(0).UserId);
                foreach (var i in dbMergeRequest.SenderGroup.GroupMembers)
                {
                    if (i.UserId == dbMergeRequest.SenderGroup.GroupMembers.ElementAt(0).UserId)
                    {
                        _context.ProjectGroupUsers.Remove(i);
                        break;
                    }
                }

                ProjectGroupUser newProjectGroupUserTmp = new ProjectGroupUser
                {
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

            // BUNDAN SONRA DIGER REQUESTLER VS NASIL OLACAK

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetProjectGroupDto>(dbMergeRequest.ReceiverGroup);
            serviceResponse.Message = "Merge request is completed.";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetOzgurDto>> GetOzgur(int groupId)
        {
            ServiceResponse<GetOzgurDto> response = new ServiceResponse<GetOzgurDto>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups
                .Include(pg => pg.Submissions).ThenInclude(s => s.Comments).ThenInclude(c => c.CommentedUser)
                .Include(pg => pg.Submissions).ThenInclude(s => s.AffiliatedAssignment)
                .FirstOrDefaultAsync(s => s.Id == groupId);
            if (projectGroup == null)
            {
                response.Data = null;
                response.Message = "There is no such project group";
                response.Success = false;
                return response;
            }

            List<Submission> submissions = projectGroup.Submissions.ToList();
            HashSet<int> graderIds = new HashSet<int>();
            foreach (Submission s in submissions)
            {
                List<Comment> comments = s.Comments.ToList();
                foreach (Comment c in comments)
                {
                    graderIds.Add(c.CommentedUserId);
                }
            }
            List<string> graderNames = new List<string>();
            foreach (int id in graderIds)
            {
                graderNames.Add(_context.Users.FirstOrDefault(u => u.Id == id).Name);
            }
            graderNames.Add("Students");
            graderIds.Add(-1);

            GetOzgurDto getOzgurDto = new GetOzgurDto();
            getOzgurDto.graders = graderNames;
            List<OzgurSubmissionDto> ozgurSubmissionDtos = new List<OzgurSubmissionDto>();
            foreach (Submission s in submissions)
            {
                OzgurSubmissionDto ozgurSubmissionDto = new OzgurSubmissionDto();
                ozgurSubmissionDto.SubmissionName = s.AffiliatedAssignment.Title;
                ozgurSubmissionDto.Grades = new List<decimal>();
                foreach (int id in graderIds)
                {
                    ServiceResponse<decimal> grade = await _submissionService.GetGradeWithGraderId(s.Id, id);
                    ozgurSubmissionDto.Grades.Add(grade.Data);
                }
                ozgurSubmissionDtos.Add(ozgurSubmissionDto);
            }
            getOzgurDto.ozgurSubmissionDtos = ozgurSubmissionDtos;
            // List<int> graderIds = submission.Comments.Select(c => c.CommentedUserId).ToList();
            response.Data = getOzgurDto;
            return response;
        }


    }
}