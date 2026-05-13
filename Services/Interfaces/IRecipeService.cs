using CookBook.Models;

namespace CookBook.Services.Interfaces;

public interface IRecipeService
{
    Task<List<Recipe>> GetAllAsync();
    Task<Recipe?> GetByIdAsync(int id);

    /// <summary>Filtered list — any null param means "no filter on that field"</summary>
    Task<List<Recipe>> FilterAsync(
        int?    categoryId,
        int?    typeCuisineId,
        int?    ingredientId,
        double? maxCaloriesTotal);

    Task<Recipe> AddAsync(Recipe recipe, IEnumerable<RecipeIngredient> ingredients);
    Task UpdateAsync(Recipe recipe, IEnumerable<RecipeIngredient> ingredients);
    Task DeleteAsync(int id);

    // ── Analytics ────────────────────────────────────────────────────────────
    Task<double> GetTotalCaloriesAsync(int recipeId);
    Task<double> GetCaloriesPerPersonAsync(int recipeId);

    /// <summary>Count of recipes grouped by category name</summary>
    Task<Dictionary<string, int>> GetCountByCategoryAsync();

    /// <summary>Count of recipes grouped by cuisine type name</summary>
    Task<Dictionary<string, int>> GetCountByTypeCuisineAsync();
}