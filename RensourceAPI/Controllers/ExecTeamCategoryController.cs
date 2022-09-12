using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExecTeamCategoryController : ControllerBase
    {
        private readonly IExecutiveCategoryRepo _executiveCategoryRepo;

        public ExecTeamCategoryController(IExecutiveCategoryRepo executiveCategoryRepo)
        {
            _executiveCategoryRepo = executiveCategoryRepo;
        }

        [HttpPost]
        [Route("CreateExecCategory")]
        [SwaggerOperation(Summary = "Create a new Executive Category", Description = "This Endpoint Creates a new Executive Category")]
        public async Task<IActionResult> CreateExecutiveCategoryAsync(ExecutiveCategoryRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveCategoryRepo.CreateExecutiveCategoryAsync(req);

            return Ok(result);
        }

        [HttpGet]
        [Route("AllExecCategory")]
        [SwaggerOperation(Summary = "All Executive Category", Description = "This Endpoint Returns a list of all  Executive Category Created")]
        public async Task<IActionResult> GetAllExecutiveCategoryAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveCategoryRepo.GetAllExecutiveCategoryAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("ExecCategory")]
        [SwaggerOperation(Summary = "Get a specific Executive Category", Description = "This Endpoint Returns a specific Executive Category")]
        public async Task<IActionResult> GetExecutiveCategoryAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveCategoryRepo.GetExecutiveCategoryAsync(Id);

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateExecCategory")]
        [SwaggerOperation(Summary = "Update a specific Executive Category", Description = "This Endpoint Updates a specifc Executive Category")]
        public async Task<IActionResult> UpdateExecutiveCategoryAsync(UpdateExecutiveCategoryRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveCategoryRepo.UpdateExecutiveCategoryAsync(req);

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteExecCategory")]
        [SwaggerOperation(Summary = "Delete a specific Executive Category", Description = "This Endpoint Deletes a specifc Executive Category")]
        public async Task<IActionResult> DeleteExecutiveCategoryAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _executiveCategoryRepo.DeleteExecutiveCategoryAsync(Id);

            return Ok(result);
        }
    }
}
