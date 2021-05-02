using System.Threading.Tasks;
using backend.Models;
using backend.Services.PeerGradeServices;
using backend.Dtos.PeerGrade;
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
    public class PeerGradeController : ControllerBase
    {
        private readonly IPeerGradeService _peerGradeService;

        private IWebHostEnvironment _hostingEnvironment;
        public PeerGradeController(IPeerGradeService peerGradeService, IWebHostEnvironment hostingEnvironment)
        {
            _peerGradeService = peerGradeService;
            _hostingEnvironment = hostingEnvironment;
        }

        
        [HttpPost]
        [Route("{projectGroupId}/{revieweeId}/{maxGrade}/{grade}")]
        public async Task<IActionResult> Add(int projectGroupId, int revieweeId, decimal maxGrade, decimal grade, string comment )
        {            
            AddPeerGradeDto dto = new AddPeerGradeDto {
                                                        ProjectGroupId = projectGroupId, 
                                                        RevieweeId = revieweeId,
                                                        CreatedAt = DateTime.Now,
                                                        MaxGrade = maxGrade,
                                                        Grade = grade,
                                                        Comment = comment
                                                    };
            ServiceResponse<AddPeerGradeDto> response = await _peerGradeService.AddPeerGrade(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete]
        [Route("{peerGradeId}")]
        public async Task<IActionResult> Delete(int peerGradeId )
        {     
            DeletePeerGradeDto dto = new DeletePeerGradeDto {Id = peerGradeId};
            ServiceResponse<string> response = await _peerGradeService.DeletePeerGrade(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut]
        [Route("{peerGradeId}/{maxGrade}/{grade}")]
        public async Task<IActionResult> Edit(int peerGradeId, decimal maxGrade, decimal grade, string comment )
        {     
            EditPeerGradeDto dto = new EditPeerGradeDto {
                                        MaxGrade = maxGrade,
                                        Grade = grade,
                                        Comment = comment,
                                        CreatedAt = DateTime.Now,
                                        Id = peerGradeId
                                    };
            
            ServiceResponse<PeerGradeInfoDto> response = await _peerGradeService.EditPeerGrade(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("{peerGradeId}")]
        public async Task<IActionResult> GetById( int peerGradeId )
        {
            GetPeerGradeByIdDto dto = new GetPeerGradeByIdDto { Id = peerGradeId };
            ServiceResponse<PeerGradeInfoDto> response = await _peerGradeService.GetPeerGradeById( dto );
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("{projectGroupId}/{reviewerId}/{revieweeId}")]
        public async Task<IActionResult> GetByUsersAndGroup( int projectGroupId, int reviewerId, int revieweeId )
        {
            GetPeerGradeDto dto = new GetPeerGradeDto { ProjectGroupId = projectGroupId, ReviewerId = reviewerId, RevieweeId = revieweeId };
            ServiceResponse<PeerGradeInfoDto> response = await _peerGradeService.GetPeerGradeByUsersAndGroup( dto );
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}