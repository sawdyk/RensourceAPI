using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Request
{
    public class ExecutiveTeamRequestBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Image { get; set; }
        public Guid? ExecutiveRoleId { get; set; }
        public Guid? ExecutiveTeamCategoryId { get; set; }
        public string? Profile { get; set; }
        public string? LinkedIn { get; set; }
        public string? Twitter { get; set; }
    }

    public class ExecutiveTeamRequest : ExecutiveTeamRequestBase
    {
        public Guid? CreatedBy { get; set; }
    }

    public class UpdateExecutiveTeamRequest : ExecutiveTeamRequestBase
    {
        public Guid? Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
