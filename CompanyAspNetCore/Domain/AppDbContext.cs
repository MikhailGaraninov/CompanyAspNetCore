using CompanyAspNetCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CompanyAspNetCore.Domain
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // сделали связь классов через тип DbSet, с этими двумя таблицами и работаем
        public DbSet<TextField> TextFields { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "e77606c5-f39c-49cf-8e00-184c73dd2d77",
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "61e9c51e-9160-4b84-8eeb-c59e5f8a39f6",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "my@email.com",
                NormalizedEmail = "MY@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "superpassword"),
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "e77606c5-f39c-49cf-8e00-184c73dd2d77",
                UserId = "61e9c51e-9160-4b84-8eeb-c59e5f8a39f6",
            });

            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid ("200c7df5-c9c3-4372-b999-0f926d56166f"),
                CodeWord = "PageIndex",
                Title = "Главная"
            });

            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("fce8960e-3e9c-4e18-8a96-cc936f910def"),
                CodeWord = "PageServices",
                Title = "Наши услуги"
            });

            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("d2abacd4-c625-4cb8-9d3c-b3163e6630df"),
                CodeWord = "PageContacts",
                Title = "Контакты"
            });
        }
    }
}
