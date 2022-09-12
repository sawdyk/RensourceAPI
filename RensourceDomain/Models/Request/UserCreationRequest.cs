using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class UserCreationRequest
    {
        [Required(ErrorMessage = "FirstName is Required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Email Address is Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Phone Number is Required")]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Invalid Phone number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }

    public class UserUpdateRequest
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Invalid Phone number")]
        public string? PhoneNumber { get; set; }
    }


}
