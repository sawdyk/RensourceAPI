using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly IPartnerRepo _partnerRepo;
        public PartnersController(IPartnerRepo partnerRepo)
        {
            _partnerRepo = partnerRepo;
        }

        [HttpPost]
        [Route("CreatePartner")]
        [SwaggerOperation(Summary = "Create a new Partner", Description = "This Endpoint Creates a new Partner")]
        public async Task<IActionResult> CreatePartnerAsync(PartnerRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _partnerRepo.CreatePartnerAsync(req);

            return Ok(result);
        }

        [HttpGet]
        [Route("AllPartner")]
        [SwaggerOperation(Summary = "All Partner", Description = "This Endpoint Returns a list of all Partner Created")]
        public async Task<IActionResult> GetAllPartnerAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _partnerRepo.GetAllPartnerAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("Partner")]
        [SwaggerOperation(Summary = "Get a specific Partner", Description = "This Endpoint Returns a specific Partner")]
        public async Task<IActionResult> GetPartnerAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _partnerRepo.GetPartnerAsync(Id);

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdatePartner")]
        [SwaggerOperation(Summary = "Update a specific Partner", Description = "This Endpoint Updates a specifc Partner")]
        public async Task<IActionResult> UpdatePartnerAsync(UpdatePartnerRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _partnerRepo.UpdatePartnerAsync(req);

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeletePartner")]
        [SwaggerOperation(Summary = "Delete a specific Partner", Description = "This Endpoint Deletes a Partner")]
        public async Task<IActionResult> DeletePartnerAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _partnerRepo.DeletePartnerAsync(Id);

            return Ok(result);
        }
    }
}
