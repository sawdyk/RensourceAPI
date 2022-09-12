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
    public class FAQsRepo : IFAQsRepo
    {
        private readonly ILogger<FAQsRepo> _logger;
        private readonly ApplicationDBContext _context;
        public FAQsRepo(ILogger<FAQsRepo> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GenericResponse> CreateFAQAsync(FAQRequest fAQReq)
        {
            try
            {
                var faq = _context.FAQs.FirstOrDefault(x => x.Question == fAQReq.Question);
                if (faq is null)
                {
                    var newFaq = new FAQs
                    {
                        Question = fAQReq.Question,
                        Answer = fAQReq.Answer,
                        CreatedBy = fAQReq.CreatedBy,
                        DateCreated = DateTime.Now
                    };

                    await _context.FAQs.AddAsync(newFaq);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"FAQ Created Successfully");
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "FAQ Created Successfully", Data = newFaq };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = $"FAQ with Question {fAQReq.Question} exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetAllFAQAsync()
        {
            try
            {
                var allFaqs = from pr in _context.FAQs
                                orderby pr.Id descending
                                select pr;

                if (allFaqs.Count() > 0)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allFaqs };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetFAQAsync(Guid Id)
        {
            try
            {
                var faq = (from pr in _context.FAQs where pr.Id == Id select pr).FirstOrDefault();
                if (faq != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = faq };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> UpdateFAQAsync(UpdateFAQRequest fAQReq)
        {
            try
            {
                var faq = await _context.FAQs.FirstOrDefaultAsync(x => x.Id == fAQReq.Id);
                if (faq != null)
                {
                    faq.Question = fAQReq.Question;
                    faq.Answer = fAQReq.Answer;
                    faq.LastUpdatedDate = DateTime.Now;
                    faq.UpdatedBy = faq.UpdatedBy;

                    int projRp = await _context.SaveChangesAsync();

                    if (projRp > 0)
                        _logger.LogInformation($"FAQ Update Successful");
                    else
                        _logger.LogError($"FAQ Update Failed");

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "FAQ Updated Sucessfully" };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "FAQ With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> DeleteFAQAsync(Guid Id)
        {
            try
            {
                var faq = await _context.FAQs.FirstOrDefaultAsync(x => x.Id == Id);
                if (faq != null)
                {
                    _context.FAQs.Remove(faq);
                    int rsp = await _context.SaveChangesAsync();
                    if (rsp > 0)
                    {
                        _logger.LogInformation($"FAQ Deleted Successful");
                        return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "FAQ Deleted Successfully" };
                    }
                    else
                        _logger.LogError($"FAQ Delete Failed");
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "FAQ With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

    }
}
