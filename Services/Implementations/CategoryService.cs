using Microsoft.EntityFrameworkCore;
using CookBook.Models;
using CookBook.Services.Interfaces;

namespace CookBook.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    public CategoryService(IDbContextFactory<AppDbContext> factory) => _factory = factory;
 
    public async Task<List<Category>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
    }
 
    public async Task<Category?> GetByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Categories.FindAsync(id);
    }
 
    public async Task<Category> AddAsync(Category category)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Categories.Add(category);
        await db.SaveChangesAsync();
        return category;
    }
 
    public async Task UpdateAsync(Category category)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Categories.Update(category);
        await db.SaveChangesAsync();
    }
 
    public async Task DeleteAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var c = await db.Categories.FindAsync(id);
        if (c is not null) { db.Categories.Remove(c); await db.SaveChangesAsync(); }
    }

}