using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using backend.Models;
using backend.Data;
using backend.Dtos.ProjectGrade;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Text;
using backend.Services.ProjectGroupServices;

//edit will probably be edited

namespace backend.Services.ProjectGradeServices
{
    public class ProjectGradeService : IProjectGradeService
    {
        // peer grade due date doesn't exist
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;

        public ProjectGradeService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<AddProjectGradeDto>> AddProjectGrade(AddProjectGradeDto addProjectGradeDto)
        {
            ServiceResponse<AddProjectGradeDto> response = new ServiceResponse<AddProjectGradeDto>();
            User user = await _context.Users
                                .Include(u => u.InstructedCourses)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups.Include(pg => pg.AffiliatedCourse)
                                            .FirstOrDefaultAsync(rg => rg.Id == addProjectGradeDto.GradedProjectGroupID);

            if (projectGroup == null)
            {
                response.Data = null;
                response.Message = "There is no project group with this id";
                response.Success = false;
                return response;
            }

            /*
            if( user == null ||  !doesUserInstruct( user, projectGroup.AffiliatedCourseId ))
            {
                response.Data = null;
                response.Message = "You are not authorized to grade this group";
                response.Success = false;
                return response;
            }*/

            if (addProjectGradeDto.MaxGrade < addProjectGradeDto.Grade)
            {
                response.Data = null;
                response.Message = "Grade should be less than or equal to the max grade";
                response.Success = false;
                return response;
            }

            if (addProjectGradeDto.MaxGrade < 0)
            {
                response.Data = null;
                response.Message = "Max grade should not be negative";
                response.Success = false;
                return response;
            }


            ProjectGrade projectGrade = await _context.ProjectGrades
                .FirstOrDefaultAsync(pg => pg.GradingUserId == GetUserId() && pg.GradedProjectGroupID == addProjectGradeDto.GradedProjectGroupID);

            //herkes bir kez gradelemek?????
            if (projectGrade != null)
            {
                response.Data = null;
                response.Message = "You have already graded this group ";
                response.Success = false;
                return response;
            }

            ProjectGrade createdProjectGrade = new ProjectGrade
            {
                MaxGrade = addProjectGradeDto.MaxGrade,
                Grade = addProjectGradeDto.Grade,
                Description = addProjectGradeDto.Comment,
                LastEdited = addProjectGradeDto.LastEdited,
                GradingUser = user,
                GradingUserId = user.Id,
                GradedProjectGroup = projectGroup,
                GradedProjectGroupID = projectGroup.Id,
            };

            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/ProjectGradeFiles", projectGroup.AffiliatedCourseId,
                projectGroup.AffiliatedSection, projectGroup.Id, user.Id));

            Directory.CreateDirectory(target);
            if (addProjectGradeDto.File != null)
            {
                if (addProjectGradeDto.File.Length > 0)
                {
                    createdProjectGrade.HasFile = true;
                    string oldfile = Directory.GetFiles(target).FirstOrDefault();
                    string extension = Path.GetExtension(addProjectGradeDto.File.FileName);
                    var filePath = Path.Combine(target, string.Format("{0}_Section{1}_Group{2}_{3}_GroupFeedback"
                        , projectGroup.AffiliatedCourse.Name.Trim().Replace(" ", "_"), projectGroup.AffiliatedSection,
                         projectGroup.Id, user.Name.Trim().Replace(" ", "_")) + extension);
                    createdProjectGrade.FilePath = filePath;
                    if (File.Exists(oldfile))
                    {
                        File.Delete(oldfile);
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await addProjectGradeDto.File.CopyToAsync(stream);
                    }
                }
            }




            _context.ProjectGrades.Add(createdProjectGrade);
            await _context.SaveChangesAsync();

            projectGroup.ProjectGrades.Add(createdProjectGrade);

            response.Data = addProjectGradeDto;
            response.Message = "You successfully entered project grade";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<ProjectGradeInfoDto>> EditProjectGrade(EditProjectGradeDto editProjectGradeDto)
        {
            ServiceResponse<ProjectGradeInfoDto> response = new ServiceResponse<ProjectGradeInfoDto>();
            User user = await _context.Users
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGrade projectGrade = await _context.ProjectGrades
                                .Include(pg => pg.GradedProjectGroup)
                                .FirstOrDefaultAsync(pg => pg.Id == editProjectGradeDto.Id);
            ProjectGroup projectGroup = await _context.ProjectGroups.Include(pg => pg.AffiliatedCourse).Include(c => c.AffiliatedSection)
                        .FirstOrDefaultAsync(rg => rg.Id == projectGrade.GradedProjectGroupID);

            if (projectGrade == null)
            {
                response.Data = null;
                response.Message = "There is no project grade with this Id.";
                response.Success = false;
                return response;
            }

            if (editProjectGradeDto.MaxGrade < editProjectGradeDto.Grade)
            {
                response.Data = null;
                response.Message = "Grade should be less than or equal to the max grade";
                response.Success = false;
                return response;
            }

            projectGrade.MaxGrade = projectGrade.MaxGrade; // ask
            projectGrade.Grade = projectGrade.Grade;
            projectGrade.Description = editProjectGradeDto.Comment;
            projectGrade.LastEdited = editProjectGradeDto.LastEdited; // ask
            if (editProjectGradeDto.File != null)
            {
                var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                    "StaticFiles/ProjectGradeFiles", projectGrade.GradedProjectGroup.AffiliatedCourseId,
                    projectGrade.GradedProjectGroup.AffiliatedSection, projectGrade.GradedProjectGroupID, user.Id));

                Directory.CreateDirectory(target);
                if (editProjectGradeDto.File.Length > 0)
                {
                    projectGrade.HasFile = true;
                    string oldfile = Directory.GetFiles(target).FirstOrDefault();
                    string extension = Path.GetExtension(editProjectGradeDto.File.FileName);
                    var filePath = Path.Combine(target, string.Format("{0}_Section{1}_Group{2}_{3}_GroupFeedback"
                        , projectGroup.AffiliatedCourse.Name.Trim().Replace(" ", "_"), projectGroup.AffiliatedSection,
                         projectGroup.Id, user.Name.Trim().Replace(" ", "_")) + extension);
                    projectGrade.FilePath = filePath;
                    if (File.Exists(oldfile))
                    {
                        File.Delete(oldfile);
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await editProjectGradeDto.File.CopyToAsync(stream);
                    }
                }
            }


            ProjectGradeInfoDto projectGradeInfoDto = new ProjectGradeInfoDto
            {
                Id = projectGrade.Id,
                MaxGrade = editProjectGradeDto.MaxGrade,
                Grade = editProjectGradeDto.Grade,
                Comment = editProjectGradeDto.Comment,
                LastEdited = editProjectGradeDto.LastEdited,
                userInProjectGradeDto = new UserInProjectGradeDto
                {
                    Id = user.Id,
                    email = user.Email,
                    name = user.Name
                },
                GradingUserId = user.Id,
                projectGroupInProjectGradeDto = new ProjectGroupInProjectGradeDto
                {
                    Id = user.Id,
                    AffiliatedSectionId = projectGrade.GradedProjectGroup.AffiliatedSectionId,
                    AffiliatedCourseId = projectGrade.GradedProjectGroup.AffiliatedCourseId,
                    ConfirmationState = projectGrade.GradedProjectGroup.ConfirmationState,
                    ConfirmedUserNumber = projectGrade.GradedProjectGroup.ConfirmedUserNumber,
                    ProjectInformation = projectGrade.GradedProjectGroup.ProjectInformation,
                    ConfirmedGroupMembers = projectGrade.GradedProjectGroup.ConfirmedGroupMembers
                },
                FileEndpoint = string.Format("ProjectGrade/DownloadById/{0}", projectGrade.Id),
                GradedProjectGroupID = projectGrade.GradedProjectGroup.Id,
                HasFile = projectGrade.HasFile
            };


            _context.ProjectGrades.Update(projectGrade);
            await _context.SaveChangesAsync();

            response.Data = projectGradeInfoDto;
            response.Message = "You successfully edited project grade";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<string>> DeleteProjectGrade(DeleteProjectGradeDto deleteProjectGradeDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGrade projectGrade = await _context.ProjectGrades.Include(pg => pg.GradedProjectGroup)
                                            .FirstOrDefaultAsync(pg => pg.Id == deleteProjectGradeDto.Id);

            if (projectGrade == null)
            {
                response.Data = "Not allowed";
                response.Message = "There is no project grade with this Id";
                response.Success = false;
                return response;
            }


            if (user == null || projectGrade.GradingUserId != user.Id)
            {
                response.Data = null;
                response.Message = "You are not authorized to delete the grade of this group";
                response.Success = false;
                return response;
            }

            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/ProjectGradeFiles", projectGrade.GradedProjectGroup.AffiliatedCourseId,
                projectGrade.GradedProjectGroup.AffiliatedSection, projectGrade.GradedProjectGroupID, user.Id));

            var filePath = Directory.GetFiles(target).FirstOrDefault();
            projectGrade.FilePath = null;
            if (filePath != null)
                File.Delete(filePath);
            projectGrade.HasFile = false;

            _context.ProjectGrades.Remove(projectGrade);
            await _context.SaveChangesAsync();

            response.Data = "Successful";
            response.Message = "Project grade is successfully cancelled";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<string>> DeleteWithForce(int projectGradeId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGrade projectGrade = await _context.ProjectGrades.Include(pg => pg.GradedProjectGroup)
                                            .FirstOrDefaultAsync(pg => pg.Id == projectGradeId);

            if (projectGrade == null)
            {
                response.Data = "Not allowed";
                response.Message = "There is no project grade with this Id";
                response.Success = false;
                return response;
            }


            if (!projectGrade.HasFile)
            {
                response.Data = null;
                response.Message = "There is no file to be deleted";
                response.Success = true;
                return response;
            }

            var target = Path.Combine(_hostingEnvironment.ContentRootPath, string.Format("{0}/{1}/{2}/{3}/{4}",
                "StaticFiles/ProjectGradeFiles", projectGrade.GradedProjectGroup.AffiliatedCourseId,
                projectGrade.GradedProjectGroup.AffiliatedSection, projectGrade.GradedProjectGroupID, user.Id));

            var filePath = Directory.GetFiles(target).FirstOrDefault();
            projectGrade.FilePath = null;
            if (filePath != null)
                File.Delete(filePath);
            projectGrade.HasFile = false;

            response.Data = "Successful";
            response.Message = "Project grade is successfully cancelled";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<ProjectGradeInfoDto>> GetProjectGradeById(GetProjectGradeByIdDto getProjectGradeDto)
        {
            ServiceResponse<ProjectGradeInfoDto> response = new ServiceResponse<ProjectGradeInfoDto>();
            User user = await _context.Users
                                .Include(u => u.InstructedCourses)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGrade projectGrade = await _context.ProjectGrades
                                            .Include(pg => pg.GradedProjectGroup)
                                            .Include(pg => pg.GradingUser)
                                            .FirstOrDefaultAsync(pg => pg.Id == getProjectGradeDto.Id);

            if (projectGrade == null)
            {
                response.Data = null;
                response.Message = "There is no project grade with this id";
                response.Success = false;
                return response;
            }

            ProjectGradeInfoDto dto = new ProjectGradeInfoDto
            {
                Id = projectGrade.Id,
                MaxGrade = projectGrade.MaxGrade,
                Grade = projectGrade.Grade,
                Comment = projectGrade.Description,
                LastEdited = projectGrade.LastEdited,
                FileEndpoint = string.Format("ProjectGrade/DownloadById/{0}", projectGrade.Id),
                userInProjectGradeDto = new UserInProjectGradeDto
                {
                    Id = projectGrade.GradingUser.Id,
                    email = projectGrade.GradingUser.Email,
                    name = projectGrade.GradingUser.Name
                },
                GradingUserId = user.Id,
                projectGroupInProjectGradeDto = new ProjectGroupInProjectGradeDto
                {
                    Id = user.Id,
                    AffiliatedSectionId = projectGrade.GradedProjectGroup.AffiliatedSectionId,
                    AffiliatedCourseId = projectGrade.GradedProjectGroup.AffiliatedCourseId,
                    ConfirmationState = projectGrade.GradedProjectGroup.ConfirmationState,
                    ConfirmedUserNumber = projectGrade.GradedProjectGroup.ConfirmedUserNumber,
                    ProjectInformation = projectGrade.GradedProjectGroup.ProjectInformation,
                    ConfirmedGroupMembers = projectGrade.GradedProjectGroup.ConfirmedGroupMembers
                },
                GradedProjectGroupID = projectGrade.GradedProjectGroup.Id,
                HasFile = projectGrade.HasFile
            };

            /*
            ProjectGroup projectGroup = await _context.ProjectGroups
                                            .Include( pg => pg.GroupMembers )
                                            .FirstOrDefaultAsync(rg => rg.Id == dto.GradedProjectGroupID );


            if( user == null || ( !doesUserInstruct( user, projectGroup.AffiliatedCourseId ) && user.Id != dto.GradingUserId && !IsUserInGroup( projectGroup, GetUserId() ) ) )
            {
                response.Data = null;
                response.Message = "You are not authorized to see this project grade";
                response.Success = false;
                return response;
            }*/

            response.Data = dto;
            response.Message = "Success";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<ProjectGradeInfoDto>> GetProjectGradeByUserAndGroup(GetProjectGradeDto getProjectGradeDto)
        {
            ServiceResponse<ProjectGradeInfoDto> response = new ServiceResponse<ProjectGradeInfoDto>();
            User user = await _context.Users
                                .Include(u => u.InstructedCourses)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups
                                            .Include(pg => pg.GroupMembers)
                                            .FirstOrDefaultAsync(rg => rg.Id == getProjectGradeDto.GradedProjectGroupID);
            User grader = await _context.Users
                                .Include(u => u.InstructedCourses)
                                .FirstOrDefaultAsync(u => u.Id == getProjectGradeDto.GradingUserId);

            if (projectGroup == null)
            {
                response.Data = null;
                response.Message = "There is no project group with this id";
                response.Success = false;
                return response;
            }

            ProjectGrade projectGrade = await _context.ProjectGrades.Include(pg => pg.GradingUser)
                .FirstOrDefaultAsync(pg => pg.GradingUserId == getProjectGradeDto.GradingUserId && pg.GradedProjectGroupID == getProjectGradeDto.GradedProjectGroupID);

            if (projectGrade == null)
            {
                response.Data = null;
                response.Message = "There is no such project grade";
                response.Success = false;
                return response;
            }

            ProjectGradeInfoDto dto = new ProjectGradeInfoDto
            {
                Id = projectGrade.Id,
                MaxGrade = projectGrade.MaxGrade,
                Grade = projectGrade.Grade,
                Comment = projectGrade.Description,
                LastEdited = projectGrade.LastEdited,
                userInProjectGradeDto = new UserInProjectGradeDto
                {
                    Id = projectGrade.GradingUser.Id,
                    email = projectGrade.GradingUser.Email,
                    name = projectGrade.GradingUser.Name
                },
                GradingUserId = user.Id,
                projectGroupInProjectGradeDto = new ProjectGroupInProjectGradeDto
                {
                    Id = user.Id,
                    AffiliatedSectionId = projectGrade.GradedProjectGroup.AffiliatedSectionId,
                    AffiliatedCourseId = projectGrade.GradedProjectGroup.AffiliatedCourseId,
                    ConfirmationState = projectGrade.GradedProjectGroup.ConfirmationState,
                    ConfirmedUserNumber = projectGrade.GradedProjectGroup.ConfirmedUserNumber,
                    ProjectInformation = projectGrade.GradedProjectGroup.ProjectInformation,
                    ConfirmedGroupMembers = projectGrade.GradedProjectGroup.ConfirmedGroupMembers
                },
                GradedProjectGroupID = projectGrade.GradedProjectGroup.Id,
                FileEndpoint = string.Format("ProjectGrade/DownloadById/{0}", projectGrade.Id),
                HasFile = projectGrade.HasFile
            };

            response.Data = dto;
            response.Message = "Success";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<string>> DownloadProjectGradeById(GetProjectGradeByIdDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            ProjectGrade projectGrade = await _context.ProjectGrades
                                            .FirstOrDefaultAsync(pg => pg.Id == dto.Id);

            if (projectGrade == null)
            {
                response.Data = null;
                response.Message = "There is no project grade with this id";
                response.Success = false;
                return response;
            }

            if (projectGrade.HasFile)
            {
                response.Data = null;
                response.Message = "There is no file in this project grade";
                response.Success = false;
                return response;
            }

            response.Data = projectGrade.FilePath;
            response.Success = true;
            return response;
        }


        public async Task<ServiceResponse<string>> DownloadProjectGradeByGroupAndUser(GetProjectGradeDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users
                                .Include(u => u.InstructedCourses)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            ProjectGroup projectGroup = await _context.ProjectGroups
                                            .Include(pg => pg.GroupMembers)
                                            .FirstOrDefaultAsync(rg => rg.Id == dto.GradedProjectGroupID);
            User grader = await _context.Users
                                .Include(u => u.InstructedCourses)
                                .FirstOrDefaultAsync(u => u.Id == dto.GradingUserId);

            if (projectGroup == null)
            {
                response.Data = null;
                response.Message = "There is no project group with this id";
                response.Success = false;
                return response;
            }

            ProjectGrade projectGrade = await _context.ProjectGrades.Include(pg => pg.GradingUser)
                .FirstOrDefaultAsync(pg => pg.GradingUserId == dto.GradingUserId && pg.GradedProjectGroupID == dto.GradedProjectGroupID);

            if (projectGrade == null)
            {
                response.Data = null;
                response.Message = "There is no such project grade";
                response.Success = false;
                return response;
            }

            if (!projectGrade.HasFile)
            {
                response.Data = null;
                response.Message = "There is no file in this grade";
                response.Success = false;
                return response;
            }

            response.Data = projectGrade.FilePath;
            response.Message = "Success";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<List<ProjectGradeInfoDto>>> GetProjectGradesGivenTo(int projectGroupId)
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
                .Include(c => c.GradingUser)
                .Where( c => c.GradedProjectGroupID == projectGroupId ) 
                .Select(c => new ProjectGradeInfoDto
                {
                    Id = c.Id,
                    MaxGrade = c.MaxGrade,
                    Grade = c.Grade,
                    Comment = c.Description,
                    LastEdited = c.LastEdited,
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
    }
}