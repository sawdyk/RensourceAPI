using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IExecutiveRoleRepo
    {
        Task<GenericResponse> CreateExecutiveRoleAsync(ExecutiveRoleRequest execReq);
        Task<GenericResponse> UpdateExecutiveRoleAsync(UpdateExecutiveRoleRequest execReq);
        Task<GenericResponse> GetAllExecutiveRolesAsync();
        Task<GenericResponse> GetExecutiveRoleAsync(Guid Id);
        Task<GenericResponse> DeleteExecutiveRoleAsync(Guid Id);
    }
}
