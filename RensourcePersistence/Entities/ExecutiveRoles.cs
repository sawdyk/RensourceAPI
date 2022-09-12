using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourcePersistence.Entities
{
    public class ExecutiveRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? ExecutiveRoleName { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime DateDeleted { get; set; }

        public ICollection<ExecutiveTeam>? ExecutiveTeams { get; set; }
    }
}
