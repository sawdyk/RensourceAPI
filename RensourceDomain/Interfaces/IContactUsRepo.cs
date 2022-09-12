using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IContactUsRepo
    {
        Task<GenericResponse> CreateMessageAsync(ContactUsRequest contReq);
        Task<GenericResponse> GetAllMessageAsync();
        Task<GenericResponse> GetMessageAsync(Guid Id);
        Task<GenericResponse> DeleteMessageAsync(Guid Id);
    }
}
