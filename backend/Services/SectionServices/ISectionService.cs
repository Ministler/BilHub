using System.Threading.Tasks;
using backend.Dtos.Section;
using backend.Models;

namespace backend.Services.SectionServices
{
    public interface ISectionService
    {
        Task<ServiceResponse<GetSectionDto>> AddStudentToSection ( int userId, int sectionId );
        Task<ServiceResponse<GetSectionDto>> RemoveStudentFromSection ( int userId, int sectionId );
        Task<ServiceResponse<GetSectionDto>> GetSectionInfo ( int sectionId );
    }
}