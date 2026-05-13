using CookBook.Models;
using CookBook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Services.Implementations;

public class IngredientService : IIngredientService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public IngredientService(IDbContextFactory<AppDbContext> factory)
        => _factory = factory;

    public async Task<List<Ingredient>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Ingredients
            .Include(i => i.Unit)
            .AsNoTracking()
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    public async Task<Ingredient?> GetByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Ingredients
            .Include(i => i.Unit)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Ingredient> AddAsync(Ingredient ingredient)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Ingredients.Add(ingredient);
        await db.SaveChangesAsync();
        return ingredient;
    }

    public async Task UpdateAsync(Ingredient ingredient)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Ingredients.Update(ingredient);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var ingredient = await db.Ingredients.FindAsync(id);
        if (ingredient is not null)
        {
            db.Ingredients.Remove(ingredient);
            await db.SaveChangesAsync();
        }
    }

    public async Task<bool> IsUsedInRecipeAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.RecipeIngredients.AnyAsync(ri => ri.IngredientId == id);
    }
}