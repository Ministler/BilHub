using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using backend.Models;
using backend.Data;
using backend.Dtos.PeerGradeAssignment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Text;
using backend.Services.ProjectGroupServices;

namespace backend.Services.PeerGradeAssignmentServices
{
    public class PeerGradeAssignmentService : IPeerGradeAssignmentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;

        public PeerGradeAssignmentService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
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

        public async Task<ServiceResponse<AddPeerGradeAssignmentDto>> AddPeerGradeAssignment(AddPeerGradeAssignmentDto dto)
        {
            ServiceResponse<AddPeerGradeAssignmentDto> response = new ServiceResponse<AddPeerGradeAssignmentDto>();
            User user = await _context.Users.Include(u => u.InstructedCourses)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());
            Course course = await _context.Courses
                                            .FirstOrDefaultAsync(rg => rg.Id == dto.CourseId );

            if( course == null )
            {
                response.Data = null;
                response.Message = "There is no course with this id";
                response.Success = false;
                return response;
            }

            if( !doesUserInstruct( user, dto.CourseId ) )
            {
                response.Data = null;
                response.Message = "You are not instructing this course";
                response.Success = false;
                return response;
            }
    
            PeerGradeAssignment pga = await _context.PeerGradeAssignments.Include( pga => pga.AffiliatedCourse )
                                                        .FirstOrDefaultAsync( pga => pga.CourseId == dto.CourseId );
            
            if( pga != null )
            {
                response.Data = null;
                response.Message = "There is already a peer grading assignment for the course";
                response.Success = false;
                return response;
            }

            if( dto.MaxGrade < 0 )
            {
                response.Data = null;
                response.Message = "Max grade should not be negative";
                response.Success = false;
                return response;
            }

            
            PeerGradeAssignment createdPga = new PeerGradeAssignment
            {
                AffiliatedCourse = course,
                CourseId = dto.CourseId,
                DueDate = dto.DueDate,
                LastEdited = dto.LastEdited,
                MaxGrade = dto.MaxGrade
            };

            _context.PeerGradeAssignments.Add( createdPga );
            await _context.SaveChangesAsync();

            course.PeerGradeAssignment = pga;

            response.Data = dto;
            response.Message = "You successfully entered peer grade assignment";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<PeerGradeAssignmentInfoDto>> EditPeerGradeAssignment(EditPeerGradeAssignmentDto dto)
        {
            ServiceResponse<PeerGradeAssignmentInfoDto> response = new ServiceResponse<PeerGradeAssignmentInfoDto>();
            User user = await _context.Users.Include(u => u.InstructedCourses)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());

            PeerGradeAssignment pga = await _context.PeerGradeAssignments.FirstOrDefaultAsync( pga => pga.Id == dto.Id );

            if( pga == null) {
                response.Data = null;
                response.Message = "There is no peer grade assignment with this Id.";
                response.Success = false;
                return response;
            }

            if( !doesUserInstruct( user, pga.CourseId ) )
            {
                response.Data = null;
                response.Message = "You are not instructing this course";
                response.Success = false;
                return response;
            }

            if( dto.MaxGrade < 0 )
            {
                response.Data = null;
                response.Message = "Max grade should not be negative";
                response.Success = false;
                return response;
            }

            pga.MaxGrade = dto.MaxGrade;
            pga.DueDate = dto.DueDate;
            pga.LastEdited = dto.LastEdited;
            
            _context.PeerGradeAssignments.Update( pga);
            await _context.SaveChangesAsync();

            PeerGradeAssignmentInfoDto pgaInfoDto = new PeerGradeAssignmentInfoDto
            {
                Id = pga.Id,
                CourseId = pga.CourseId,
                MaxGrade = pga.MaxGrade,
                DueDate = pga.DueDate,
                LastEdited = pga.LastEdited,
            };

            response.Data = pgaInfoDto;
            response.Message = "You successfully edited peer grade assignment";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<string>> DeletePeerGradeAssignment(int Id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.Include(u => u.InstructedCourses)
                                .FirstOrDefaultAsync(u => u.Id == GetUserId());

            PeerGradeAssignment pga = await _context.PeerGradeAssignments.FirstOrDefaultAsync( pga => pga.Id == Id );

            if( pga == null) {
                response.Data = null;
                response.Message = "There is no peer grade assignment with this Id.";
                response.Success = false;
                return response;
            }

            if( !doesUserInstruct( user, pga.CourseId ) )
            {
                response.Data = null;
                response.Message = "You are not instructing this course";
                response.Success = false;
                return response;
            }
            

            _context.PeerGradeAssignments.Remove( pga );
            await _context.SaveChangesAsync();

            response.Data = "Successful";
            response.Message = "Peer grade assignment is successfully cancelled";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<PeerGradeAssignmentInfoDto>> GetPeerGradeAssignmentByCourseId(int courseId)
        {
            ServiceResponse<PeerGradeAssignmentInfoDto> response = new ServiceResponse<PeerGradeAssignmentInfoDto>();
            PeerGradeAssignment pga = await _context.PeerGradeAssignments.FirstOrDefaultAsync( pga => pga.CourseId == courseId );

            if( pga == null) {
                response.Data = null;
                response.Message = "There is no peer grade assignment with this Id.";
                response.Success = false;
                return response;
            }

            PeerGradeAssignmentInfoDto pgaInfoDto = new PeerGradeAssignmentInfoDto
            {
                Id = pga.Id,
                CourseId = pga.CourseId,
                MaxGrade = pga.MaxGrade,
                DueDate = pga.DueDate,
                LastEdited = pga.LastEdited,
            };

            response.Data = pgaInfoDto;
            response.Message = "success";
            response.Success = true;
            
            return response;
        }
    }
}