using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IExecutiveCategoryRepo
    {
        Task<GenericResponse> CreateExecutiveCategoryAsync(ExecutiveCategoryRequest execReq);
        Task<GenericResponse> UpdateExecutiveCategoryAsync(UpdateExecutiveCategoryRequest execReq);
        Task<GenericResponse> GetAllExecutiveCategoryAsync();
        Task<GenericResponse> GetExecutiveCategoryAsync(Guid Id);
        Task<GenericResponse> DeleteExecutiveCategoryAsync(Guid Id);
    }
}
