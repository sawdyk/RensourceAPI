using RensourceDomain.Helpers.Enums;
using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IPressReleaseRepo
    {
        Task<GenericResponse> CreatePressReleaseAsync(PressReleaseRequest pressReq);
        Task<GenericResponse> UpdatePressReleaseAsync(UpdatePressReleaseRequest pressReq);
        Task<PaginationResponse> GetAllPressReleaseAsync(int pageNumber, int pageSize);
        Task<PaginationResponse> GetAllPressReleaseByOrderingAsync(int pageNumber, int pageSize, OrderFilter order);
        Task<GenericResponse> GetPressReleaseAsync(Guid Id);
        Task<GenericResponse> GetPressReleaseByTitleAsync(string? title);
        Task<GenericResponse> DeletePressReleaseAsync(Guid Id);
    }
}
