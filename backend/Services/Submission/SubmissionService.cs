using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BilHub.Data;
using BilHub.Dtos.Submission;
using BilHub.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BilHub.Services.Submission
{
    public class SubmissionService : ISubmissionService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        public SubmissionService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<string>> SubmitAssignment(IFormFile file)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, "folder");
            Directory.CreateDirectory(target);
            if (file.Length <= 0) response.Success = false;
            else
            {
                var filePath = Path.Combine(target, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                response.Data = target;
                response.Message = "file succesfully saved.";
            }

            return response;
        }


    }
}