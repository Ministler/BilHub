using System.Collections.Generic;
using backend.Models;
using System.Threading.Tasks;
using backend.Dtos.ProjectGrade;

namespace backend.Services.ProjectGradeServices
{
    public interface IProjectGradeService
    {
        Task<ServiceResponse<AddProjectGradeDto>> AddProjectGrade(AddProjectGradeDto addProjectGradeDto);
        Task<ServiceResponse<ProjectGradeInfoDto>> EditProjectGrade(EditProjectGradeDto editProjectGradeDto);
        Task<ServiceResponse<string>> DeleteProjectGrade(DeleteProjectGradeDto deleteProjectGradeDto);
        Task<ServiceResponse<ProjectGradeInfoDto>> GetProjectGradeById(GetProjectGradeByIdDto getProjectGradeDto);
        Task<ServiceResponse<string>> DownloadProjectGradeById(GetProjectGradeByIdDto dto);
        Task<ServiceResponse<ProjectGradeInfoDto>> GetProjectGradeByUserAndGroup(GetProjectGradeDto getProjectGradeDto);
        Task<ServiceResponse<string>> DownloadProjectGradeByGroupAndUser(GetProjectGradeDto dto);
        Task<ServiceResponse<string>> DeleteWithForce(int projectGradeId);
        Task<ServiceResponse<List<ProjectGradeInfoDto>>> GetProjectGradesGivenTo(int projectGroupId);
    }
}