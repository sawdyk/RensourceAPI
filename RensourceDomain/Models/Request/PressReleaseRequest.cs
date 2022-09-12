using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class PressReleaseRequestBase
    {
        [Required(ErrorMessage = "Title is Required")]
        public string? Title { get; set; }
        public string? Content { get; set; }
        [Required(ErrorMessage = "Image is Required")]
        public IFormFile? Image { get; set; }
        public string? VideoLink { get; set; }
        public List<string>? Tags { get; set; }
    }

    public class PressReleaseRequest : PressReleaseRequestBase
    {
        [Required(ErrorMessage = "CreatedBy is Required")]
        public Guid? CreatedBy { get; set; }
    }
    public class UpdatePressReleaseRequest : PressReleaseRequestBase
    {
        [Required(ErrorMessage = "Id is Required")]
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
