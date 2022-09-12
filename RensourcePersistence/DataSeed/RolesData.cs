using Microsoft.EntityFrameworkCore;
using RensourcePersistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourcePersistence.DataSeed
{
    public static class RolesData
    {
        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>().HasData(
                new Roles
                {
                    Id = 1,
                    RoleName = "SuperAdmin"
                },
                new Roles
                {
                    Id = 2,
                    RoleName = "Admin"
                }
            );
        }
    }
}
