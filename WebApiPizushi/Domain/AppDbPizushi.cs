﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Entities.Identity;
using System.Reflection.Emit;

namespace Domain;

public class AppDbPizushiContext : IdentityDbContext<UserEntity, RoleEntity, long>
{
    public AppDbPizushiContext(DbContextOptions<AppDbPizushiContext> opt) : base(opt) { }


    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<IngredientEntity> Ingredients { get; set; }
    public DbSet<ProductSizeEntity> ProductSizes { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductIngredientEntity> ProductIngredients { get; set; }
    public DbSet<ProductImageEntity> ProductImages { get; set; }
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<OrderStatusEntity> OrderStatuses { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserRoleEntity>(ur =>
        {
            ur.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(r => r.RoleId)
                .IsRequired();

            ur.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        });

        builder.Entity<ProductIngredientEntity>()
            .HasKey(pi => new { pi.ProductId, pi.IngredientId });
        
        builder.Entity<CartEntity>()
            .HasKey(pi => new { pi.ProductId, pi.UserId });
    }
}
