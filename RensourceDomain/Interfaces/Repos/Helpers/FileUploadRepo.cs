using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RensourceDomain.Configurations;
using RensourceDomain.Interfaces.Helpers;
using RensourceDomain.Models.Response;
using RensourcePersistence.AppDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces.Repos.Helpers
{
    public class FileUploadRepo : IFileUploadRepo
    {
        private readonly ILogger<FileUploadRepo> _logger;
        private FileUrlConfig? _fileUrlConfig;
        public FileUploadRepo(ILogger<FileUploadRepo> logger, IOptions<FileUrlConfig>? fileUrlConfig)
        {
            _logger = logger;
            _fileUrlConfig = fileUrlConfig.Value;
        }

        public string[] ImageFormats()
        {
            return new string[] { ".jpeg", ".png", ".gif", ".tiff", ".psd", ".jpg",".svg" };
        }
        public async Task<GenericResponse> UploadImageToDirectoryAsync(IFormFile file, string folder)
        {
            var response = new GenericResponse();
            try
            {
                if (file == null || file.Length <= 0)
                {
                    _logger.LogError($"UploadImageToDirectoryAsync:=> Please kindly upload the File");
                    return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = $"Please kindly upload the File" };
                }
                else if (!ImageFormats().Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    _logger.LogError($"UploadImageToDirectoryAsync:=> Please kindly upload In Image Format");
                    return new GenericResponse { StatusCode = HttpStatusCode.BadRequest, StatusMessage = $"Please kindly upload In Image Format" };
                }
                else
                {
                    
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/" + folder);
                    //string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads\\" + folder);
                    _logger.LogInformation($"UploadImageToDirectoryAsync:=> Upload Path => {path}");

                    string fullPathWithFileName = path + "/" + file.FileName;
                    string fileUrl = $"{_fileUrlConfig.BaseUrl}{_fileUrlConfig.MainFolder}{folder}{file.FileName}";
                    _logger.LogInformation($"UploadImageToDirectoryAsync:=> fileUrl => [{fileUrl}]");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var stream = new FileStream(fullPathWithFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        file?.CopyToAsync(stream);
                        response = new GenericResponse { StatusCode = HttpStatusCode.OK, StatusMessage = $"Successful", Data = fileUrl };
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return default;
            }
        }
    }
}
