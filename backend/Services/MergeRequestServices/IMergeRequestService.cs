using System.Collections.Generic;
using backend.Models;
using System.Threading.Tasks;
using backend.Dtos.MergeRequest;

namespace backend.Services.MergeRequestServices
{
    public interface IMergeRequestService
    {
        Task<ServiceResponse<AddMergeRequestDto>> SendMergeRequest(AddMergeRequestDto newMergeRequestDto);
        
        Task<ServiceResponse<string>> CancelMergeRequest(CancelMergeRequestDto mergeRequestDto); 
        
        Task<ServiceResponse<MergeRequestInfoDto>> Vote(VoteMergeRequestDto mergeRequestInfoDto);
        
        Task<ServiceResponse<string>> DeleteAllMergeRequests(DeleteAllMergeRequestsDto deleteAllMergeRequestsDto);
        Task<ServiceResponse<GetMergeRequestDto>> GetMergeRequestById(int Id);
        Task<ServiceResponse<List<GetMergeRequestDto>>> GetOutgoingMergeRequestsOfUser ();
        Task<ServiceResponse<List<GetMergeRequestDto>>> GetIncomingMergeRequestsOfUser ();
        Task<ServiceResponse<string>> GetVoteOfUser( int mergeRequestId );
    }
}