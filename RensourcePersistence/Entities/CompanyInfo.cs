using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourcePersistence.Entities
{
    public class CompanyInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? OfficeAddress { get; set; }
        public string? HeadOfficeAddress { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber1 { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? LinkedIn { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime DateDeleted { get; set; }
    }
}
