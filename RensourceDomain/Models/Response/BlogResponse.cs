using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Models.Response
{
    public class BlogResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public string? VideoLink { get; set; }
        public List<string>? Tags { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime DateDeleted { get; set; }

    }
}
