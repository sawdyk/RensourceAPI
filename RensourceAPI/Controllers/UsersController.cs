using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _usersRepo;
        private readonly IPasswordEncryptRepo ppd;

        public UsersController(IUserRepo usersRepo, IPasswordEncryptRepo ppd)
        {
            _usersRepo = usersRepo;
            this.ppd = ppd;
        }

        [HttpPost]
        [Route("UserLogin")]
        [SwaggerOperation(Summary = "User Login", Description = "This is an Endpoint for All Users Login")]
        public async Task<IActionResult> UserLoginAsync(UserLoginRequest loginReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _usersRepo.UserLoginAsync(loginReq);

            return Ok(result);
        }

        [HttpPost]
        [Route("ForgotPassword")]
        [SwaggerOperation(Summary = "User Forgot Password", Description = "This is an Endpoint for forgot password reset")]
        public async Task<IActionResult> ForgotPasswordAsync(PasswordResetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _usersRepo.ForgotPasswordAsync(request);

            return Ok(result);
        }

        [HttpPost]
        [Route("ChangePassword")]
        [SwaggerOperation(Summary = "User Change Password", Description = "This is an Endpoint for Changing users Password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _usersRepo.ChangePasswordAsync(request);

            return Ok(result);
        }
    }
}
