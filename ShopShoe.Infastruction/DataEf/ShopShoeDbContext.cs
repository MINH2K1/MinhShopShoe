﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using ShopShoe.Domain.Entities;
using ShopShoe.Domain.Interface;
using ShopShoe.Infastruction.Configuration;
using ShopShoe.Infastruction.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Infastruction.DataEf
{
    public class ShopShoeDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ShopShoeDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Function> Functions { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Announcement> Announcements { set; get; }
        public DbSet<AnnouncementUser> AnnouncementUsers { set; get; }

        public DbSet<Bill> Bills { set; get; }
        public DbSet<BillDetail> BillDetails { set; get; }
      
        public DbSet<Color> Colors { set; get; }
        public DbSet<Contact> Contacts { set; get; }
        public DbSet<Footer> Footers { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }
        public DbSet<ProductQuantity> ProductQuantities { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }

        public DbSet<Size> Sizes { set; get; }
        public DbSet<Slide> Slides { set; get; }

        public DbSet<Tag> Tags { set; get; }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<WholePrice> WholePrices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
                .HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
                .HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
               .HasKey(x => new { x.UserId });

            builder.AddConfiguration(new ProductTagConfiguration());
            builder.AddConfiguration(new TagConfiguration());
        }
        public override int SaveChanges()
        {
            System.Collections.Generic.IEnumerable<EntityEntry> modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (EntityEntry item in modified)
            {
                if (item.Entity is IDateTracking changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.DateCreated = DateTime.Now;
                    }
                    changedOrAddedItem.DateModified = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }


        //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ShopShoeDbContext>
        //{
        //    public ShopShoeDbContext CreateDbContext(string[] args)
        //    {
        //        IConfiguration configuration = new ConfigurationBuilder()
        //            .SetBasePath(Directory.GetCurrentDirectory())
        //            .AddJsonFile("appsettings.json").Build();
        //        DbContextOptionsBuilder<ShopShoeDbContext> builder = new DbContextOptionsBuilder<ShopShoeDbContext>();
        //        string connectionString = configuration.GetConnectionString("DefaultConnection");
        //        builder.UseSqlServer(connectionString);
        //        return new ShopShoeDbContext(builder.Options);
        //    }
        //}
    //}
    }
}
