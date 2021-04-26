using System.Threading.Tasks;
using backend.Dtos.ProjectGroup;
using backend.Models;

namespace backend.Services.ProjectGroupServices
{
    public interface IProjectGroupService
    {
        Task<ServiceResponse<GetProjectGroupDto>> GetProjectGroupById(int id);
        
    }
}