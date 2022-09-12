using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourcePersistence.Entities
{
    public class ExecutiveTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Image { get; set; }
        public Guid? ExecutiveRoleId { get; set; }
        public Guid? ExecutiveTeamCategoryId { get; set; }
        public string? Profile { get; set; }
        public string? LinkedIn { get; set; }
        public string? Twitter { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime DateDeleted { get; set; }

        [ForeignKey("ExecutiveTeamCategoryId")]
        public virtual ExecutiveTeamCategory? ExecutiveTeamCategory { get; set; }

        [ForeignKey("ExecutiveRoleId")]
        public virtual ExecutiveRoles? ExecutiveRoles { get; set; }
    }
}
