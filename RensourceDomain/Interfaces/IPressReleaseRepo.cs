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
        Task<GenericResponse> GetAllPressReleaseAsync(int pageNumber, int pageSize);
        Task<GenericResponse> GetPressReleaseAsync(Guid Id);
        Task<GenericResponse> DeletePressReleaseAsync(Guid Id);
    }
}
