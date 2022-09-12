using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RensourceDomain.Interfaces;
using RensourceDomain.Models.Request;
using Swashbuckle.AspNetCore.Annotations;

namespace RensourceAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepo _blogRepo;

        public BlogController(IBlogRepo blogRepo)
        {
            _blogRepo = blogRepo;
        }

        [HttpPost]
        [Route("CreateBlog")]
        [SwaggerOperation(Summary = "Create a new Blog", Description = "This Endpoint Creates a new Blog")]
        public async Task<IActionResult> CreateBlogAsync(BlogRequest blogReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _blogRepo.CreateBlogAsync(blogReq);

            return Ok(result);
        }

        [HttpGet]
        [Route("AllBlogs")]
        [SwaggerOperation(Summary = "All Blogs", Description = "This Endpoint Returns a list of all Blogs Created, specifying the number of Blogs to be returned")]
        public async Task<IActionResult> GetAllBlogAsync(int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _blogRepo.GetAllBlogAsync(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet]
        [Route("Blog")]
        [SwaggerOperation(Summary = "Get a specific Blog", Description = "This Endpoint Returns a specific Blog")]
        public async Task<IActionResult> GetBlogAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _blogRepo.GetBlogAsync(Id);

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateBlog")]
        [SwaggerOperation(Summary = "Update a specific Blog", Description = "This Endpoint Updates a specifc Blog")]
        public async Task<IActionResult> UpdateBlogAsync(UpdateBlogRequest blogReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _blogRepo.UpdateBlogAsync(blogReq);

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteBlog")]
        [SwaggerOperation(Summary = "Delete a specific Blog", Description = "This Endpoint Deletes a Blog")]
        public async Task<IActionResult> DeleteBlogAsync(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _blogRepo.DeleteBlogAsync(Id);

            return Ok(result);
        }
    }
}
