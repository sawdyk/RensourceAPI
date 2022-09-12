using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourcePersistence.Entities
{
    public class UserRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public int? RoleId { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("UserId")]
        public virtual Users? Users { get; set; }

        [ForeignKey("RoleId")]
        public virtual Roles? Roles { get; set; }
    }
}
