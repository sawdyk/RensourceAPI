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
    public class PartnerRepo : IPartnerRepo
    {
        private readonly ILogger<PartnerRepo> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IFileUploadRepo? _fileUploadRepo;
        private FoldersConfig? _foldersConfig;
        public PartnerRepo(ILogger<PartnerRepo> logger, ApplicationDBContext context, IFileUploadRepo? fileUploadRepo, IOptions<FoldersConfig>? foldersConfig)
        {
            _logger = logger;
            _context = context;
            _fileUploadRepo = fileUploadRepo;
            _foldersConfig = foldersConfig.Value;
        }
        public async Task<GenericResponse> CreatePartnerAsync(PartnerRequest partnerRequest)
        {
            try
            {
                var partner = _context.Partners.FirstOrDefault(x => x.PartnerName == partnerRequest.PartnerName);
                if (partner is null)
                {
                    var response = await _fileUploadRepo.UploadImageToDirectoryAsync(partnerRequest?.Image, _foldersConfig.Partners);
                    if (response is null)
                        return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                            return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    }

                    var newPart = new Partners
                    {
                        PartnerName = partnerRequest.PartnerName,
                        Image = response.Data.ToString(),
                        CreatedBy = partnerRequest.CreatedBy,
                        DateCreated = DateTime.Now
                    };

                    await _context.Partners.AddAsync(newPart);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Partner Created Successfully");
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Partner Created Successfully", Data = newPart };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = $"Partner with title {partnerRequest.PartnerName} exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetAllPartnerAsync()
        {
            try
            {
                var allPart = from pr in _context.Partners orderby pr.Id descending select pr;
                if (allPart.Count() > 0)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allPart };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetPartnerAsync(Guid Id)
        {
            try
            {
                var part = (from pr in _context.Partners where pr.Id == Id select pr).FirstOrDefault();
                if (part != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = part };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> UpdatePartnerAsync(UpdatePartnerRequest partnerRequest)
        {
            try
            {
                var part = await _context.Partners.FirstOrDefaultAsync(x => x.Id == partnerRequest.Id);
                if (part != null)
                {
                    var response = await _fileUploadRepo.UploadImageToDirectoryAsync(partnerRequest?.Image, _foldersConfig.Partners);
                    if (response is null)
                        return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                            return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    }

                    part.PartnerName = partnerRequest.PartnerName;
                    part.Image = response.Data.ToString();
                    part.LastUpdatedDate = DateTime.Now;
                    part.UpdatedBy = partnerRequest.UpdatedBy;

                    int partRp = await _context.SaveChangesAsync();

                    if (partRp > 0)
                        _logger.LogInformation($"Partner Update Successful");
                    else
                        _logger.LogError($"Partner Update Failed");

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Partner Updated Sucessfully" };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Partner With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
        public async Task<GenericResponse> DeletePartnerAsync(Guid Id)
        {
            try
            {
                var part = await _context.Partners.FirstOrDefaultAsync(x => x.Id == Id);
                if (part != null)
                {
                    _context.Partners.Remove(part);
                    int rsp = await _context.SaveChangesAsync();
                    if (rsp > 0)
                    {
                        _logger.LogInformation($"Partner Deleted Successful");
                        return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Partner Deleted Successfully" };
                    }
                    else
                        _logger.LogError($"Partner Delete Failed");
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Partner With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
    }
}
