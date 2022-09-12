using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompanyInfoController : ControllerBase
    {
        private readonly ICompanyInfoRepo _companyInfoRepo;

        public CompanyInfoController(ICompanyInfoRepo companyInfoRepo)
        {
            _companyInfoRepo = companyInfoRepo;
        }

        [HttpPost]
        [Route("CreateUpdateCompanyInfo")]
        [SwaggerOperation(Summary = "Create or Updates the Company's Info", Description = "This Endpoint Creates or Updates the Company's Info")]
        public async Task<IActionResult> CreateUpdateCompanyInfoAsync(CompanyInfoRequest conReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _companyInfoRepo.CreateUpdateCompanyInfoAsync(conReq);

            return Ok(result);
        }

        [HttpGet]
        [Route("CompanyInfo")]
        [SwaggerOperation(Summary = "Get the Company Info", Description = "This Endpoint Returns the Company Info")]
        public async Task<IActionResult> GetCompanyInfoAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _companyInfoRepo.GetCompanyInfoAsync();

            return Ok(result);
        }
    }
}
