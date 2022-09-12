using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourcePersistence.Entities
{
    public class ContactUs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? SendersName { get; set; }
        public string? MessageSubject { get; set; }
        [Column(TypeName = "text")]
        public string? Message { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsRead { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime DateDeleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

