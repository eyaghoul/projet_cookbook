using CookBook.Models;
 
namespace CookBook.Services.Interfaces;
public interface ITypeCuisineService
{
    Task<List<TypeCuisine>> GetAllAsync();
    Task<TypeCuisine?> GetByIdAsync(int id);
    Task<TypeCuisine> AddAsync(TypeCuisine typeCuisine);
    Task UpdateAsync(TypeCuisine typeCuisine);
    Task DeleteAsync(int id);
}