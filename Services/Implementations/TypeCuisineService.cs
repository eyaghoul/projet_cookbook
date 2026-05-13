using Microsoft.EntityFrameworkCore;
using CookBook.Models;
using CookBook.Services.Interfaces;

namespace CookBook.Services.Implementations;

public class TypeCuisineService : ITypeCuisineService
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    public TypeCuisineService(IDbContextFactory<AppDbContext> factory) => _factory = factory;

    public async Task<List<TypeCuisine>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.TypeCuisines.AsNoTracking().OrderBy(t => t.Name).ToListAsync();
    }

    public async Task<TypeCuisine?> GetByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.TypeCuisines.FindAsync(id);
    }

    public async Task<TypeCuisine> AddAsync(TypeCuisine typeCuisine)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.TypeCuisines.Add(typeCuisine);
        await db.SaveChangesAsync();
        return typeCuisine;
    }

    public async Task UpdateAsync(TypeCuisine typeCuisine)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.TypeCuisines.Update(typeCuisine);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var typeCuisine = await db.TypeCuisines.FindAsync(id);
        if (typeCuisine is not null)
        {
            db.TypeCuisines.Remove(typeCuisine);
            await db.SaveChangesAsync();
        }
    }
}