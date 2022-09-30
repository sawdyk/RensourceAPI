using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class ProjectRequestBase
    {
        [Required(ErrorMessage = "Title is Required")]
        public string? Title { get; set; }
        public string? SinglePageTitle { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Image is Required")]
        public IFormFile? Image { get; set; }
        public string? VideoLink { get; set; }
        public string? ProjectOverview { get; set; }
        public string? TechnicalInformation { get; set; }
        public string? PracticalBenefits { get; set; }
    }
    public class ProjectRequest : ProjectRequestBase
    {
        [Required(ErrorMessage = "CreatedBy is Required")]
        public Guid? CreatedBy { get; set; }
    }
    public class UpdateProjectRequest : ProjectRequestBase
    {
        [Required(ErrorMessage = "Id is Required")]
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
