using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class FAQRequestBase
    {
        [Required(ErrorMessage = "Question is Required")]
        public string? Question { get; set; }
        [Required(ErrorMessage = "Answer is Required")]
        public string? Answer { get; set; }
    }

    public class FAQRequest : FAQRequestBase
    {
        [Required(ErrorMessage = "CreatedBy is Required")]
        public Guid? CreatedBy { get; set; }
    }
    public class UpdateFAQRequest : FAQRequestBase
    {
        [Required(ErrorMessage = "Id is Required")]
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
