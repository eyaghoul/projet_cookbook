using CookBook.Models;

namespace CookBook.Services.Interfaces;

public interface IRatingService
{
    Task AddRatingAsync(int recipeId, string userId, int stars);
    Task<double> GetAverageRatingAsync(int recipeId);
    Task<int> GetVoteCountAsync(int recipeId);
    Task<int?> GetUserRatingAsync(int recipeId, string userId);
}
