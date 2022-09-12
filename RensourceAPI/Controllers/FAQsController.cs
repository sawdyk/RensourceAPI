using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FAQsController : ControllerBase
    {
        private readonly IFAQsRepo _fAQsRepo;

        public FAQsController(IFAQsRepo fAQsRepo)
        {
            _fAQsRepo = fAQsRepo;
        }

        [HttpPost]
        [Route("CreateFAQ")]
        [SwaggerOperation(Summary = "Create a new FAQ", Description = "This Endpoint Creates a new FAQ")]
        public async Task<IActionResult> CreateFAQAsync(FAQRequest faqReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _fAQsRepo.CreateFAQAsync(faqReq);

            return Ok(result);
        }

        [HttpGet]
        [Route("AllFAQ")]
        [SwaggerOperation(Summary = "All FAQ", Description = "This Endpoint Returns a list of all FAQ Created")]
        public async Task<IActionResult> GetAllFAQAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _fAQsRepo.GetAllFAQAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("FAQ")]
        [SwaggerOperation(Summary = "Get a specific FAQ", Description = "This Endpoint Returns a specific FAQ")]
        public async Task<IActionResult> GetFAQAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _fAQsRepo.GetFAQAsync(Id);

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateFAQ")]
        [SwaggerOperation(Summary = "Update a specific FAQ", Description = "This Endpoint Updates a specifc FAQ")]
        public async Task<IActionResult> UpdateFAQAsync(UpdateFAQRequest faqReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _fAQsRepo.UpdateFAQAsync(faqReq);

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteFAQ")]
        [SwaggerOperation(Summary = "Delete a specific FAQ", Description = "This Endpoint Deletes a FAQ")]
        public async Task<IActionResult> DeleteFAQAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _fAQsRepo.DeleteFAQAsync(Id);

            return Ok(result);
        }
    }
}
