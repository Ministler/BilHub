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
using backend.Services.CommentServices;
using backend.Dtos.Comment;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        private IWebHostEnvironment _hostingEnvironment;
        public CommentController(ICommentService commentService, IWebHostEnvironment hostingEnvironment)
        {
            _commentService = commentService;

            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("File/{commentId}")]
        public async Task<IActionResult> Submit(IFormFile file, int commentId)
        {
            AddCommentFileDto dto = new AddCommentFileDto { CommentFile = file, CommentId = commentId };
            ServiceResponse<string> response = await _commentService.SubmitCommentFile(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete]
        [Route("File/{commentId}")]
        public async Task<IActionResult> DeleteFile(int commentId)
        {
            DeleteCommentFileDto dto = new DeleteCommentFileDto { CommentId = commentId };
            ServiceResponse<string> response = await _commentService.DeleteFile(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete]
        [Route("{commentId}")]
        public async Task<IActionResult> Delete(int commentId)
        {
            ServiceResponse<string> response = await _commentService.Delete(commentId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateCommentWithAttachmentDto updateCommentWithAttachmentDto)
        {
            ServiceResponse<GetCommentDto> response = await _commentService.Update(updateCommentWithAttachmentDto.AddCommentDto);
            if (response.Success)
            {
                if (updateCommentWithAttachmentDto.File != null)
                    await _commentService.SubmitCommentFile(new AddCommentFileDto
                    {
                        CommentFile = updateCommentWithAttachmentDto.File,
                        CommentId = updateCommentWithAttachmentDto.CommentId
                    });
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("File/{commentId}")]
        public async Task<IActionResult> GetCommentFile(int commentId)
        {
            GetCommentFileDto dto = new GetCommentFileDto { CommentId = commentId };
            ServiceResponse<string> response = await _commentService.DownloadCommentFile(dto);
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
        [HttpGet]
        [Route("File/Zip/{submissionId}")]
        public async Task<IActionResult> GetAllCommentFiles(int submissionId)
        {
            GetAllCommentFilesDto dto = new GetAllCommentFilesDto { SubmissionId = submissionId };
            ServiceResponse<string> response = await _commentService.DownloadAllCommentFiles(dto);
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
            var fileName = "Feedbacks.zip";
            var tempOutput = webRoot + "/StaticFiles/Feedbacks/" + fileName;
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

        public async Task<IActionResult> Comment([FromForm] AddCommentwithAttachmentDto acwadto)
        {
            ServiceResponse<GetCommentDto> response = await _commentService.Add(acwadto.AddCommentDto);
            if (response.Success)
            {
                if (acwadto.File != null)
                    await _commentService.SubmitCommentFile(new AddCommentFileDto { CommentFile = acwadto.File, CommentId = response.Data.CommentId });
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}