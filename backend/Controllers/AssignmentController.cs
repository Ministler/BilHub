using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using backend.Services.AssignmentServices;
using backend.Dtos.Assignment;
using backend.Dtos.ProjectGroup;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        private IWebHostEnvironment _hostingEnvironment;
        public AssignmentController(IAssignmentService assignmentService, IWebHostEnvironment hostingEnvironment)
        {
            _assignmentService = assignmentService;

            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("File/{assignmentId}")]
        public async Task<IActionResult> SubmitFile(IFormFile file, int assignmentId)
        {
            AddAssignmentFileDto dto = new AddAssignmentFileDto { File = file, AssignmentId = assignmentId };
            ServiceResponse<string> response = await _assignmentService.SubmitAssignmentFile(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAssignment([FromForm] AddwithAttachmentDto addwithAttachment)
        {
            ServiceResponse<GetAssignmentDto> response = await _assignmentService.SubmitAssignment(addwithAttachment.addAssignmentDto);
            if (response.Success)
            {
                if (addwithAttachment.file != null)
                    await _assignmentService.SubmitAssignmentFile(new AddAssignmentFileDto { File = addwithAttachment.file, AssignmentId = response.Data.Id });
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAssignment([FromForm] UpdateAssignmentWithAttachment updateAssignment)
        {
            ServiceResponse<GetAssignmentDto> response = await _assignmentService.UpdateAssignment(updateAssignment.updateAssignmentDto);
            if (response.Success)
            {
                if (updateAssignment.file != null)
                    await _assignmentService.SubmitAssignmentFile(new AddAssignmentFileDto { File = updateAssignment.file, AssignmentId = response.Data.Id });
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete]
        [Route("{assignmentId}")]
        public async Task<IActionResult> DeleteAssignment(int assignmentId)
        {
            ServiceResponse<string> response = await _assignmentService.DeleteAssignment(new DeleteAssignmentDto { AssignmentId = assignmentId });
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("{assignmentId}")]
        public async Task<IActionResult> GetAssignment(int assignmentId)
        {
            ServiceResponse<GetAssignmentDto> response = await _assignmentService.GetAssignment(assignmentId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpDelete]
        [Route("File/{assignmentId}")]
        public async Task<IActionResult> DeleteFile(int assignmentId)
        {
            DeleteAssignmentFileDto dto = new DeleteAssignmentFileDto { AssignmentId = assignmentId };
            ServiceResponse<string> response = await _assignmentService.DeleteFile(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("File/{assignmentId}")]
        public async Task<IActionResult> GetAssignmentFile(int assignmentId)
        {
            GetAssignmentFileDto dto = new GetAssignmentFileDto { AssignmentId = assignmentId };
            ServiceResponse<string> response = await _assignmentService.DownloadAssignmentFile(dto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
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
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (types.ContainsKey(ext))
                return types[ext];
            return null;
        }

        [HttpGet]
        [Route("Statistics/{assignmentId}")]
        public async Task<IActionResult> GetOzgur(int assignmentId)
        {
            ServiceResponse<GetOzgurDto> response = await _assignmentService.GetOzgur(assignmentId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
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
    }
}