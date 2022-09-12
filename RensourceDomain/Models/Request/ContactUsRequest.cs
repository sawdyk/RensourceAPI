using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class ContactUsRequest
    {
        [Required(ErrorMessage = "Sender's Name is Required")]
        public string? SendersName { get; set; }
        [Required(ErrorMessage = "Subject of Message is Required")]
        public string? MessageSubject { get; set; }
        [Required(ErrorMessage = "Message is Required")]
        public string? Message { get; set; }
        [Required(ErrorMessage = "EmailAddress is Required")]
        public string? EmailAddress { get; set; }
        [Required(ErrorMessage = "PhoneNumber is Required")]
        public string? PhoneNumber { get; set; }
    }
}
