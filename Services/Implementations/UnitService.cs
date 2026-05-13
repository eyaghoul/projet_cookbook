using Microsoft.EntityFrameworkCore;
using CookBook.Models;
using CookBook.Services.Interfaces;

namespace CookBook.Services.Implementations;

public class UnitService : IUnitService
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    public UnitService(IDbContextFactory<AppDbContext> factory) => _factory = factory;
 
    public async Task<List<Unit>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Units.AsNoTracking().OrderBy(u => u.Name).ToListAsync();
    }
 
    public async Task<Unit?> GetByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Units.FindAsync(id);
    }
 
    public async Task<Unit> AddAsync(Unit unit)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Units.Add(unit);
        await db.SaveChangesAsync();
        return unit;
    }
 
    public async Task UpdateAsync(Unit unit)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Units.Update(unit);
        await db.SaveChangesAsync();
    }
 
    public async Task DeleteAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var u = await db.Units.FindAsync(id);
        if (u is not null) { db.Units.Remove(u); await db.SaveChangesAsync(); }
    }
}