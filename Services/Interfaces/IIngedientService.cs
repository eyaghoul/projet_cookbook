using CookBook.Models;

namespace CookBook.Services.Interfaces;

public interface IIngredientService
{
    Task<List<Ingredient>> GetAllAsync();
    Task<Ingredient?> GetByIdAsync(int id);
    Task<Ingredient> AddAsync(Ingredient ingredient);
    Task UpdateAsync(Ingredient ingredient);
    Task DeleteAsync(int id);
    Task<bool> IsUsedInRecipeAsync(int id);
}