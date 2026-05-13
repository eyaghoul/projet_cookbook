using CookBook.Models;
 
namespace CookBook.Services.Interfaces;
 
public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
}