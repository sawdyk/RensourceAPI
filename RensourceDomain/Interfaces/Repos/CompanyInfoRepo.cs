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
    public class CompanyInfoRepo : ICompanyInfoRepo
    {
        private readonly ILogger<CompanyInfoRepo> _logger;
        private readonly ApplicationDBContext _context;
        public CompanyInfoRepo(ILogger<CompanyInfoRepo> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GenericResponse> CreateUpdateCompanyInfoAsync(CompanyInfoRequest comReq)
        {
            try
            {
                var compInf = _context.CompanyInfo.FirstOrDefault();
                if (compInf is null)
                {
                    var newComInf = new CompanyInfo
                    {
                        OfficeAddress = comReq.OfficeAddress,
                        HeadOfficeAddress = comReq.HeadOfficeAddress,
                        EmailAddress = comReq.EmailAddress,
                        PhoneNumber1 = comReq.PhoneNumber1,
                        PhoneNumber2 = comReq.PhoneNumber2,
                        LinkedIn = comReq.LinkedIn,
                        Twitter = comReq.Twitter,
                        Instagram = comReq.Instagram,
                        Facebook = comReq.Facebook,
                        CreatedBy = comReq.UpdatedBy,
                        DateCreated = DateTime.Now
                    };

                    await _context.CompanyInfo.AddAsync(newComInf);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Company Information Created Successfully");
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Company Information Created Successfully", Data = newComInf };
                }
                else
                {
                    compInf.OfficeAddress = comReq.OfficeAddress;
                    compInf.HeadOfficeAddress = comReq.HeadOfficeAddress;
                    compInf.EmailAddress = comReq.EmailAddress;
                    compInf.PhoneNumber1 = comReq.PhoneNumber1;
                    compInf.PhoneNumber2 = comReq.PhoneNumber2;
                    compInf.LinkedIn = comReq.LinkedIn;
                    compInf.Twitter = comReq.Twitter;
                    compInf.Instagram = comReq.Instagram;
                    compInf.Facebook = comReq.Facebook;
                    compInf.UpdatedBy = comReq.UpdatedBy;
                    compInf.LastUpdatedDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Company Information Updated Successfully", Data = compInf };
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetCompanyInfoAsync()
        {
            try
            {
                var compInf = (from pr in _context.CompanyInfo select pr).FirstOrDefault();
                if (compInf != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = compInf };
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
