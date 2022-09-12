using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsRepo _projectsRepo;

        public ProjectsController(IProjectsRepo projectsRepo)
        {
            _projectsRepo = projectsRepo;
        }

        [HttpPost]
        [Route("CreateProject")]
        [SwaggerOperation(Summary = "Create a new Project", Description = "This Endpoint Creates a new Project")]
        public async Task<IActionResult> CreateProjectAsync([FromForm]ProjectRequest projReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _projectsRepo.CreateProjectAsync(projReq);

            return Ok(result);
        }

        [HttpGet]
        [Route("AllProjects")]
        [SwaggerOperation(Summary = "All Projects", Description = "This Endpoint Returns a list of all Projects Created, specifying the number of projects to be returned")]
        public async Task<IActionResult> GetAllProjectsAsync(int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _projectsRepo.GetAllProjectsAsync(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet]
        [Route("Project")]
        [SwaggerOperation(Summary = "Get a specific Project", Description = "This Endpoint Returns a specific project")]
        public async Task<IActionResult> GetProjectAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _projectsRepo.GetProjectAsync(Id);

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateProject")]
        [SwaggerOperation(Summary = "Update a specific Project", Description = "This Endpoint Updates a specifc project")]
        public async Task<IActionResult> UpdateProjectAsync([FromForm] UpdateProjectRequest projReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _projectsRepo.UpdateProjectAsync(projReq);

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteProject")]
        [SwaggerOperation(Summary = "Delete a specific Project", Description = "This Endpoint Deletes a specifc project")]
        public async Task<IActionResult> DeleteProjectAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _projectsRepo.DeleteProjectAsync(Id);

            return Ok(result);
        }

    }
}
