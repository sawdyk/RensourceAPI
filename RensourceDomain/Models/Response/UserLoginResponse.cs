using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Response
{
    public class UserLoginResponse
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime DateCreated { get; set; }
        public UserRoleResponse UserRole { get; set; }
    }
    public class UserRoleResponse
    {
        public Guid? UserId { get; set; }
        public int? RoleId { get; set; }
    }
}
