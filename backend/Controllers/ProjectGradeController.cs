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
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (types.ContainsKey(ext))
                return types[ext];
            return null;
        }

         private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".zip", "application/zip"},
                {".csv", "text/csv"}
            };
        }

        
        [HttpPost]
        [Route("file/{gradedProjectGroupId}/{maxGrade}/{grade}")]
        public async Task<IActionResult> Add(IFormFile file, int gradedProjectGroupId, decimal maxGrade, decimal grade, string comment )
        {            
            AddProjectGradeDto dto = new AddProjectGradeDto {
                                                        File = file,
                                                        GradedProjectGroupID = gradedProjectGroupId, 
                                                        LastEdited = DateTime.Now,
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
                                        LastEdited = DateTime.Now,
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
        [Route("GetById/{projectGradeId}")]
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
        [Route("DownloadById/{gradeId}")]
        public async Task<IActionResult> DownloadById( int gradeId )
        {
            GetProjectGradeByIdDto dto = new GetProjectGradeByIdDto { Id = gradeId };
            ServiceResponse<string> response = await _projectGradeService.DownloadProjectGradeById( dto );
            if (response.Success)
            {   
                string path = response.Data;
                string type = GetContentType(path);
                if (type == null)
                    return BadRequest("this file type is not supported");
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, type, Path.GetFileName(path));
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("GetByUsersAndGroup/{gradedProjectGroupId}/{gradingUserId}")]
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
        [Route("DownloadByUserAndGroup/{projectGroupId}/{gradingUserId}")]
        public async Task<IActionResult> DownloadByGroup( int projectGroupId, int gradingUserId )
        {
            GetProjectGradeDto dto = new GetProjectGradeDto { GradedProjectGroupID = projectGroupId, GradingUserId = gradingUserId };
            ServiceResponse<string> response = await _projectGradeService.DownloadProjectGradeByGroupAndUser( dto );
            if (response.Success)
            {
                string path = response.Data;
                string type = GetContentType(path);
                if (type == null)
                    return BadRequest("this file type is not supported");
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, type, Path.GetFileName(path));
            }
            return NotFound(response);
        }
    }
}