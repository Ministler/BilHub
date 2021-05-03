using System.Threading.Tasks;
using backend.Models;
using backend.Services.MergeRequestServices;
using backend.Dtos.MergeRequest;
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
    public class MergeRequestController : ControllerBase
    {
        private readonly IMergeRequestService _mergeRequestService;

        private IWebHostEnvironment _hostingEnvironment;
        public MergeRequestController(IMergeRequestService mergeRequestService, IWebHostEnvironment hostingEnvironment)
        {
            _mergeRequestService = mergeRequestService;
            _hostingEnvironment = hostingEnvironment;
        }

        
        [HttpPost]
        [Route("{receiverGroupId}")]
        public async Task<IActionResult> Send(int receiverGroupId, string description )
        {            
            AddMergeRequestDto dto = new AddMergeRequestDto {ReceiverGroupId = receiverGroupId, CreatedAt = DateTime.Now, Description = description};
            ServiceResponse<AddMergeRequestDto> response = await _mergeRequestService.SendMergeRequest(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete]
        [Route("{mergeRequestId}")]
        public async Task<IActionResult> Cancel(int mergeRequestId )
        {     
            CancelMergeRequestDto dto = new CancelMergeRequestDto {Id = mergeRequestId};
            ServiceResponse<string> response = await _mergeRequestService.CancelMergeRequest(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        
        [HttpPut]
        [Route("{mergeRequestId}/{_accept}")]
        public async Task<IActionResult> Vote(int mergeRequestId, bool _accept )
        {     
            VoteMergeRequestDto dto = new VoteMergeRequestDto {Id = mergeRequestId, accept = _accept};
            ServiceResponse<MergeRequestInfoDto> response = await _mergeRequestService.Vote(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("{mergeRequestId}")]
        public async Task<IActionResult> Get( int mergeRequestId )
        {
            ServiceResponse<GetMergeRequestDto> response = await _mergeRequestService.GetMergeRequestById(mergeRequestId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("OutgoingMergeRequestsOfUser")]
        public async Task<IActionResult> GetOutgoingMergeRequestsOfUser ( )
        {
            ServiceResponse<List<GetMergeRequestDto>> response = await _mergeRequestService.GetOutgoingMergeRequestsOfUser();
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("IncomingMergeRequestsOfUser")]
        public async Task<IActionResult> GetIncomingMergeRequestsOfUser ( )
        {
            ServiceResponse<List<GetMergeRequestDto>> response = await _mergeRequestService.GetIncomingMergeRequestsOfUser();
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("GetVoteOfUser/{mergeRequestId}")]
        public async Task<IActionResult> GetVote ( int mergeRequestId )
        {
            ServiceResponse<string> response = await _mergeRequestService.GetVoteOfUser(mergeRequestId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}