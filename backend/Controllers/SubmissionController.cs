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
        [Route("{courseId}/{sectionId}/{assignmentId}")]
        //istedigim gibi calismiyor
        public async Task<ActionResult> GetAllSubmissions(int courseId, int sectionId, int assignmentId)
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
            var tempOutput = webRoot + "/Submissions/" + fileName;
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
        [Route("{assignmentId}/{groupId}")]
        public async Task<IActionResult> Submit(IFormFile file, int assignmentId, int groupId)
        {
            AddSubmissionFileDto dto = new AddSubmissionFileDto { File = file, AssignmentId = assignmentId, ProjectGroupId = groupId };
            ServiceResponse<string> response = await _submissionService.SubmitAssignment(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);

        }

        [HttpGet]
        [Route("{assignmentId}/{groupId}")]
        public async Task<IActionResult> GetSubmission(int assignmentId, int groupId)
        {
            GetSubmissionFileDto dto = new GetSubmissionFileDto { AssignmentId = assignmentId, ProjectGroupId = groupId };
            ServiceResponse<string> response = await _submissionService.DownloadSubmission(dto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            string path = response.Data;
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));

        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
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
                {".csv", "text/csv"}
            };
        }
    }
}