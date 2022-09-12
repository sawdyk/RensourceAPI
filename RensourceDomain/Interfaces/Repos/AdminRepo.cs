using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RensourceDomain.Helpers;
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
    public class AdminRepo : IAdminRepo
    {
        private readonly ILogger<AdminRepo> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IPasswordEncryptRepo _passwordEncryptRepo;
        public AdminRepo(ILogger<AdminRepo> logger, ApplicationDBContext context, IPasswordEncryptRepo passwordEncryptRepo)
        {
            _logger = logger;
            _context = context;
            _passwordEncryptRepo = passwordEncryptRepo;
        }
        public async Task<GenericResponse> CreatAdminAsync(UserCreationRequest userReq)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.EmailAddress == userReq.EmailAddress);
                if (user is null)
                {
                    //Ecrypt Password
                    var userPassword = _passwordEncryptRepo.EncryptPassword(userReq.Password.Trim());
                    var newUser = new Users
                    {
                        FirstName = userReq.FirstName,
                        LastName = userReq.LastName,
                        MiddleName = userReq.MiddleName,
                        EmailAddress = userReq.EmailAddress,
                        PhoneNumber = userReq.PhoneNumber,
                        Password = userPassword,
                        IsActive = true,
                        DateCreated = DateTime.Now,
                    };

                    await _context.Users.AddAsync(newUser);
                    int usrRp = await _context.SaveChangesAsync();

                    if (usrRp > 0)
                    {
                        _logger.LogInformation($"New User Creation Successful");
                        //user role
                        var usrRol = new UserRoles
                        {
                            UserId = newUser.Id,
                            RoleId = (int?)EnumUtility.UserRoles.Admin,
                            DateCreated  = DateTime.Now,
                        };

                        await _context.UserRoles.AddAsync(usrRol);
                        int rolRsp = await _context.SaveChangesAsync();
                        if(rolRsp > 0)
                            _logger.LogInformation($"New User Role Creation Successful");
                        else
                            _logger.LogInformation($"New User Role Creation Failed");
                    }
                    else
                    {
                        _logger.LogInformation($"New User Creation Failed");
                    }

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Created Sucessfully" };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "User with Email Address already exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> DeleteAdminAsync(Guid Id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
                if (user != null)
                {
                    var usrRol = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == Id);
                    if (usrRol != null)
                    {
                        _context.UserRoles.Remove(usrRol);
                        int usrlRsp = await _context.SaveChangesAsync();

                        if (usrlRsp > 0)
                        {
                            _logger.LogInformation($"User Role Deleted Successful");
                            _context.Users.Remove(user);
                            int usrRsp = await _context.SaveChangesAsync();
                            if (usrRsp > 0)
                            {
                                _logger.LogInformation($"User Deleted Successfully");
                                return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "User Deleted Successfully" };
                            }
                            else
                                _logger.LogInformation($"User Delete Failed");
                        }
                        else
                            _logger.LogInformation($"User Role Delete Failed");
                    }

                    return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "User Role does not exists" };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "User With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetAdminAsync(Guid Id)
        {
            try
            {
                var admin = (from t1 in _context.UserRoles
                             where t1.RoleId == (int?)EnumUtility.UserRoles.Admin && t1.UserId == Id
                             join t2 in _context.Users on t1.UserId equals t2.Id
                             select new
                             {
                                 t2.Id,
                                 t2.FirstName,
                                 t2.LastName,
                                 t2.MiddleName,
                                 t2.EmailAddress,
                                 t2.PhoneNumber,
                                 t2.IsActive,
                                 t2.LastLoginDate,
                                 t2.LastPasswordChangedDate,
                                 t2.LastUpdatedDate,
                                 t2.DateCreated,
                             }).FirstOrDefault();

                if (admin != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = admin };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetAllAdminAsync()
        {
            try
            {                          
                var admins = from t1 in _context.UserRoles where t1.RoleId == (int?)EnumUtility.UserRoles.Admin
                          join t2 in _context.Users on t1.UserId equals t2.Id select new
                          {
                              t2.Id,
                              t2.FirstName,
                              t2.LastName,
                              t2.MiddleName,
                              t2.EmailAddress,
                              t2.PhoneNumber,
                              t2.IsActive,
                              t2.LastLoginDate,
                              t2.LastPasswordChangedDate,
                              t2.LastUpdatedDate,
                              t2.DateCreated,
                          };

                if (admins.Count() > 0)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = admins };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> UpdateAdminAsync(UserUpdateRequest userReq)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userReq.Id);
                if (user != null)
                {
                    var chkEmail = await _context.Users.FirstOrDefaultAsync(x => x.EmailAddress == userReq.EmailAddress);

                    if (user.EmailAddress == userReq.EmailAddress || chkEmail == null)
                    {
                        user.FirstName = userReq.FirstName;
                        user.LastName = userReq.LastName;
                        user.MiddleName = userReq.MiddleName;
                        user.EmailAddress = userReq.EmailAddress;
                        user.PhoneNumber = userReq.PhoneNumber;
                        user.LastUpdatedDate = DateTime.Now;

                        int usrRp = await _context.SaveChangesAsync();

                        if (usrRp > 0)
                            _logger.LogInformation($"User Update Successful");
                        else
                            _logger.LogInformation($"User Update Failed");

                        return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Updated Sucessfully" };
                    }

                    return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Email Address Already Exists" };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "User With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
    }
}
