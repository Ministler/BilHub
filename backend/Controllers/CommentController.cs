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
        [Route("{submissionId}/{commentId}")]
        public async Task<IActionResult> Submit(IFormFile file, int submissionId, int commentId)
        {
            AddCommentFileDto dto = new AddCommentFileDto { CommentFile = file, SubmissionId = submissionId, CommentId = commentId };
            ServiceResponse<string> response = await _commentService.SubmitCommentFile(dto);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);

        }
    }

}