using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RensourceDomain.Models.Response;
using RensourcePersistence.AppDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces.Repos
{
    public class RolesRepo : IRolesRepo
    {
        private readonly ILogger<RolesRepo> _logger;
        private readonly ApplicationDBContext _context;
        public RolesRepo(ILogger<RolesRepo> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GenericResponse> GetAllRolesAsync()
        {
            try
            {
                var allRoles = (from role in _context.Roles select role).ToList();
                if (allRoles.Count() > 0)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allRoles };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetRoleAsync(int Id)
        {
            try
            {
                var role = (from rl in _context.Roles where rl.Id == Id select rl).FirstOrDefault();
                if (role != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = role };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
    }
}
