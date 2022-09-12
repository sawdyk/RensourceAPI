using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IProjectsRepo
    {
        Task<GenericResponse> CreateProjectAsync(ProjectRequest projReq);
        Task<GenericResponse> UpdateProjectAsync(UpdateProjectRequest projReq);
        Task<GenericResponse> GetAllProjectsAsync(int pageNumber, int pageSize);
        Task<GenericResponse> GetProjectAsync(Guid Id);
        Task<GenericResponse> DeleteProjectAsync(Guid Id);
    }
}
