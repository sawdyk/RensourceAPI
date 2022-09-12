using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesRepo _roleRepo;

        public RolesController(IRolesRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }

        [HttpGet]
        [Route("AllRoles")]
        [SwaggerOperation(Summary = "Get All Roles (SuperAdmin, Admin)", Description = "Returns a list of all roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _roleRepo.GetAllRolesAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("Role")]
        [SwaggerOperation(Summary = "Get a specific Role", Description = "Returns a specific role")]
        public async Task<IActionResult> GetRole(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _roleRepo.GetRoleAsync(Id);

            return Ok(result);
        }
    }
}
