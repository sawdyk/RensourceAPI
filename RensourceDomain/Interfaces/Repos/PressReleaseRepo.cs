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
    public class PressReleaseRepo : IPressReleaseRepo
    {
        private readonly ILogger<PressReleaseRepo> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IFileUploadRepo? _fileUploadRepo;
        private FoldersConfig? _foldersConfig;
        public PressReleaseRepo(ILogger<PressReleaseRepo> logger, ApplicationDBContext context, IFileUploadRepo? fileUploadRepo, IOptions<FoldersConfig>? foldersConfig)
        {
            _logger = logger;
            _context = context;
            _fileUploadRepo = fileUploadRepo;
            _foldersConfig = foldersConfig.Value;
        }
        public async Task<GenericResponse> CreatePressReleaseAsync(PressReleaseRequest pressReq)
        {
            try
            {
                var pressRls = _context.PressRelease.FirstOrDefault(x => x.Title == pressReq.Title);
                if (pressRls is null)
                {
                    var response = await _fileUploadRepo.UploadImageToDirectoryAsync(pressReq?.Image, _foldersConfig.PressRelease);
                    if (response is null)
                        return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                            return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    }

                    IList<string> tags = pressReq.Tags;
                    string tagList = string.Join(",", tags);

                    var newpressRls = new PressRelease
                    {
                        Title = pressReq.Title,
                        Image = response.Data.ToString(),
                        VideoLink = pressReq.VideoLink,
                        Tags = tagList,
                        Content = pressReq.Content,
                        CreatedBy = pressReq.CreatedBy,
                        DateCreated = DateTime.Now
                    };

                    await _context.PressRelease.AddAsync(newpressRls);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Press Release Created Successfully");
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Press Release Created Successfully", Data = newpressRls };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = $"Press Release with title {pressReq.Title} exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<PaginationResponse> GetAllPressReleaseAsync(int pageNumber, int pageSize)
        {
            try
            {
                var allPresRls = (from pr in _context.PressRelease orderby pr.Id descending select pr)
                    .Skip((pageNumber - 1) * pageSize).Take(pageSize);
                var pressReleaseCount = (from pr in _context.PressRelease select pr).Count();
                if (allPresRls.Count() > 0)
                {
                    return new PaginationResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allPresRls, TotalData = pressReleaseCount };
                }

                return new PaginationResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new PaginationResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetPressReleaseAsync(Guid Id)
        {
            try
            {
                var pressRls = (from pr in _context.PressRelease where pr.Id == Id select pr).FirstOrDefault();
                if (pressRls != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = pressRls };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> UpdatePressReleaseAsync(UpdatePressReleaseRequest pressReq)
        {
            try
            {
                var pres = await _context.PressRelease.FirstOrDefaultAsync(x => x.Id == pressReq.Id);
                if (pres != null)
                {
                    var response = await _fileUploadRepo.UploadImageToDirectoryAsync(pressReq?.Image, _foldersConfig.PressRelease);
                    if (response is null)
                        return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                            return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    }

                    IList<string> tags = pressReq.Tags;
                    string tagList = string.Join(",", tags);

                    pres.Title = pressReq.Title;
                    pres.Image = response.Data.ToString();
                    pres.Content = pressReq.Content;
                    pres.Tags = tagList;
                    pres.VideoLink = pressReq.VideoLink;
                    pres.LastUpdatedDate = DateTime.Now;
                    pres.UpdatedBy = pressReq.UpdatedBy;

                    int projRp = await _context.SaveChangesAsync();

                    if (projRp > 0)
                        _logger.LogInformation($"Press Release Update Successful");
                    else
                        _logger.LogError($"Press Release Update Failed");

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Press Release Updated Sucessfully" };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Press Release With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }

        }

        public async Task<GenericResponse> DeletePressReleaseAsync(Guid Id)
        {
            try
            {
                var press = await _context.PressRelease.FirstOrDefaultAsync(x => x.Id == Id);
                if (press != null)
                {
                    _context.PressRelease.Remove(press);
                    int rsp = await _context.SaveChangesAsync();
                    if (rsp > 0)
                    {
                        _logger.LogInformation($"Press Release Deleted Successful");
                        return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Press Release Deleted Successfully" };
                    }
                    else
                        _logger.LogError($"Press Release Delete Failed");
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Press Release With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
    }
}
