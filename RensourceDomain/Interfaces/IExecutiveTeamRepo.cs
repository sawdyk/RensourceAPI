using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IExecutiveTeamRepo
    {
        Task<GenericResponse> CreateExecutiveTeamAsync(ExecutiveTeamRequest execReq);
        Task<GenericResponse> UpdateExecutiveTeamAsync(UpdateExecutiveTeamRequest execReq);
        Task<GenericResponse> GetAllExecutiveTeamAsync();
        Task<GenericResponse> GetExecutiveTeamAsync(Guid Id);
        Task<GenericResponse> GetExecutiveTeamByCategoryAsync(Guid categoryId);
        Task<GenericResponse> GetExecutiveTeamByRoleAsync(Guid roleId);
        Task<GenericResponse> DeleteExecutiveTeamAsync(Guid Id);
    }
}
