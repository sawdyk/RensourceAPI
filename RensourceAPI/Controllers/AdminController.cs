using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepo _superAdminRepo;

        public AdminController(IAdminRepo superAdminRepo)
        {
            _superAdminRepo = superAdminRepo;
        }

        [HttpPost]
        [Route("CreateAdmin")]
        [SwaggerOperation(Summary = "Create a new Admin User", Description = "This Endpoint Creates a new Admin User")]
        public async Task<IActionResult> CreatAdminAsync(UserCreationRequest userReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.CreatAdminAsync(userReq);

            return Ok(result);
        }

        [HttpGet]
        [Route("Alladmin")]
        [SwaggerOperation(Summary = "All Admins", Description = "This Endpoint Returns a list of all Admins Created")]
        public async Task<IActionResult> GetAllAdminAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.GetAllAdminAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("Admin")]
        [SwaggerOperation(Summary = "Get an Admin User", Description = "This Endpoint Returns a Specific Admin User")]
        public async Task<IActionResult> GetAdminAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.GetAdminAsync(Id);

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateAdmin")]
        [SwaggerOperation(Summary = "Update an Admin User Info", Description = "This Endpoint Updates an Admin User Info")]
        public async Task<IActionResult> UpdateAdminAsync(UserUpdateRequest usrReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.UpdateAdminAsync(usrReq);

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteAdmin")]
        [SwaggerOperation(Summary = "Delete an Admin User", Description = "This Endpoint Deletes a Specific Admin User")]
        public async Task<IActionResult> DeleteAdminAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.DeleteAdminAsync(Id);

            return Ok(result);
        }
    }
}

