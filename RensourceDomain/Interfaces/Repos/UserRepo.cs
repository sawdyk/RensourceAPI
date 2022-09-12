using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RensourceDomain.Configurations;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using RensourcePersistence.AppDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly EmailConfig _emailConfig;
        private readonly ILogger<UserRepo> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IPasswordEncryptRepo _passwordEncryptRepo;
        public UserRepo(IOptions<EmailConfig> emailConfig, ILogger<UserRepo> logger, ApplicationDBContext context, IPasswordEncryptRepo passwordEncryptRepo)
        {
            _emailConfig = emailConfig.Value;
            _logger = logger;
            _context = context;
            _passwordEncryptRepo = passwordEncryptRepo;
        }

        public async Task<GenericResponse> UserLoginAsync(UserLoginRequest loginReq)
        {
            try
            {
                //Encrypt password
                var password = _passwordEncryptRepo.EncryptPassword(loginReq.Password.Trim());
                var user =   _context.Users.FirstOrDefault(x => x.EmailAddress == loginReq.EmailAddress && x.Password == password);
                if (user != null)
                {
                    //Get the user role
                    var getUserRole =  _context.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
                    var userData = new UserLoginResponse
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        MiddleName = user.MiddleName,
                        EmailAddress = user.EmailAddress,
                        PhoneNumber = user.PhoneNumber,
                        IsActive = user.IsActive,
                        LastLoginDate = user.LastLoginDate,
                        LastUpdatedDate = user.LastUpdatedDate,
                        LastPasswordChangedDate = user.LastPasswordChangedDate,
                        DateCreated = user.DateCreated,
                        UserRole = new UserRoleResponse
                        {
                            RoleId = getUserRole.RoleId,
                            UserId = getUserRole.UserId,
                        } 
                    };

                    //Update the login date
                    user.LastLoginDate = DateTime.Now;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Login Successful for User: {loginReq.EmailAddress}; DateTime: {DateTime.Now}");
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = userData };
                }

                _logger.LogError($"Login Failed for User {loginReq.EmailAddress}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Invalid Username/Password" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public Task<GenericResponse> ChangePasswordAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse> ForgotPasswordAsync()
        {
            throw new NotImplementedException();
        }       
    }
}
