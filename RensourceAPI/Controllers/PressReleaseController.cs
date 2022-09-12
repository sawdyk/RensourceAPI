using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PressReleaseController : ControllerBase
    {
        private readonly IPressReleaseRepo _pressReleaseRepo;

        public PressReleaseController(IPressReleaseRepo pressReleaseRepo)
        {
            _pressReleaseRepo = pressReleaseRepo;
        }

        [HttpPost]
        [Route("CreatePressRelease")]
        [SwaggerOperation(Summary = "Create a new Press Release", Description = "This Endpoint Creates a new Press Release")]
        public async Task<IActionResult> CreatePressReleaseAsync([FromForm] PressReleaseRequest pressReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _pressReleaseRepo.CreatePressReleaseAsync(pressReq);

            return Ok(result);
        }

        [HttpGet]
        [Route("AllPressRelease")]
        [SwaggerOperation(Summary = "All Press Release", Description = "This Endpoint Returns a list of all Press Release Created, specifying the number of Press Release to be returned")]
        public async Task<IActionResult> GetAllPressReleaseAsync(int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _pressReleaseRepo.GetAllPressReleaseAsync(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet]
        [Route("PressRelease")]
        [SwaggerOperation(Summary = "Get a specific Press Release", Description = "This Endpoint Returns a specific Press Release")]
        public async Task<IActionResult> GetPressReleaseAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _pressReleaseRepo.GetPressReleaseAsync(Id);

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdatePressRelease")]
        [SwaggerOperation(Summary = "Update a specific Press Release", Description = "This Endpoint Updates a specifc Press Release")]
        public async Task<IActionResult> UpdatePressReleaseAsync([FromForm] UpdatePressReleaseRequest pressReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _pressReleaseRepo.UpdatePressReleaseAsync(pressReq);

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeletePressRelease")]
        [SwaggerOperation(Summary = "Delete a specific Press Release", Description = "This Endpoint Deletes a specifc Press Release")]
        public async Task<IActionResult> DeletePressReleaseAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _pressReleaseRepo.DeletePressReleaseAsync(Id);

            return Ok(result);
        }
    }
}
