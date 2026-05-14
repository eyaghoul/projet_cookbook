using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CookBook.Models;

namespace CookBook.Models;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category>         Categories        => Set<Category>();
    public DbSet<TypeCuisine>      TypeCuisines      => Set<TypeCuisine>();
    public DbSet<Ingredient>       Ingredients       => Set<Ingredient>();
    public DbSet<Recipe>           Recipes           => Set<Recipe>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public DbSet<Rating>           Ratings           => Set<Rating>();
 
    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        // ── RecipeIngredient composite PK ────────────────────────────────────
        b.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });
 
        b.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.RecipeIngredients)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
 
        b.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);
 

 

 
        // ── Recipe → Category ─────────────────────────────────────────────────
        b.Entity<Recipe>()
            .HasOne(r => r.Category)
            .WithMany(c => c.Recipes)
            .HasForeignKey(r => r.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
 
        // ── Recipe → TypeCuisine ──────────────────────────────────────────────
        b.Entity<Recipe>()
            .HasOne(r => r.TypeCuisine)
            .WithMany(t => t.Recipes)
            .HasForeignKey(r => r.TypeCuisineId)
            .OnDelete(DeleteBehavior.Restrict);
 
        // ── Precision for doubles ─────────────────────────────────────────────
        b.Entity<Ingredient>()
            .Property(i => i.CaloriesPerUnit)
            .HasPrecision(10, 2);
 
        b.Entity<RecipeIngredient>()
            .Property(ri => ri.Quantity)
            .HasPrecision(10, 3);
       
    }
}