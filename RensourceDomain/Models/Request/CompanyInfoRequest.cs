using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class CompanyInfoRequest
    {
        public string? OfficeAddress { get; set; }
        public string? HeadOfficeAddress { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber1 { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? LinkedIn { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
