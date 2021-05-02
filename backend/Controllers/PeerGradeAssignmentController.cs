using System.Threading.Tasks;
using backend.Models;
using backend.Services.PeerGradeAssignmentServices;
using backend.Dtos.PeerGradeAssignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Hosting;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PeerGradeAssignmentController : ControllerBase
    {
        private readonly IPeerGradeAssignmentService _peerGradeAssignmentService;

        private IWebHostEnvironment _hostingEnvironment;
        public PeerGradeAssignmentController(IPeerGradeAssignmentService peerGradeAssignmentService, IWebHostEnvironment hostingEnvironment)
        {
            _peerGradeAssignmentService = peerGradeAssignmentService;
            _hostingEnvironment = hostingEnvironment;
        }

        
        [HttpPost]
        [Route("{courseId}/{maxGrade}/{dueDate}")]
        public async Task<IActionResult> Add(int courseId, int maxGrade, DateTime dueDate)
        {            
            AddPeerGradeAssignmentDto dto = new AddPeerGradeAssignmentDto {
                                                        CourseId = courseId,
                                                        LastEdited = DateTime.Now,
                                                        MaxGrade = maxGrade,
                                                        DueDate = dueDate
                                                    };
            ServiceResponse<AddPeerGradeAssignmentDto> response = await _peerGradeAssignmentService.AddPeerGradeAssignment(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete]
        [Route("{peerGradeAssignmentId}")]
        public async Task<IActionResult> Delete(int peerGradeAssignmentId )
        {     
            ServiceResponse<string> response = await _peerGradeAssignmentService.DeletePeerGradeAssignment(peerGradeAssignmentId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut]
        [Route("{peerGradeAssignmentId}/{maxGrade}/{dueDate}")]
        public async Task<IActionResult> Edit(int peerGradeAssignmentId, int maxGrade, DateTime dueDate )
        {     
            EditPeerGradeAssignmentDto dto = new EditPeerGradeAssignmentDto {
                                        Id = peerGradeAssignmentId,
                                        LastEdited = DateTime.Now,
                                        MaxGrade = maxGrade,
                                        DueDate = dueDate
                                    };
            
            ServiceResponse<PeerGradeAssignmentInfoDto> response = await _peerGradeAssignmentService.EditPeerGradeAssignment(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("{courseId}")]
        public async Task<IActionResult> GetByCourse( int courseId )
        {
            ServiceResponse<PeerGradeAssignmentInfoDto> response = await _peerGradeAssignmentService.GetPeerGradeAssignmentByCourseId( courseId );
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}