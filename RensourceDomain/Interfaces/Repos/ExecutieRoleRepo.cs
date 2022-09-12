using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using RensourcePersistence.AppDBContext;
using RensourcePersistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces.Repos
{
    public class ExecutieRoleRepo : IExecutiveRoleRepo
    {
        private readonly ILogger<ExecutieRoleRepo> _logger;
        private readonly ApplicationDBContext _context;
        public ExecutieRoleRepo(ILogger<ExecutieRoleRepo> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GenericResponse> CreateExecutiveRoleAsync(ExecutiveRoleRequest execReq)
        {
            try
            {
                var execRole = _context.ExecutiveRoles.FirstOrDefault(x => x.ExecutiveRoleName == execReq.ExecutiveRoleName);
                if (execRole is null)
                {
                    var newExce = new ExecutiveRoles
                    {
                        ExecutiveRoleName = execReq.ExecutiveRoleName,
                        CreatedBy = execReq.CreatedBy,
                        DateCreated = DateTime.Now
                    };

                    await _context.ExecutiveRoles.AddAsync(newExce);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Executive Role Created Successfully");
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Executive Role Created Successfully", Data = newExce };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = $"Executive Role with Name {execReq.ExecutiveRoleName} exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }

        }

        public async Task<GenericResponse> DeleteExecutiveRoleAsync(Guid Id)
        {
            try
            {
                var exec = await _context.ExecutiveRoles.FirstOrDefaultAsync(x => x.Id == Id);
                if (exec != null)
                {
                    _context.ExecutiveRoles.Remove(exec);
                    int rsp = await _context.SaveChangesAsync();
                    if (rsp > 0)
                    {
                        _logger.LogInformation($"Executive Role Deleted Successful");
                        return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Executive Role Deleted Successfully" };
                    }
                    else
                        _logger.LogError($"Executive Role Delete Failed");
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Executive Role With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetAllExecutiveRolesAsync()
        {
            try
            {
                var allExecRoles = (from pr in _context.ExecutiveRoles orderby pr.ExecutiveRoleName ascending select pr)
                     .Include(x => x.ExecutiveTeams).AsNoTracking()
                     .ToList();
                if (allExecRoles.Count() > 0)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allExecRoles };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetExecutiveRoleAsync(Guid Id)
        {
            try
            {
                var execRole = (from pr in _context.ExecutiveRoles where pr.Id == Id select pr)
                    .Include(x => x.ExecutiveTeams).AsNoTracking()
                    .FirstOrDefault();
                if (execRole != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = execRole };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> UpdateExecutiveRoleAsync(UpdateExecutiveRoleRequest execReq)
        {
            try
            {
                var exec = await _context.ExecutiveRoles.FirstOrDefaultAsync(x => x.Id == execReq.Id);
                if (exec != null)
                {
                    exec.ExecutiveRoleName = execReq.ExecutiveRoleName;
                    exec.LastUpdatedDate = DateTime.Now;
                    exec.UpdatedBy = execReq.UpdatedBy;

                    int projRp = await _context.SaveChangesAsync();

                    if (projRp > 0)
                        _logger.LogInformation($"Exceutive Role Update Successful");
                    else
                        _logger.LogError($"Exceutive Role Update Failed");

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Exceutive Role Updated Sucessfully" };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Exceutive Role With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
    }
}
