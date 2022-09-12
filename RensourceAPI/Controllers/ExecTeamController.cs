using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExecTeamController : ControllerBase
    {
        private readonly IExecutiveTeamRepo _executiveTeamRepo;

        public ExecTeamController(IExecutiveTeamRepo executiveTeamRepo)
        {
            _executiveTeamRepo = executiveTeamRepo;
        }

        [HttpPost]
        [Route("CreateExecTeam")]
        [SwaggerOperation(Summary = "Create a new Executive Team Member", Description = "This Endpoint Creates a new Executive Team Member")]
        public async Task<IActionResult> CreateExecutiveTeamAsync(ExecutiveTeamRequest execReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveTeamRepo.CreateExecutiveTeamAsync(execReq);

            return Ok(result);
        }

        [HttpGet]
        [Route("AllExecTeam")]
        [SwaggerOperation(Summary = "All Executive Team Members", Description = "This Endpoint Returns a list of all Executive Team Member Created")]
        public async Task<IActionResult> GetAllExecutiveTeamAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveTeamRepo.GetAllExecutiveTeamAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("ExecTeam")]
        [SwaggerOperation(Summary = "Get a specific Executive Team Member", Description = "This Endpoint Returns a specific Executive Team Member")]
        public async Task<IActionResult> GetExecutiveTeamAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveTeamRepo.GetExecutiveTeamAsync(Id);

            return Ok(result);
        }

        [HttpGet]
        [Route("ExecTeamByCategory")]
        [SwaggerOperation(Summary = "Get a Executive Team Member By Category", Description = "This Endpoint Returns a list of Executive Team Members by Category")]
        public async Task<IActionResult> GetExecutiveTeamByCategoryAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveTeamRepo.GetExecutiveTeamByCategoryAsync(Id);

            return Ok(result);
        }

        [HttpGet]
        [Route("ExecTeamByRole")]
        [SwaggerOperation(Summary = "Get Executive Team Members By Role", Description = "This Endpoint Returns a list of Executive Team Members by Role")]
        public async Task<IActionResult> GetExecutiveTeamByRoleAsync(Guid roleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveTeamRepo.GetExecutiveTeamByRoleAsync(roleId);

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateExecTeam")]
        [SwaggerOperation(Summary = "Update a specific Executive Team Member", Description = "This Endpoint Updates a specifc Executive Team Member")]
        public async Task<IActionResult> UpdateExecutiveTeamAsync(UpdateExecutiveTeamRequest execReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveTeamRepo.UpdateExecutiveTeamAsync(execReq);

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteExecTeam")]
        [SwaggerOperation(Summary = "Delete a specific Executive Team Member", Description = "This Endpoint Deletes a specifc Executive Team Member")]
        public async Task<IActionResult> DeleteExecutiveTeamAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveTeamRepo.DeleteExecutiveTeamAsync(Id);

            return Ok(result);
        }

    }
}
