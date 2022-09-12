using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface ICompanyInfoRepo
    {
        Task<GenericResponse> CreateUpdateCompanyInfoAsync(CompanyInfoRequest comReq);
        Task<GenericResponse> GetCompanyInfoAsync();
    }
}
