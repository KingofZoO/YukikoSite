using System;
using Microsoft.EntityFrameworkCore;
using YukikoSite.Models;
using YukikoSite.Models.Account;

namespace YukikoSite.Models {
    public class AppDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<GlovesItem> Gloves { get; set; }
        public DbSet<SidingItem> Siding { get; set; }
        public DbSet<VentilationItem> Ventilation { get; set; }
        public DbSet<OthersItem> Others { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            Role adminRole = new Role() { Id = 1, Name = RoleTypes.AdminRole };
            User adminUser = new User() {
                Id = 1,
                Name = "admin",
                Password = "admin",
                RoleId = 1
            };
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
        }
    }
}
