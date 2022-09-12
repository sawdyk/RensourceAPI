using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExecRoleController : ControllerBase
    {
        private readonly IExecutiveRoleRepo _executiveRoleRepo;

        public ExecRoleController(IExecutiveRoleRepo executiveRoleRepo)
        {
            _executiveRoleRepo = executiveRoleRepo;
        }

        [HttpPost]
        [Route("CreateExecRole")]
        [SwaggerOperation(Summary = "Create a new Executive Role", Description = "This Endpoint Creates a new Executive Role")]
        public async Task<IActionResult> CreateExecutiveRoleAsync(ExecutiveRoleRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveRoleRepo.CreateExecutiveRoleAsync(req);

            return Ok(result);
        }

        [HttpGet]
        [Route("AllExecRoles")]
        [SwaggerOperation(Summary = "All Executive Roles", Description = "This Endpoint Returns a list of all  Executive Roles Created")]
        public async Task<IActionResult> GetAllExecutiveRolesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveRoleRepo.GetAllExecutiveRolesAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("ExecRole")]
        [SwaggerOperation(Summary = "Get a specific Executive Role", Description = "This Endpoint Returns a specific Executive Role")]
        public async Task<IActionResult> GetExecutiveRoleAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveRoleRepo.GetExecutiveRoleAsync(Id);

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateExecRole")]
        [SwaggerOperation(Summary = "Update a specific Executive Role", Description = "This Endpoint Updates a specifc Executive Role")]
        public async Task<IActionResult> UpdateExecutiveRoleAsync(UpdateExecutiveRoleRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveRoleRepo.UpdateExecutiveRoleAsync(req);

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteExecRole")]
        [SwaggerOperation(Summary = "Delete a specific Executive Role", Description = "This Endpoint Deletes a specifc Executive Role")]
        public async Task<IActionResult> DeleteExecutiveRoleAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveRoleRepo.DeleteExecutiveRoleAsync(Id);

            return Ok(result);
        }

    }
}
