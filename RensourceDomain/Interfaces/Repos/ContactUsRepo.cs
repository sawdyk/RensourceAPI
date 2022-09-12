﻿using Microsoft.EntityFrameworkCore;
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
    public class ContactUsRepo : IContactUsRepo
    {
        private readonly ILogger<ContactUsRepo> _logger;
        private readonly ApplicationDBContext _context;
        public ContactUsRepo(ILogger<ContactUsRepo> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GenericResponse> CreateMessageAsync(ContactUsRequest contReq)
        {
            try
            {
                var newMsg = new ContactUs
                {
                    SendersName = contReq.SendersName,
                    MessageSubject = contReq.MessageSubject,
                    Message = contReq.Message,
                    EmailAddress = contReq.EmailAddress,
                    PhoneNumber = contReq.PhoneNumber,
                    DateCreated = DateTime.Now
                };

                await _context.ContactUs.AddAsync(newMsg);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Message Created Successfully");
                return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Message Created Successfully", Data = newMsg };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> DeleteMessageAsync(Guid Id)
        {
            try
            {
                var cont = await _context.ContactUs.FirstOrDefaultAsync(x => x.Id == Id);
                if (cont != null)
                {
                    _context.ContactUs.Remove(cont);
                    int rsp = await _context.SaveChangesAsync();
                    if (rsp > 0)
                    {
                        _logger.LogInformation($"Message Deleted Successful");
                        return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Message Deleted Successfully" };
                    }
                    else
                        _logger.LogError($"Message Delete Failed");
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Message With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

       public async Task<GenericResponse> GetAllMessageAsync()
        {
            try
            {
                var allMsg = (from pr in _context.ContactUs orderby pr.Id descending select pr).ToList();
                if (allMsg.Count() > 0)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allMsg };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetMessageAsync(Guid Id)
        {
            try
            {
                var msg = (from pr in _context.ContactUs where pr.Id == Id select pr).FirstOrDefault();
                if (msg != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = msg };
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
