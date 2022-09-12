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
    public class PressReleaseRepo : IPressReleaseRepo
    {
        private readonly ILogger<PressReleaseRepo> _logger;
        private readonly ApplicationDBContext _context;
        public PressReleaseRepo(ILogger<PressReleaseRepo> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GenericResponse> CreatePressReleaseAsync(PressReleaseRequest pressReq)
        {
            try
            {
                var pressRls = _context.PressRelease.FirstOrDefault(x => x.Title == pressReq.Title);
                if (pressRls is null)
                {
                    IList<string> tags = pressReq.Tags;
                    string tagList = string.Join(",", tags);

                    var newpressRls = new PressRelease
                    {
                        Title = pressReq.Title,
                        Image = pressReq.Image,
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

        public async Task<GenericResponse> GetAllPressReleaseAsync(int pageNumber, int pageSize)
        {
            try
            {
                var allPresRls = (from pr in _context.PressRelease orderby pr.Id descending select pr).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                if (allPresRls.Count() > 0)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allPresRls };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
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
                    IList<string> tags = pressReq.Tags;
                    string tagList = string.Join(",", tags);

                    pres.Title = pressReq.Title;
                    pres.Image = pressReq.Image;
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
