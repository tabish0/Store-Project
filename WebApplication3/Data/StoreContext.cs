using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Item>().HasKey(I => I.ItemId);
            modelBuilder.Entity<Item>().HasOne(I => I.Category)
                                       .WithMany(I => I.Items)
                                       .HasForeignKey(I => I.CategoryId);

            modelBuilder.Entity<Category>().HasKey(C => C.CategoryId);
            modelBuilder.Entity<User>().HasKey(U => U.UserId);
            modelBuilder.Entity<RefreshToken>().HasKey(R => R.UserID);
            modelBuilder.Entity<RefreshToken>().HasOne(R => R.User)
                                               .WithOne(U => U.RefreshToken)
                                               .HasForeignKey<RefreshToken>(R => R.UserID);
            modelBuilder.Entity<RefreshToken>().Property(R => R.ExpiryDate)
                                               .HasColumnType("datetime2");
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
    }
}
