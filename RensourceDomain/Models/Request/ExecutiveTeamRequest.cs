using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class ExecutiveTeamRequestBase
    {
        [Required(ErrorMessage = "FirstName is Required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        [Required(ErrorMessage = "Image is Required")]
        public IFormFile? Image { get; set; }
        [Required(ErrorMessage = "ExecutiveRoleId is Required")]
        public Guid? ExecutiveRoleId { get; set; }
        [Required(ErrorMessage = "ExecutiveTeamCategoryId is Required")]
        public Guid? ExecutiveTeamCategoryId { get; set; }
        public string? Profile { get; set; }
        public string? LinkedIn { get; set; }
        public string? Twitter { get; set; }
    }

    public class ExecutiveTeamRequest : ExecutiveTeamRequestBase
    {
        public Guid? CreatedBy { get; set; }
    }

    public class UpdateExecutiveTeamRequest : ExecutiveTeamRequestBase
    {
        public Guid? Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
