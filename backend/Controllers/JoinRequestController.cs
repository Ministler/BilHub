using System.Threading.Tasks;
using backend.Models;
using backend.Services.JoinRequestServices;
using backend.Dtos.JoinRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using ICSharpCode.SharpZipLib.Zip;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class JoinRequestController : ControllerBase
    {
        private readonly IJoinRequestService _joinRequestService;

        private IWebHostEnvironment _hostingEnvironment;
        public JoinRequestController(IJoinRequestService joinRequestService, IWebHostEnvironment hostingEnvironment)
        {
            _joinRequestService = joinRequestService;
            _hostingEnvironment = hostingEnvironment;
        }

        
        [HttpPost]
        [Route("{requestedGroupId}")]
        public async Task<IActionResult> Send(int requestedGroupId )
        {            
            AddJoinRequestDto dto = new AddJoinRequestDto {RequestedGroupId = requestedGroupId, CreatedAt = DateTime.Now};
            ServiceResponse<AddJoinRequestDto> response = await _joinRequestService.SendJoinRequest(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete]
        [Route("{joinRequestId}")]
        public async Task<IActionResult> Cancel(int joinRequestId )
        {     
            CancelJoinRequestDto dto = new CancelJoinRequestDto {Id = joinRequestId};
            //CancelJoinRequestDto dto = new CancelJoinRequestDto {Id = joinRequestId, RequestingStudentId = requestingStudentId, RequestedGroupId = requestedGroupId};
            ServiceResponse<string> response = await _joinRequestService.CancelJoinRequest(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut]
        [Route("{joinRequestId}/{_accept}")]
        public async Task<IActionResult> Vote(int joinRequestId, bool _accept )
        {     
            VoteJoinRequestDto dto = new VoteJoinRequestDto {Id = joinRequestId, accept = _accept};
            //CancelJoinRequestDto dto = new CancelJoinRequestDto {Id = joinRequestId, RequestingStudentId = requestingStudentId, RequestedGroupId = requestedGroupId};
            ServiceResponse<JoinRequestInfoDto> response = await _joinRequestService.Vote(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("{joinRequestId}")]
        public async Task<IActionResult> Get( int joinRequestId )
        {
            ServiceResponse<GetJoinRequestDto> response = await _joinRequestService.GetJoinRequestById(joinRequestId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}