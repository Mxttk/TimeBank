using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestMVC.Models;
using POE_PART_3_ST0084433.Models;

namespace POE_PART_3_ST0084433.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TestMVC.Models.UserData> UserData { get; set; }
        public DbSet<POE_PART_3_ST0084433.Models.SelfStudy> SelfStudy { get; set; }
    }
}
