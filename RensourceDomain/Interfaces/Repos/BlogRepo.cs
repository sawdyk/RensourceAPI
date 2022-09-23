using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RensourceDomain.Configurations;
using RensourceDomain.Helpers.Enums;
using RensourceDomain.Interfaces.Helpers;
using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using RensourcePersistence.AppDBContext;
using RensourcePersistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces.Repos
{
    public class BlogRepo : IBlogRepo
    {
        private readonly ILogger<BlogRepo> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IFileUploadRepo? _fileUploadRepo;
        private FoldersConfig? _foldersConfig;
        public BlogRepo(ILogger<BlogRepo> logger, ApplicationDBContext context, IFileUploadRepo? fileUploadRepo, IOptions<FoldersConfig>? foldersConfig)
        {
            _logger = logger;
            _context = context;
            _fileUploadRepo = fileUploadRepo;
            _foldersConfig = foldersConfig.Value;
        }

        public async Task<GenericResponse> CreateBlogAsync(BlogRequest blogReq)
        {
            try
            {
                var blog = _context.Blog.FirstOrDefault(x => x.Title == blogReq.Title);
                if (blog is null)
                {
                    var response = await _fileUploadRepo.UploadImageToDirectoryAsync(blogReq?.Image, _foldersConfig.Blog);
                    if (response is null)
                        return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                            return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    }

                    IList<string> tags = blogReq.Tags;
                    string tagList = string.Join(",", tags);

                    var newBlog = new Blog
                    {
                        Title = blogReq.Title,
                        Image = response.Data.ToString(),
                        VideoLink = blogReq.VideoLink,
                        Content = blogReq.Content,
                        Tags = tagList,
                        CreatedBy = blogReq.CreatedBy,
                        DateCreated = blogReq.DateCreated
                    };

                    await _context.Blog.AddAsync(newBlog);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Blog Created Successfully");
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Blog Created Successfully", Data = newBlog };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = $"Blog with title {blogReq.Title} exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<PaginationResponse> GetAllBlogAsync(int pageNumber, int pageSize)
        {
            try
            {
                var allBlogs = (from pr in _context.Blog
                               orderby pr.DateCreated descending
                               select pr).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                var blogsCount = (from pr in _context.Blog select pr).Count();
                if (allBlogs.Count() > 0)
                {
                    return new PaginationResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allBlogs , TotalData = blogsCount };
                }

                return new PaginationResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new PaginationResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<PaginationResponse> GetAllBlogsByOrderingAsync(int pageNumber, int pageSize, OrderFilter order)
        {
            try
            {
                var allBlogs = from pr in _context.Blog select pr;
                if (order == OrderFilter.Ascending)
                    allBlogs = (from pr in allBlogs orderby pr.DateCreated ascending select pr)
                        .Skip((pageNumber - 1) * pageSize).Take(pageSize);
                if (order == OrderFilter.Descending)
                    allBlogs = (from pr in allBlogs orderby pr.DateCreated descending select pr)
                        .Skip((pageNumber - 1) * pageSize).Take(pageSize);
                var blogsCount = (from pr in _context.Blog select pr).Count();
                if (allBlogs.Count() > 0)
                {
                    return new PaginationResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = allBlogs, TotalData = blogsCount };
                }

                return new PaginationResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new PaginationResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetBlogAsync(Guid Id)
        {
            try
            {
                var blog = (from pr in _context.Blog where pr.Id == Id select pr).FirstOrDefault();
                if (blog != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = blog };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> GetBlogByTitleAsync(string? title)
        {
            try
            {
                var blog = (from pr in _context.Blog where pr.Title == title select pr).FirstOrDefault();
                if (blog != null)
                {
                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Successful", Data = blog };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.NoContent, StatusMessage = "No Data Found" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }

        public async Task<GenericResponse> UpdateBlogAsync(UpdateBlogRequest blogReq)
        {
            try
            {
                var blog = await _context.Blog.FirstOrDefaultAsync(x => x.Id == blogReq.Id);
                if (blog != null)
                {
                    var response = await _fileUploadRepo.UploadImageToDirectoryAsync(blogReq?.Image, _foldersConfig.Blog);
                    if (response is null)
                        return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                            return new GenericResponse { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                    }

                    IList<string> tags = blogReq.Tags;
                    string tagList = string.Join(",", tags);

                    blog.Title = blogReq.Title;
                    blog.Image = response.Data.ToString();
                    blog.Content = blogReq.Content;
                    blog.Tags = tagList;
                    blog.VideoLink = blogReq.VideoLink;
                    blog.LastUpdatedDate = DateTime.Now;
                    blog.UpdatedBy = blogReq.UpdatedBy;
                    blog.DateCreated = blogReq.DateCreated;

                    int projRp = await _context.SaveChangesAsync();

                    if (projRp > 0)
                        _logger.LogInformation($"Blog Update Successful");
                    else
                        _logger.LogError($"Blog Update Failed");

                    return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Blog Updated Sucessfully" };
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Blog With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
        public async Task<GenericResponse> DeleteBlogAsync(Guid Id)
        {
            try
            {
                var blog = await _context.Blog.FirstOrDefaultAsync(x => x.Id == Id);
                if (blog != null)
                {
                    _context.Blog.Remove(blog);
                    int rsp = await _context.SaveChangesAsync();
                    if (rsp > 0)
                    {
                        _logger.LogInformation($"Blog Deleted Successful");
                        return new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = "Blog Deleted Successfully" };
                    }
                    else
                        _logger.LogError($"Blog Delete Failed");
                }

                return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = "Blog With Specified Id does not exists" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return new GenericResponse { StatusCode = HttpStatusCode.InternalServerError, StatusMessage = $"An Error Occurred! {ex.Message}" };
            }
        }
    }
}
