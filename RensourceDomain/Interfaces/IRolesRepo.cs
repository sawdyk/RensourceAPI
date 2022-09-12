using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IRolesRepo
    {
        Task<GenericResponse> GetAllRolesAsync();
        Task<GenericResponse> GetRoleAsync(int Id);
    }
}
