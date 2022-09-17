using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RensourceDomain.Configurations;
using RensourceDomain.Helpers.EmailClient;
using RensourceDomain.Interfaces;
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
    public class UserRepo : IUserRepo
    {
        private readonly EmailConfig _emailConfig;
        private readonly ILogger<UserRepo> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IPasswordEncryptRepo _passwordEncryptRepo;
        private readonly IEmailClientRepo _emailClientRepo;

        public UserRepo(IOptions<EmailConfig> emailConfig, ILogger<UserRepo> logger, ApplicationDBContext context, IPasswordEncryptRepo passwordEncryptRepo,
            IEmailClientRepo emailClientRepo)
        {
            _emailConfig = emailConfig.Value;
            _logger = logger;
            _context = context;
            _passwordEncryptRepo = passwordEncryptRepo;
            _emailClientRepo = emailClientRepo;
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

        public async Task<GenericResponse> ForgotPasswordAsync(PasswordResetRequest request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.EmailAddress == request.EmailAddress);
                if (user != null) //user exist 
                {
                    //generate a new code for forgot password
                    Random rnd = new Random();
                    int codes = rnd.Next(000000, 900000);
                    var confirmationCode = codes.ToString(); 

                    //save the code generated
                    var forgotPassword = new ForgotPasswordCodes
                    {
                        UserId = user.Id,
                        Code = confirmationCode,
                        DateGenerated = DateTime.Now
                    };

                    await _context.ForgotPasswordCodes.AddAsync(forgotPassword);
                    await _context.SaveChangesAsync();

                    //send Mail to user 
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmailTemplates");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    } 
                    var filePath = path + "/ForgotPassword.html";
                    //var rensource = path + "/icon.jpg";
                    //var mailIcon = path + "/icon.jpg";  

                    //byte[] rensourceBytes = File.ReadAllBytes(rensource);
                    //byte[] mailIconBytes = File.ReadAllBytes(mailIcon);

                    //string base64Rensource = Convert.ToBase64String(rensourceBytes);
                    //string base64MailIcon = Convert.ToBase64String(mailIconBytes);

                    string MailContent = File.ReadAllText(filePath);
                    MailContent = MailContent.Replace("{Name}", user.FirstName);
                    MailContent = MailContent.Replace("{PasswordCode}", confirmationCode);
                    MailContent = MailContent.Replace("{Date}", DateTime.Now.ToString("yyyy"));
                    //MailContent = MailContent.Replace("{icon}", base64MailIcon);
                    //MailContent = MailContent.Replace("{icon}", base64Rensource);

                    EmailMessage message = new EmailMessage(user.EmailAddress, MailContent);
                    _emailClientRepo.SendEmailAsync(message);
                   
                    _logger.LogError($"Mail successfully sent to user for Password reset; DateTime: {DateTime.Now}");
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Check your Email for Password Reset Instructions!" };
                }
                else
                {
                    _logger.LogError($"Mail failed to send to user for Password reset; DateTime: {DateTime.Now}");
                    return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "This User doesnt exist!" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                var codeCheck = _context.ForgotPasswordCodes.FirstOrDefault(u => u.Code == request.Code);
                if (codeCheck != null) //code exist
                {
                    Users getUser = _context.Users.FirstOrDefault(u => u.Id == codeCheck.UserId);
                    //Encrypt password
                    var password = _passwordEncryptRepo.EncryptPassword(request.Password.Trim());
                    //Update the password
                    getUser.Password = password;
                    getUser.LastPasswordChangedDate = DateTime.Now;

                    //delete the forgotpassword code after successful Update
                    _context.ForgotPasswordCodes.Remove(codeCheck);
                    await _context.SaveChangesAsync();

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Password Changed Successfully" };
                }
                else
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Invalid Code Entered" };
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; InnerException: {ex.InnerException}; Source: {ex.Source} DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

    }
}
