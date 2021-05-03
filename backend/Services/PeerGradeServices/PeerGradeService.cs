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


namespace backend.Services.PeerGradeServices
{
    public class PeerGradeService : IPeerGradeService
    {
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
            ProjectGroup projectGroup = await _context.ProjectGroups
                                            .Include( g => g.AffiliatedCourse )
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

            PeerGradeAssignment pga = await _context.PeerGradeAssignments.Include( pg => pg.PeerGrades )
                                            .FirstOrDefaultAsync( pga => pga.CourseId == projectGroup.AffiliatedCourseId );

            if(pga == null )
            {
                response.Data = null;
                response.Message = "Course doesn't have a peer grade assignment";
                response.Success = false;
                return response;
            }

            if( pga.DueDate < addPeerGradeDto.LastEdited )
            {
                response.Data = null;
                response.Message = "Due date has passed for the peer grade assignment";
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

            
            if( pga.MaxGrade < addPeerGradeDto.Grade )
            {
                response.Data = null;
                response.Message = "Grade should be less than or equal to the max grade";
                response.Success = false;
                return response;
            }


            PeerGrade createdPeerGrade = new PeerGrade
            {
                MaxGrade = pga.MaxGrade,
                Grade = addPeerGradeDto.Grade,
                Comment = addPeerGradeDto.Comment,
                LastEdited = addPeerGradeDto.LastEdited,
                ProjectGroupId = addPeerGradeDto.ProjectGroupId,
                ReviewerId = GetUserId(),
                RevieweeId = addPeerGradeDto.RevieweeId
            };

            pga.PeerGrades.Add(createdPeerGrade);

            _context.ProjectGroups.Update( projectGroup );
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

            PeerGrade peerGrade = await _context.PeerGrades.FirstOrDefaultAsync( pg => pg.Id == editPeerGradeDto.Id );


            if( peerGrade == null) {
                response.Data = null;
                response.Message = "There is no peer grade with this Id.";
                response.Success = false;
                return response;
            }

            if( peerGrade.ReviewerId != user.Id)
            {
                response.Data = null;
                response.Message = "You are not authorized to change this peer grade";
                response.Success = false;
                return response;
            }
            ProjectGroup projectGroup = await _context.ProjectGroups
                                            .Include(g => g.GroupMembers)
                                            .FirstOrDefaultAsync(rg => rg.Id == peerGrade.ProjectGroupId );

            PeerGradeAssignment pga = await _context.PeerGradeAssignments
                                            .FirstOrDefaultAsync( pga => pga.CourseId == projectGroup.AffiliatedCourseId );

            if( pga.DueDate < editPeerGradeDto.LastEdited )
            {
                response.Data = null;
                response.Message = "Due date has passed for the peer grade assignment";
                response.Success = false;
                return response;
            }

             if( peerGrade.MaxGrade < editPeerGradeDto.Grade )
            {
                response.Data = null;
                response.Message = "Grade should be less than or equal to the max grade";
                response.Success = false;
                return response;
            }

            peerGrade.Grade = editPeerGradeDto.Grade;
            peerGrade.Comment = editPeerGradeDto.Comment;
            
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
                LastEdited = peerGrade.LastEdited
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

            ProjectGroup projectGroup = await _context.ProjectGroups
                                            .Include(g => g.GroupMembers)
                                            .FirstOrDefaultAsync(rg => rg.Id == peerGrade.ProjectGroupId );

            PeerGradeAssignment pga = await _context.PeerGradeAssignments.Include( pga=> pga.PeerGrades)
                                            .FirstOrDefaultAsync( pga => pga.CourseId == projectGroup.AffiliatedCourseId );

            if( pga.DueDate < deletePeerGradeDto.LastEdited )
            {
                response.Data = null;
                response.Message = "Due date has passed for the peer grade assignment";
                response.Success = false;
                return response;
            }

            _context.PeerGrades.Remove( peerGrade );
            await _context.SaveChangesAsync();

            if (pga != null)
            {
                _context.PeerGradeAssignments.Update(pga);
                await _context.SaveChangesAsync();
            }

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
                LastEdited = peerGrade.LastEdited
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

            PeerGradeAssignment pga = await _context.PeerGradeAssignments
                                            .FirstOrDefaultAsync( pga => pga.CourseId == projectGroup.AffiliatedCourseId );

            if(pga == null )
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
                LastEdited = peerGrade.LastEdited
            };

            response.Data = dto;
            response.Message = "Success";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<List<PeerGradeInfoDto>>> GetPeerGradesGivenTo(GetPeerGradesGivenToDto dto)
        {
            ServiceResponse<List<PeerGradeInfoDto>> response = new ServiceResponse<List<PeerGradeInfoDto>>();
            User user = await _context.Users
                                .Include( u => u.InstructedCourses )
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());

            ProjectGroup projectGroup = await _context.ProjectGroups.Include(g => g.AffiliatedCourse)
                                            .Include(g => g.GroupMembers)
                                            .FirstOrDefaultAsync(rg => rg.Id == dto.ProjectGroupId );
            
            if( projectGroup == null )
            {
                response.Data = null;
                response.Message = "There is no project group with this id";
                response.Success = false;
                return response;
            }

            if( !IsUserInGroup( projectGroup, dto.RevieweeId ) )
            {
                response.Data = null;
                response.Message = "There is no user with revieweeId in group";
                response.Success = false;
                return response;
            }

             PeerGradeAssignment pga = await _context.PeerGradeAssignments
                                            .FirstOrDefaultAsync( pga => pga.CourseId == projectGroup.AffiliatedCourseId );

            if(pga == null )
            {
                response.Data = null;
                response.Message = "Course doesn't have a peer grade assignment";
                response.Success = false;
                return response;
            }

            if( user == null || ( !doesUserInstruct( user, projectGroup.AffiliatedCourseId ) ) )
            {
                response.Data = null;
                response.Message = "You are not authorized to see these peer grades";
                response.Success = false;
                return response;
            } 

            List<PeerGradeInfoDto> peerGrades = _context.PeerGrades.Where(pg => pg.RevieweeId == dto.RevieweeId && pg.ProjectGroupId == dto.ProjectGroupId ).Select(c => _mapper.Map<PeerGradeInfoDto>(c)).ToList();
            response.Data = peerGrades;
            response.Message = "Success";
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<List<PeerGradeInfoDto>>> GetPeerGradesGivenBy(GetPeerGradesGivenByDto dto)
        {
            ServiceResponse<List<PeerGradeInfoDto>> response = new ServiceResponse<List<PeerGradeInfoDto>>();
            User user = await _context.Users
                                .Include( u => u.InstructedCourses )
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());

            ProjectGroup projectGroup = await _context.ProjectGroups.Include(g => g.AffiliatedCourse).ThenInclude( c => c.PeerGradeAssignment)
                                            .Include(g => g.GroupMembers)
                                            .FirstOrDefaultAsync(rg => rg.Id == dto.ProjectGroupId );
            
            if( projectGroup == null )
            {
                response.Data = null;
                response.Message = "There is no project group with this id";
                response.Success = false;
                return response;
            }

            if( !IsUserInGroup( projectGroup, dto.ReviewerId ) )
            {
                response.Data = null;
                response.Message = "There is no user with reviewerId in group";
                response.Success = false;
                return response;
            }
            
             PeerGradeAssignment pga = await _context.PeerGradeAssignments
                                            .FirstOrDefaultAsync( pga => pga.CourseId == projectGroup.AffiliatedCourseId );

            if(pga == null )
            {
                response.Data = null;
                response.Message = "Course doesn't have a peer grade assignment";
                response.Success = false;
                return response;
            }
            
            if( user == null || ( !doesUserInstruct( user, projectGroup.AffiliatedCourseId ) && user.Id != dto.ReviewerId  ) )
            {
                response.Data = null;
                response.Message = "You are not authorized to see these peer grades";
                response.Success = false;
                return response;
            } 

            List<PeerGradeInfoDto> peerGrades = _context.PeerGrades.Where(pg => pg.ProjectGroupId == dto.ProjectGroupId).Where( pg => pg.ReviewerId == dto.ReviewerId ).Select(c => _mapper.Map<PeerGradeInfoDto>(c)).ToList();
            response.Data = peerGrades;
            response.Message = "Success";
            response.Success = true;
            return response;
        }
    }
}