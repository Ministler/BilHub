using System.Threading.Tasks;
using backend.Models;
using backend.Services.SubmissionServices;
using backend.Dtos.Submission;
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
using backend.Dtos.Comment;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        private IWebHostEnvironment _hostingEnvironment;
        public SubmissionController(ISubmissionService submissionService, IWebHostEnvironment hostingEnvironment)
        {
            _submissionService = submissionService;

            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        [Route("File/Ungraded/{courseId}")]
        public async Task<ActionResult> GetUngradedSubmissions(int courseId)
        {
            GetUngradedSubmissionFilesDto dto = new GetUngradedSubmissionFilesDto { CourseId = courseId };
            ServiceResponse<IEnumerable<string>> response = await _submissionService.DownloadNotGradedSubmissions(dto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            List<string> files = response.Data.ToList();

            var webRoot = _hostingEnvironment.ContentRootPath;
            var fileName = "Submissions.zip";
            var tempOutput = webRoot + "/StaticFiles/Submissions/" + fileName;
            using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutput)))
            {
                zipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                for (int i = 0; i < files.Count; i++)
                {
                    ZipEntry zipEntry = new ZipEntry(Path.GetFileName(files[i]));
                    zipEntry.DateTime = DateTime.Now;
                    zipEntry.IsUnicodeText = true;
                    zipOutputStream.PutNextEntry(zipEntry);
                    using (FileStream fileStream = System.IO.File.OpenRead(files[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                            zipOutputStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }

                zipOutputStream.Finish();
                zipOutputStream.Flush();
                zipOutputStream.Close();

                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutput);
                if (System.IO.File.Exists(tempOutput))
                {
                    System.IO.File.Delete(tempOutput);
                }
                if (finalResult == null || !finalResult.Any())
                {
                    return BadRequest(response);
                }
                return File(finalResult, "application/zip", fileName);
            }
        }
        [HttpGet]
        [Route("File/{courseId}/{sectionId?}/{assignmentId?}")]
        public async Task<ActionResult> GetAllSubmissions(int courseId, int sectionId = -1, int assignmentId = -1)
        {
            GetSubmissionsFileDto dto = new GetSubmissionsFileDto { CourseId = courseId, SectionId = sectionId, AssignmentId = assignmentId };
            ServiceResponse<string> response = await _submissionService.DownloadAllSubmissions(dto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            List<string> files = new List<string>();
            Stack<string> stack = new Stack<string>();
            string s = response.Data;
            stack.Push(s);
            while (stack.Count != 0)
            {
                s = stack.Peek();
                stack.Pop();
                foreach (string f in Directory.GetFiles(s))
                    files.Add(f);

                foreach (string d in Directory.GetDirectories(s))
                    stack.Push(d);
            }

            var webRoot = _hostingEnvironment.ContentRootPath;
            var fileName = "Submissions.zip";
            var tempOutput = webRoot + "/StaticFiles/Submissions/" + fileName;
            using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutput)))
            {
                zipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                for (int i = 0; i < files.Count; i++)
                {
                    ZipEntry zipEntry = new ZipEntry(Path.GetFileName(files[i]));
                    zipEntry.DateTime = DateTime.Now;
                    zipEntry.IsUnicodeText = true;
                    zipOutputStream.PutNextEntry(zipEntry);
                    using (FileStream fileStream = System.IO.File.OpenRead(files[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                            zipOutputStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }

                zipOutputStream.Finish();
                zipOutputStream.Flush();
                zipOutputStream.Close();

                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutput);
                if (System.IO.File.Exists(tempOutput))
                {
                    System.IO.File.Delete(tempOutput);
                }
                if (finalResult == null || !finalResult.Any())
                {
                    return BadRequest(response);
                }
                return File(finalResult, "application/zip", fileName);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAssignment([FromForm] AddSubmissionwithAttachmentDto addwithAttachment)
        {
            ServiceResponse<GetSubmissionDto> response = await _submissionService.AddSubmission(addwithAttachment.addSubmissionDto);
            if (response.Success)
            {
                if (addwithAttachment.file != null)
                    await _submissionService.SubmitAssignment(new AddSubmissionFileDto { File = addwithAttachment.file, SubmissionId = response.Data.Id });
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete]
        [Route("{submissionId}")]
        public async Task<IActionResult> DeleteSubmission(int submissionId)
        {
            ServiceResponse<string> response = await _submissionService.Delete(submissionId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("{submissionId}")]
        public async Task<IActionResult> GetSubmission(int submissionId)
        {
            ServiceResponse<GetSubmissionDto> response = await _submissionService.GetSubmission(submissionId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("InstructorComments/{submissionId}")]
        public async Task<IActionResult> GetInstructorComments(int submissionId)
        {
            ServiceResponse<List<GetCommentDto>> response = await _submissionService.GetInstructorComments(submissionId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("TAComments/{submissionId}")]
        public async Task<IActionResult> GetTAComments(int submissionId)
        {
            ServiceResponse<List<GetCommentDto>> response = await _submissionService.GetTaComments(submissionId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("StudentComments/{submissionId}")]
        public async Task<IActionResult> GetStudentComments(int submissionId)
        {
            ServiceResponse<List<GetCommentDto>> response = await _submissionService.GetStudentComments(submissionId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("Grade/{submissionId}/{graderId}")]
        public async Task<IActionResult> GetGradeById(int submissionId, int graderId)
        {
            ServiceResponse<decimal> response = await _submissionService.GetGradeWithGraderId(submissionId, graderId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateAssignment([FromForm] UpdateSubmissionWithAttachment updateSubmissionWithAttachment)
        {
            ServiceResponse<GetSubmissionDto> response = await _submissionService.UpdateSubmission(updateSubmissionWithAttachment.updateSubmissionDto);
            if (response.Success)
            {
                if (updateSubmissionWithAttachment.file != null)
                    await _submissionService.SubmitAssignment(new AddSubmissionFileDto { File = updateSubmissionWithAttachment.file, SubmissionId = response.Data.Id });
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost]
        [Route("File/{submissionId}")]
        public async Task<IActionResult> Submit(IFormFile file, int submissionId)
        {
            AddSubmissionFileDto dto = new AddSubmissionFileDto { File = file, SubmissionId = submissionId };
            ServiceResponse<string> response = await _submissionService.SubmitAssignment(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);

        }

        [HttpGet]
        [Route("File/{submissionId}")]
        public async Task<IActionResult> GetSubmissionFile(int submissionId)
        {
            GetSubmissionFileDto dto = new GetSubmissionFileDto { SubmissionId = submissionId };
            ServiceResponse<string> response = await _submissionService.DownloadSubmission(dto);
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

        [HttpDelete]
        [Route("File/{submissionId}")]
        public async Task<IActionResult> DeleteSubmissionFile(int submissionId)
        {
            DeleteSubmissionFileDto dto = new DeleteSubmissionFileDto { SubmissionId = submissionId };
            ServiceResponse<string> response = await _submissionService.DeleteSubmissionFile(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);

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
    }
}