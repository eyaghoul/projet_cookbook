using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookBook.Models;

public class Rating
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RecipeId { get; set; }

    [ForeignKey("RecipeId")]
    public Recipe? Recipe { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public IdentityUser? User { get; set; }

    [Range(1, 5)]
    public int Stars { get; set; }
}
