using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IBlogRepo
    {
        Task<GenericResponse> CreateBlogAsync(BlogRequest blogReq);
        Task<GenericResponse> UpdateBlogAsync(UpdateBlogRequest blogReq);
        Task<PaginationResponse> GetAllBlogAsync(int pageNumber, int pageSize);
        Task<GenericResponse> GetBlogAsync(Guid Id);
        Task<GenericResponse> DeleteBlogAsync(Guid Id);
    }
}
