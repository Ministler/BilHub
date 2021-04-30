using System.Collections.Generic;
using backend.Models;
using System.Threading.Tasks;
using backend.Dtos.PeerGrade;

namespace backend.Services.PeerGradeServices
{
    public interface IPeerGradeService
    {
        Task<ServiceResponse<AddPeerGradeDto>> AddPeerGrade(AddPeerGradeDto addPeerGradeDto);
        Task<ServiceResponse<PeerGradeInfoDto>> EditPeerGrade(EditPeerGradeDto editPeerGradeDto);
        Task<ServiceResponse<string>> DeletePeerGrade(DeletePeerGradeDto deletePeerGradeDto); 
        Task<ServiceResponse<PeerGradeInfoDto>> GetPeerGradeById(GetPeerGradeByIdDto getPeerGradeDto);
        Task<ServiceResponse<PeerGradeInfoDto>> GetPeerGradeByUsersAndGroup(GetPeerGradeDto getPeerGradeDto);
    }
}