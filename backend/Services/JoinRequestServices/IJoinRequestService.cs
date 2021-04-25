using System.Collections.Generic;
using BilHub.Models;
using System.Threading.Tasks;
using BilHub.Dtos.JoinRequest;

namespace backend.Services.JoinRequestServices
{
    public interface IJoinRequestService
    {
        Task<ServiceResponse<string>> SendJoinRequest(AddJoinRequestDto newJoinRequest);

    }
}