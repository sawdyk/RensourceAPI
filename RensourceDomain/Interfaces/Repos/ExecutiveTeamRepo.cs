using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RensourceDomain.Configurations;
using RensourceDomain.Interfaces.Helpers;
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
    public class ExecutiveTeamRepo : IExecutiveTeamRepo
    {
        private readonly ILogger<ExecutiveTeamRepo> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IFileUploadRepo? _fileUploadRepo;
        private FoldersConfig? _foldersConfig;
        public ExecutiveTeamRepo(ILogger<ExecutiveTeamRepo> logger, ApplicationDBContext context, IFileUploadRepo? fileUploadRepo, IOptions<FoldersConfig>? foldersConfig)
        {
            _logger = logger;
            _context = context;
            _fileUploadRepo = fileUploadRepo;
            _foldersConfig = foldersConfig.Value;
        }
        public async Task<GenericResponse> CreateExecutiveTeamAsync(ExecutiveTeamRequest execReq)
        {
            try
            {
                var response = await _fileUploadRepo.UploadImageToDirectoryAsync(execReq?.Image, _foldersConfig.Executives);
                if (response is null)
                    return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                        return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                }

                var newExec = new ExecutiveTeam
                {
                    FirstName = execReq.FirstName,
                    LastName = execReq.LastName,
                    OtherName = execReq.OtherName,
                    Image = response.Data.ToString(),
                    Profile = execReq.Profile,
                    LinkedIn = execReq.LinkedIn,
                    Twitter = execReq.Twitter,
                    ExecutiveRoleId = execReq.ExecutiveRoleId,
                    ExecutiveTeamCategoryId = execReq.ExecutiveTeamCategoryId,
                    CreatedBy = execReq.CreatedBy,
                    DateCreated = DateTime.Now
                };

                await _context.ExecutiveTeam.AddAsync(newExec);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Executive Team Member Created Successfully");

                return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Executive Team Member Created Successfully", Data = newExec };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> DeleteExecutiveTeamAsync(Guid Id)
        {
            try
            {
                var exec = await _context.ExecutiveTeam.FirstOrDefaultAsync(x => x.Id == Id);
                if (exec != null)
                {
                    _context.ExecutiveTeam.Remove(exec);
                    int rsp = await _context.SaveChangesAsync();
                    if (rsp > 0)
                    {
                        _logger.LogInformation($"Executive Team Member Deleted Successful");
                        return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Executive Team Member Deleted Successfully" };
                    }
                    else
                        _logger.LogError($"Executive Team Member Delete Failed");
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Executive Team Member With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetAllExecutiveTeamAsync()
        {
            try
            {
                var allExecs = (from pr in _context.ExecutiveTeam select pr)
                    .Include(x => x.ExecutiveRoles).AsNoTracking()
                    .Include(x => x.ExecutiveTeamCategory).AsNoTracking()
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

        public async Task<GenericResponse> GetExecutiveTeamAsync(Guid Id)
        {
            try
            {
                var exec = (from pr in _context.ExecutiveTeam where pr.Id == Id select pr)
                      .Include(x => x.ExecutiveRoles).AsNoTracking()
                      .Include(x => x.ExecutiveTeamCategory).AsNoTracking()
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

        public async Task<GenericResponse> GetExecutiveTeamByCategoryAsync(Guid categoryId)
        {
            try
            {
                var exec = (from pr in _context.ExecutiveTeam where pr.ExecutiveTeamCategoryId == categoryId select pr)
                    .Include(x => x.ExecutiveRoles).AsNoTracking()
                      .Include(x => x.ExecutiveTeamCategory).AsNoTracking()
                      .ToList();
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

        public async Task<GenericResponse> GetExecutiveTeamByRoleAsync(Guid roleId)
        {
            try
            {
                var exec = (from pr in _context.ExecutiveTeam where pr.ExecutiveRoleId == roleId select pr)
                    .Include(x => x.ExecutiveRoles).AsNoTracking()
                      .Include(x => x.ExecutiveTeamCategory).AsNoTracking()
                      .ToList();
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

        public async Task<GenericResponse> UpdateExecutiveTeamAsync(UpdateExecutiveTeamRequest execReq)
        {
            try
            {
                var exec = await _context.ExecutiveTeam.FirstOrDefaultAsync(x => x.Id == execReq.Id);
                if (exec != null)
                {
                    var response = await _fileUploadRepo.UploadImageToDirectoryAsync(execReq?.Image, _foldersConfig.Executives);
                    if (response is null)
                        return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                            return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    }

                    exec.FirstName = execReq.FirstName;
                    exec.LastName = execReq.LastName;
                    exec.OtherName = execReq.OtherName;
                    exec.Image = response.Data.ToString();
                    exec.Profile = execReq.OtherName;
                    exec.LinkedIn = execReq.OtherName;
                    exec.Twitter = execReq.OtherName;
                    exec.ExecutiveRoleId = execReq.ExecutiveRoleId;
                    exec.ExecutiveTeamCategoryId = execReq.ExecutiveTeamCategoryId;
                    exec.UpdatedBy = execReq.UpdatedBy;
                    exec.LastUpdatedDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Executive Team Member Updated Successfully");

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Executive Team Member Updated Successfully", Data = exec };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Executive Team Member With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
    }
}
