using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IAdminRepo
    {
        Task<GenericResponse> CreatAdminAsync(UserCreationRequest userReq);
        Task<GenericResponse> GetAllAdminAsync();
        Task<GenericResponse> GetAdminAsync(Guid Id);
        Task<GenericResponse> UpdateAdminAsync(UserUpdateRequest userReq);
        Task<GenericResponse> DeleteAdminAsync(Guid Id);
    }
}
