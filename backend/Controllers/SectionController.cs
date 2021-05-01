using System.Threading.Tasks;
using backend.Services.SectionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _sectionService;
        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;

        }

        [HttpPost("AddStudentToSection")]
        public async Task<ActionResult> AddStudentToSection ( int userId, int sectionId )
        {
            return Ok ( await _sectionService.AddStudentToSection ( userId, sectionId ) );
        }

        [HttpDelete("RemoveStudentFromSection")]
        public async Task<ActionResult> RemoveStudentFromSection ( int userId, int sectionId )
        {
            return Ok ( await _sectionService.RemoveStudentFromSection ( userId, sectionId ) );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSectionInfo ( int id )
        {
            return Ok ( await _sectionService.GetSectionInfo ( id ) );
        }

    }
}