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
    public class ExecutiveCategoryRepo : IExecutiveCategoryRepo
    {
        private readonly ILogger<ExecutiveCategoryRepo> _logger;
        private readonly ApplicationDBContext _context;
        public ExecutiveCategoryRepo(ILogger<ExecutiveCategoryRepo> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GenericResponse> CreateExecutiveCategoryAsync(ExecutiveCategoryRequest execReq)
        {
            try
            {
                var exec = _context.ExecutiveTeamCategory.FirstOrDefault(x => x.ExecutiveTeamCategoryName == execReq.ExecutiveTeamCategoryName);
                if (exec is null)
                {
                    var newExce = new ExecutiveTeamCategory
                    {
                        ExecutiveTeamCategoryName = execReq.ExecutiveTeamCategoryName,
                        CreatedBy = execReq.CreatedBy,
                        DateCreated = DateTime.Now
                    };

                    await _context.ExecutiveTeamCategory.AddAsync(newExce);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Executive Team Category Created Successfully");
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Executive Team Category Created Successfully", Data = newExce };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = $"Executive Team Category with Name {execReq.ExecutiveTeamCategoryName} exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> DeleteExecutiveCategoryAsync(Guid Id)
        {
            try
            {
                var exec = await _context.ExecutiveTeamCategory.FirstOrDefaultAsync(x => x.Id == Id);
                if (exec != null)
                {
                    _context.ExecutiveTeamCategory.Remove(exec);
                    int rsp = await _context.SaveChangesAsync();
                    if (rsp > 0)
                    {
                        _logger.LogInformation($"Executive Team Category Deleted Successful");
                        return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Executive Team Category Deleted Successfully" };
                    }
                    else
                        _logger.LogError($"Executive Team Category Delete Failed");
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Executive Team Category With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetAllExecutiveCategoryAsync()
        {
            try
            {
                var allExecs = (from pr in _context.ExecutiveTeamCategory orderby pr.ExecutiveTeamCategoryName ascending select pr)
                    .Include(x => x.ExecutiveTeams).AsNoTracking()
                    .ToList();
                if (allExecs.Count() > 0)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allExecs };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetExecutiveCategoryAsync(Guid Id)
        {
            try
            {
                var exec = (from pr in _context.ExecutiveTeamCategory where pr.Id == Id select pr)
                    .Include(x => x.ExecutiveTeams).AsNoTracking()
                    .FirstOrDefault();
                if (exec != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = exec };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> UpdateExecutiveCategoryAsync(UpdateExecutiveCategoryRequest execReq)
        {
            try
            {
                var exec = await _context.ExecutiveTeamCategory.FirstOrDefaultAsync(x => x.Id == execReq.Id);
                if (exec != null)
                {
                    exec.ExecutiveTeamCategoryName = execReq.ExecutiveTeamCategoryName;
                    exec.LastUpdatedDate = DateTime.Now;
                    exec.UpdatedBy = execReq.UpdatedBy;

                    int projRp = await _context.SaveChangesAsync();

                    if (projRp > 0)
                        _logger.LogInformation($"Executive Team CategoryName Update Successful");
                    else
                        _logger.LogError($"Executive Team CategoryName Update Failed");

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Executive Team CategoryName Updated Sucessfully" };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Executive Team CategoryName With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
    }
}
