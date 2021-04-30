using System.Threading.Tasks;
using backend.Models;
using backend.Services.ProjectGradeServices;
using backend.Dtos.ProjectGrade;
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
    public class ProjectGradeController : ControllerBase
    {
        private readonly IProjectGradeService _projectGradeService;

        private IWebHostEnvironment _hostingEnvironment;
        public ProjectGradeController(IProjectGradeService projectGradeService, IWebHostEnvironment hostingEnvironment)
        {
            _projectGradeService = projectGradeService;
            _hostingEnvironment = hostingEnvironment;
        }

        
        [HttpPost]
        [Route("file/{gradedProjectGroupId}/{maxGrade}/{grade}")]
        public async Task<IActionResult> Add(IFormFile file, int gradedProjectGroupId, decimal maxGrade, decimal grade, string comment )
        {            
            AddProjectGradeDto dto = new AddProjectGradeDto {
                                                        File = file,
                                                        GradedProjectGroupID = gradedProjectGroupId, 
                                                        CreatedAt = DateTime.Now,
                                                        MaxGrade = maxGrade,
                                                        Grade = grade,
                                                        Comment = comment
                                                    };
            ServiceResponse<AddProjectGradeDto> response = await _projectGradeService.AddProjectGrade(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete]
        [Route("{projectGradeId}")]
        public async Task<IActionResult> Delete(int projectGradeId )
        {     
            DeleteProjectGradeDto dto = new DeleteProjectGradeDto {Id = projectGradeId};
            ServiceResponse<string> response = await _projectGradeService.DeleteProjectGrade(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut]
        [Route("file/{projectGradeId}/{maxGrade}/{grade}")]
        public async Task<IActionResult> Edit(IFormFile file, int projectGradeId, decimal maxGrade, decimal grade, string comment )
        {     
            EditProjectGradeDto dto = new EditProjectGradeDto {
                                        File = file,
                                        Id = projectGradeId, 
                                        CreatedAt = DateTime.Now,
                                        MaxGrade = maxGrade,
                                        Grade = grade,
                                        Comment = comment
                                    };
            
            ServiceResponse<ProjectGradeInfoDto> response = await _projectGradeService.EditProjectGrade(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("{projectGradeId}")]
        public async Task<IActionResult> GetById( int projectGradeId )
        {
            GetProjectGradeByIdDto dto = new GetProjectGradeByIdDto { Id = projectGradeId };
            ServiceResponse<ProjectGradeInfoDto> response = await _projectGradeService.GetProjectGradeById( dto );
            if (response.Success)
            {   
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("{projectGradeId}")]
        public async Task<IActionResult> DownloadById( int projectGradeId )
        {
            GetProjectGradeByIdDto dto = new GetProjectGradeByIdDto { Id = projectGradeId };
            ServiceResponse<ProjectGradeFileDownloadDto> response = await _projectGradeService.DownloadProjectGradeById( dto );
            if (response.Success)
            {   
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("{gradedProjectGroupId}/{gradingUserId}")]
        public async Task<IActionResult> GetByUsersAndGroup( int gradedProjectGroupId, int gradingUserId )
        {
            GetProjectGradeDto dto = new GetProjectGradeDto { GradedProjectGroupID = gradedProjectGroupId, GradingUserId = gradingUserId };
            ServiceResponse<ProjectGradeInfoDto> response = await _projectGradeService.GetProjectGradeByUserAndGroup( dto );
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        

        [HttpGet]
        [Route("{gradedProjectGroupId}/{gradingUserId}")]
        public async Task<IActionResult> DownloadByGroup( int gradedProjectGroupId, int gradingUserId )
        {
            GetProjectGradeDto dto = new GetProjectGradeDto { GradedProjectGroupID = gradedProjectGroupId, GradingUserId = gradingUserId };
            ServiceResponse<ProjectGradeFileDownloadDto> response = await _projectGradeService.DownloadProjectGradeByGroupAndUser( dto );
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}