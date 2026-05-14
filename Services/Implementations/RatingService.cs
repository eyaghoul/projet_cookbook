using CookBook.Models;
using CookBook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Services.Implementations;

public class RatingService : IRatingService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public RatingService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task AddRatingAsync(int recipeId, string userId, int stars)
    {
        await using var db = await _factory.CreateDbContextAsync();
        
        var existing = await db.Ratings
            .FirstOrDefaultAsync(r => r.RecipeId == recipeId && r.UserId == userId);

        if (existing != null)
        {
            existing.Stars = stars;
        }
        else
        {
            db.Ratings.Add(new Rating
            {
                RecipeId = recipeId,
                UserId = userId,
                Stars = stars
            });
        }

        await db.SaveChangesAsync();
    }

    public async Task<double> GetAverageRatingAsync(int recipeId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var ratings = await db.Ratings
            .Where(r => r.RecipeId == recipeId)
            .Select(r => r.Stars)
            .ToListAsync();

        if (!ratings.Any()) return 0;
        return Math.Round(ratings.Average(), 1);
    }

    public async Task<int> GetVoteCountAsync(int recipeId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Ratings.CountAsync(r => r.RecipeId == recipeId);
    }

    public async Task<int?> GetUserRatingAsync(int recipeId, string userId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var rating = await db.Ratings
            .FirstOrDefaultAsync(r => r.RecipeId == recipeId && r.UserId == userId);
        
        return rating?.Stars;
    }
}
