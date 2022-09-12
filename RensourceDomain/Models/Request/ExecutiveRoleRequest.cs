using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class ExecutiveRoleRequestBase
    {
        [Required(ErrorMessage = "Executive Role Name is Required")]
        public string? ExecutiveRoleName { get; set; }
    }

    public class ExecutiveRoleRequest : ExecutiveRoleRequestBase
    {
        public Guid? CreatedBy { get; set; }
    }

    public class UpdateExecutiveRoleRequest : ExecutiveRoleRequestBase
    {

        [Required(ErrorMessage = "Id is Required")]
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
       
    }
}
