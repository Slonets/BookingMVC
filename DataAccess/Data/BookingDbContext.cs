using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class BookingDbContext : IdentityDbContext<UserEntity, RoleEntity, int,
         IdentityUserClaim<int>, UserRoleEntity, IdentityUserLogin<int>,
         IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options)
        : base(options) { }

        public DbSet<BuildingEntity> Buildings { get; set; }
        public DbSet<TypeOfSale> TypeOfSale { get; set; }
        public DbSet<ViewOfTheHouse> ViewOfTheHouse { get; set; }
        public DbSet<ImagesBulding> ImagesBuldings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserRoleEntity>(ur =>
            {
                ur.HasKey(ur => new { ur.UserId, ur.RoleId });
                ur.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(r => r.RoleId)
                    .IsRequired();
                ur.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(u => u.UserId)
                    .IsRequired();
            });
        }
    }
}
