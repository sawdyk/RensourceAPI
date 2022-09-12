using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class PartnerRequestBase
    {
        [Required(ErrorMessage = "Partner Name is Required")]
        public string? PartnerName { get; set; }
        [Required(ErrorMessage = "Image is Required")]
        public string? Image { get; set; }
    }

    public class PartnerRequest : PartnerRequestBase
    {
        [Required(ErrorMessage = "CreatedBy is Required")]
        public Guid? CreatedBy { get; set; }
    }
    public class UpdatePartnerRequest : PartnerRequestBase
    {
        [Required(ErrorMessage = "Id is Required")]
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
