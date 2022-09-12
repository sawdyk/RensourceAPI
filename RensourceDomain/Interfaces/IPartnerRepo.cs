using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IPartnerRepo
    {
        Task<GenericResponse> CreatePartnerAsync(PartnerRequest partnerRequest);
        Task<GenericResponse> UpdatePartnerAsync(UpdatePartnerRequest partnerRequest);
        Task<GenericResponse> GetAllPartnerAsync();
        Task<GenericResponse> GetPartnerAsync(Guid Id);
        Task<GenericResponse> DeletePartnerAsync(Guid Id);
    }
}
