using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IFAQsRepo
    {
        Task<GenericResponse> CreateFAQAsync(FAQRequest fAQReq);
        Task<GenericResponse> UpdateFAQAsync(UpdateFAQRequest fAQReq);
        Task<GenericResponse> GetAllFAQAsync();
        Task<GenericResponse> GetFAQAsync(Guid Id);
        Task<GenericResponse> DeleteFAQAsync(Guid Id);
    }
}
