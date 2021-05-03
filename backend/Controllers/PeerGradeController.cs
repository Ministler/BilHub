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
        [Route("{projectGroupId}/{revieweeId}/{grade}")]
        public async Task<IActionResult> Add(int projectGroupId, int revieweeId, int grade, string comment )
        {            
            AddPeerGradeDto dto = new AddPeerGradeDto {
                                                        ProjectGroupId = projectGroupId, 
                                                        RevieweeId = revieweeId,
                                                        LastEdited = DateTime.Now,
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
            DeletePeerGradeDto dto = new DeletePeerGradeDto {Id = peerGradeId, LastEdited = DateTime.Now};
            ServiceResponse<string> response = await _peerGradeService.DeletePeerGrade(dto);
            if (response.Success)
            {
                return Ok(response);
            } 
            return NotFound(response);
        }

        [HttpPut]
        [Route("{peerGradeId}/{grade}")]
        public async Task<IActionResult> Edit(int peerGradeId, int grade, string comment )
        {     
            EditPeerGradeDto dto = new EditPeerGradeDto {
                                        Grade = grade,
                                        LastEdited = DateTime.Now,
                                        Comment = comment,
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

        [HttpGet]
        [Route("To/{projectGroupId}/{revieweeId}")]
        public async Task<IActionResult> GetPeerGradesGivenTo( int projectGroupId, int revieweeId )
        {
            GetPeerGradesGivenToDto dto = new GetPeerGradesGivenToDto { ProjectGroupId = projectGroupId, RevieweeId = revieweeId };
            ServiceResponse<List<PeerGradeInfoDto>> response = await _peerGradeService.GetPeerGradesGivenTo( dto );
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("By/{projectGroupId}/{reviewerId}")]
        public async Task<IActionResult> GetPeerGradesGivenBy( int projectGroupId, int reviewerId )
        {
            GetPeerGradesGivenByDto dto = new GetPeerGradesGivenByDto { ProjectGroupId = projectGroupId, ReviewerId = reviewerId };
            ServiceResponse<List<PeerGradeInfoDto>> response = await _peerGradeService.GetPeerGradesGivenBy( dto );
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}