using CookBook.Models;
 
namespace CookBook.Services.Interfaces;
public interface IUnitService
{
    Task<List<Unit>> GetAllAsync();
    Task<Unit?> GetByIdAsync(int id);
    Task<Unit> AddAsync(Unit unit);
    Task UpdateAsync(Unit unit);
    Task DeleteAsync(int id);
}