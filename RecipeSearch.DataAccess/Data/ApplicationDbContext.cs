using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeSearch.Models;

namespace RecipeSearch.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Unit> Units { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Recipe> Recipes{ get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<GroceryItem> GroceryItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductUnit>()
                .HasKey(pu => new { pu.ProductId, pu.UnitId });
            modelBuilder.Entity<ProductUnit>()
                .HasOne(pu => pu.Product)
                .WithMany(p => p.ProductUnit)
                .HasForeignKey(pu => pu.ProductId);
            modelBuilder.Entity<ProductUnit>()
                .HasOne(pu => pu.Unit)
                .WithMany(p => p.ProductUnits)
                .HasForeignKey(pu => pu.UnitId);

            modelBuilder.Entity<ListOfFavouritesRecipe>()
                .HasKey(l => new { l.ListOfFavouritesId, l.RecipeId });
            modelBuilder.Entity<ListOfFavouritesRecipe>()
                .HasOne(l => l.ListOfFavourites)
                .WithMany(p => p.FavouriteRecipes)
                .HasForeignKey(pu => pu.ListOfFavouritesId);
            modelBuilder.Entity<ListOfFavouritesRecipe>()
                .HasOne(pu => pu.Recipe)
                .WithMany(p => p.ListsOfFavourites)
                .HasForeignKey(pu => pu.RecipeId);

            modelBuilder.Entity<Recipe>().Ignore(t => t.Ingredients);
            modelBuilder.Entity<Recipe>().Ignore(t => t.Steps);

            modelBuilder.Entity<Ingredient>()
                .HasOne(c => c.Recipe).WithMany(e => e.Ingredients).HasForeignKey(c => c.RecipeId);
            modelBuilder.Entity<Step>()
                .HasOne(c => c.Recipe).WithMany(e => e.Steps).HasForeignKey(c => c.RecipeId);
            modelBuilder.Entity<ListOfFavourites>()
                .HasOne(c => c.ApplicationUser).WithMany(e => e.ListsOfFavourites).HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
