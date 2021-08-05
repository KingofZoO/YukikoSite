using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public DbSet<GalleryItem> GalleryItems { get; set; }

        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<NewsContentItem> NewsContentItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            //Database.EnsureCreated();
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
            modelBuilder.Entity<GlovesItem>().HasData(DataPreset(new GlovesItem(), "gloves"));
            modelBuilder.Entity<OthersItem>().HasData(DataPreset(new OthersItem(), "other"));
            modelBuilder.Entity<SidingItem>().HasData(DataPreset(new SidingItem(), "siding"));
            modelBuilder.Entity<VentilationItem>().HasData(DataPreset(new VentilationItem(), "ventil"));
        }

        private T[] DataPreset<T>(T item, string name) where T : IModelItem {
            T[] data = new T[3];
            for(int i = 1; i <= 3; i++) {
                var el = data[i - 1] = (T)Activator.CreateInstance(typeof(T));
                el.Id = i;
                el.ImagePath = $"{name}{i}.jpg";
                el.Title = $"{name}{i}";
                el.Description = $"This is {name}{i}";
            }
            return data;
        }

        public async Task<List<T>> GetPagedDataAsync<T>(int pageSize, int currentPage) where T : class {
            DbSet<T> dbSet = this.Set<T>();
            return await dbSet.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> GetCountAsync<T>() where T : class {
            DbSet<T> dbSet = this.Set<T>();
            return await dbSet.CountAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync<T>(int id) where T : class, IModelItem {
            if (id <= 0)
                return default;

            DbSet<T> dbSet = this.Set<T>();
            return await dbSet.FirstAsync(el => el.Id == id);
        }
    }
}
