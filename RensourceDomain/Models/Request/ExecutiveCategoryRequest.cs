using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class ExecutiveCategoryRequestBase
    {
        [Required(ErrorMessage = "Executive Team CategoryName is Required")]
        public string? ExecutiveTeamCategoryName { get; set; }

    }

    public class ExecutiveCategoryRequest : ExecutiveCategoryRequestBase
    {
        public Guid? CreatedBy { get; set; }
    }

    public class UpdateExecutiveCategoryRequest : ExecutiveCategoryRequestBase
    {

        [Required(ErrorMessage = "Id is Required")]
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }

    }
}
