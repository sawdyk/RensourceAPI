using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsRepo _contactUsRepo;

        public ContactUsController(IContactUsRepo contactUsRepo)
        {
            _contactUsRepo = contactUsRepo;
        }

        [HttpPost]
        [Route("CreateMessage")]
        [SwaggerOperation(Summary = "Create a new Message", Description = "This Endpoint Creates a new Message")]
        public async Task<IActionResult> CreateMessageAsync(ContactUsRequest conReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _contactUsRepo.CreateMessageAsync(conReq);

            return Ok(result);
        }

        [HttpGet]
        [Route("AllMessages")]
        [SwaggerOperation(Summary = "All Messages", Description = "This Endpoint Returns a list of all Messages")]
        public async Task<IActionResult> GetAllMessageAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _contactUsRepo.GetAllMessageAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("Message")]
        [SwaggerOperation(Summary = "Get a specific Message", Description = "This Endpoint Returns a specific Message")]
        public async Task<IActionResult> GetMessageAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _contactUsRepo.GetMessageAsync(Id);

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteMessage")]
        [SwaggerOperation(Summary = "Delete a specific Message", Description = "This Endpoint Deletes a specifc Message")]
        public async Task<IActionResult> DeleteMessageAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _contactUsRepo.DeleteMessageAsync(Id);

            return Ok(result);
        }
    }
}
