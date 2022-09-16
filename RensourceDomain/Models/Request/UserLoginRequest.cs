﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class UserLoginRequest
    {
        [Required]
        public string? EmailAddress { get; set; }
        [Required]
        public string? Password { get; set; }

    }
}
