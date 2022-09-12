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
    public class ProjectsRepo : IProjectsRepo
    {
        private readonly ILogger<ProjectsRepo> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IFileUploadRepo? _fileUploadRepo;
        private FoldersConfig? _foldersConfig;
        public ProjectsRepo(ILogger<ProjectsRepo> logger, ApplicationDBContext context, IFileUploadRepo? fileUploadRepo, IOptions<FoldersConfig>? foldersConfig)
        {
            _logger = logger;
            _context = context;
            _fileUploadRepo = fileUploadRepo;
            _foldersConfig = foldersConfig.Value;
        }
        public async Task<GenericResponse> CreateProjectAsync(ProjectRequest? projReq)
        {
            try
            {
                var proj = _context.Projects.FirstOrDefault(x => x.Title == projReq.Title);
                if (proj is null)
                {
                    var response = await _fileUploadRepo.UploadImageToDirectoryAsync(projReq?.Image, _foldersConfig.Projects);
                    if (response is null)
                        return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                            return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    }

                    var newProj = new Projects
                    {
                        Title = projReq.Title,
                        Description = projReq.Description,
                        Image = response.Data.ToString(),
                        VideoLink = projReq.VideoLink,
                        ProjectOverview = projReq.ProjectOverview,
                        TechnicalInformation = projReq.TechnicalInformation,
                        PracticalBenefits = projReq.PracticalBenefits,
                        CreatedBy = projReq.CreatedBy,
                        DateCreated = DateTime.Now
                    };

                    await _context.Projects.AddAsync(newProj);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Project Created Successfully");
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Project Created Successfully", Data = newProj };
                }                

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = $"Project with title {projReq.Title} exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetAllProjectsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var allProjects = (from pr in _context.Projects orderby pr.Id descending select pr).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                if (allProjects.Count() > 0)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allProjects };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetProjectAsync(Guid Id)
        {
            try
            {
                var project = (from pr in _context.Projects where pr.Id == Id select pr).FirstOrDefault();
                if (project != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = project };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> UpdateProjectAsync(UpdateProjectRequest projReq)
        {
            try
            {
                var proj = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projReq.Id);
                if (proj != null)
                {
                    var response = await _fileUploadRepo.UploadImageToDirectoryAsync(projReq?.Image, "Projects");
                    if (response is null)
                        return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                            return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    }

                    proj.Title = projReq.Title;
                    proj.Description = projReq.Description;
                    proj.Image = response.Data.ToString();
                    proj.ProjectOverview = projReq.ProjectOverview;
                    proj.TechnicalInformation = projReq.TechnicalInformation;
                    proj.PracticalBenefits = projReq.PracticalBenefits;
                    proj.VideoLink = projReq.VideoLink;
                    proj.LastUpdatedDate = DateTime.Now;
                    proj.UpdatedBy = projReq.UpdatedBy;

                    int projRp = await _context.SaveChangesAsync();

                    if (projRp > 0)
                        _logger.LogInformation($"Project Update Successful");
                    else
                        _logger.LogError($"Project Update Failed");

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Project Updated Sucessfully" };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Project With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> DeleteProjectAsync(Guid Id)
        {
            try
            {
                var proj = await _context.Projects.FirstOrDefaultAsync(x => x.Id == Id);
                if (proj != null)
                {
                    _context.Projects.Remove(proj);
                    int rsp = await _context.SaveChangesAsync();
                    if (rsp > 0)
                    {
                        _logger.LogInformation($"Project Deleted Successful");
                        return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Project Deleted Successfully" };
                    }
                    else
                        _logger.LogError($"Project Delete Failed");
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Project With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
    }
}
