using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class UserLoginRequest
    {
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }

    }
}
