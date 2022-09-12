using Microsoft.EntityFrameworkCore;
using RensourcePersistence.DataSeed;
using RensourcePersistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourcePersistence.AppDBContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {
        }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CompanyInfo>? CompanyInfo { get; set; }
        public virtual DbSet<ContactUs>? ContactUs { get; set; }
        public virtual DbSet<ExecutiveRoles>? ExecutiveRoles { get; set; }
        public virtual DbSet<ExecutiveTeam>? ExecutiveTeam { get; set; }
        public virtual DbSet<ExecutiveTeamCategory>? ExecutiveTeamCategory { get; set; }
        public virtual DbSet<PressRelease>? PressRelease { get; set; }
        public virtual DbSet<Projects>? Projects { get; set; }
        public virtual DbSet<Roles>? Roles { get; set; }
        public virtual DbSet<UserRoles>? UserRoles { get; set; }
        public virtual DbSet<Users>? Users { get; set; }
        public virtual DbSet<Blog>? Blog { get; set; }
        public virtual DbSet<FAQs>? FAQs { get; set; }
        public virtual DbSet<Partners>? Partners { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.SeedRoles();
        }
    }
}
