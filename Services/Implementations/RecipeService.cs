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
                .ThenInclude(ri => ri.Ingredient)
                    .ThenInclude(i => i.Unit)
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Unit);

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
        db.Recipes.Add(recipe);
        await db.SaveChangesAsync();

        foreach (var ri in ingredients)
        {
            ri.RecipeId = recipe.Id;
            db.RecipeIngredients.Add(ri);
        }
        await db.SaveChangesAsync();

        return recipe;
    }

    public async Task UpdateAsync(Recipe recipe, IEnumerable<RecipeIngredient> ingredients)
    {
        await using var db = await _factory.CreateDbContextAsync();

        // Remove old ingredients
        var oldItems = db.RecipeIngredients.Where(ri => ri.RecipeId == recipe.Id);
        db.RecipeIngredients.RemoveRange(oldItems);

        recipe.RecipeIngredients = new List<RecipeIngredient>();
        db.Recipes.Update(recipe);

        foreach (var ri in ingredients)
        {
            ri.RecipeId = recipe.Id;
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