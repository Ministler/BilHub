using System.Collections.Generic;
using backend.Models;
using System.Threading.Tasks;
using backend.Dtos.PeerGradeAssignment;

namespace backend.Services.PeerGradeAssignmentServices
{
    public interface IPeerGradeAssignmentService
    {
        Task<ServiceResponse<AddPeerGradeAssignmentDto>> AddPeerGradeAssignment(AddPeerGradeAssignmentDto dto);
        Task<ServiceResponse<PeerGradeAssignmentInfoDto>> EditPeerGradeAssignment(EditPeerGradeAssignmentDto dto);
        Task<ServiceResponse<PeerGradeAssignmentInfoDto>> GetPeerGradeAssignmentByCourseId(int courseId);         
        Task<ServiceResponse<string>> DeletePeerGradeAssignment(int Id);
    }
}