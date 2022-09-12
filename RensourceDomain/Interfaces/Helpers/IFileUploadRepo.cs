using Microsoft.AspNetCore.Http;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces.Helpers
{
    public interface IFileUploadRepo
    {
        Task<GenericResponse> UploadImageToDirectoryAsync(IFormFile file, string folder);
        string[] ImageFormats();
    }
}
