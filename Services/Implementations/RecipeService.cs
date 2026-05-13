using CookBook.Models;
using CookBook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Services.Implementations;

public class RecipeService : IRecipeService
{
    private readonly IDbContextFactory<Models.AppDbContext> _factory;

    public RecipeService(IDbContextFactory<AppDbContext> factory)
        => _factory = factory;

    // ── Base query with all includes ─────────────────────────────────────────
    private static IQueryable<Recipe> FullQuery(AppDbContext db) =>
        db.Recipes
            .Include(r => r.Category)
            .Include(r => r.TypeCuisine)
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient);

    // ── CRUD ─────────────────────────────────────────────────────────────────
    public async Task<List<Recipe>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await FullQuery(db).AsNoTracking().ToListAsync();
    }

    public async Task<Recipe?> GetByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await FullQuery(db).AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Recipe>> FilterAsync(
        int?    categoryId,
        int?    typeCuisineId,
        int?    ingredientId,
        double? maxCaloriesTotal)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var query = FullQuery(db).AsNoTracking();

        if (categoryId.HasValue)
            query = query.Where(r => r.CategoryId == categoryId.Value);

        if (typeCuisineId.HasValue)
            query = query.Where(r => r.TypeCuisineId == typeCuisineId.Value);

        if (ingredientId.HasValue)
            query = query.Where(r =>
                r.RecipeIngredients.Any(ri => ri.IngredientId == ingredientId.Value));

        var results = await query.ToListAsync();

        // Calorie filter is computed in memory (aggregation over navigation)
        if (maxCaloriesTotal.HasValue)
            results = results
                .Where(r => r.TotalCalories <= maxCaloriesTotal.Value)
                .ToList();

        return results;
    }

    public async Task<Recipe> AddAsync(Recipe recipe, IEnumerable<RecipeIngredient> ingredients)
    {
        await using var db = await _factory.CreateDbContextAsync();

        recipe.RecipeIngredients = new List<RecipeIngredient>();
        
        // Null out navigation properties to avoid EF trying to insert existing entities
        var category = recipe.Category;
        var typeCuisine = recipe.TypeCuisine;
        recipe.Category = null!;
        recipe.TypeCuisine = null!;

        db.Recipes.Add(recipe);
        await db.SaveChangesAsync();

        foreach (var ri in ingredients)
        {
            ri.RecipeId = recipe.Id;
            ri.Ingredient = null!;
            db.RecipeIngredients.Add(ri);
        }
        await db.SaveChangesAsync();

        // Restore navigation properties for the caller if needed
        recipe.Category = category;
        recipe.TypeCuisine = typeCuisine;

        return recipe;
    }

    public async Task UpdateAsync(Recipe recipe, IEnumerable<RecipeIngredient> ingredients)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var existing = await db.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefaultAsync(r => r.Id == recipe.Id);

        if (existing == null) return;

        // Update scalar properties
        existing.Name = recipe.Name;
        existing.CategoryId = recipe.CategoryId;
        existing.TypeCuisineId = recipe.TypeCuisineId;
        existing.NumberOfPersons = recipe.NumberOfPersons;
        existing.CookingMethod = recipe.CookingMethod;
        existing.ImageUrl = recipe.ImageUrl;

        // Update ingredients: simpler to clear and re-add for this scale
        db.RecipeIngredients.RemoveRange(existing.RecipeIngredients);
        
        foreach (var ri in ingredients)
        {
            ri.RecipeId = recipe.Id;
            // Ensure we don't try to insert existing objects if they are attached to ri
            ri.Ingredient = null!; 

            db.RecipeIngredients.Add(ri);
        }

        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var recipe = await db.Recipes.FindAsync(id);
        if (recipe is not null)
        {
            db.Recipes.Remove(recipe);
            await db.SaveChangesAsync();
        }
    }

    // ── Analytics ─────────────────────────────────────────────────────────────
    public async Task<double> GetTotalCaloriesAsync(int recipeId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.RecipeIngredients
            .Where(ri => ri.RecipeId == recipeId)
            .Include(ri => ri.Ingredient)
            .SumAsync(ri => ri.Quantity * ri.Ingredient.CaloriesPerUnit);
    }

    public async Task<double> GetCaloriesPerPersonAsync(int recipeId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var recipe = await db.Recipes.FindAsync(recipeId);
        if (recipe is null || recipe.NumberOfPersons == 0) return 0;

        var total = await GetTotalCaloriesAsync(recipeId);
        return Math.Round(total / recipe.NumberOfPersons, 1);
    }

    public async Task<Dictionary<string, int>> GetCountByCategoryAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Recipes
            .Include(r => r.Category)
            .GroupBy(r => r.Category.Name)
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Name, x => x.Count);
    }

    public async Task<Dictionary<string, int>> GetCountByTypeCuisineAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Recipes
            .Include(r => r.TypeCuisine)
            .GroupBy(r => r.TypeCuisine.Name)
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Name, x => x.Count);
    }
}