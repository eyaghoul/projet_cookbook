using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookBook.Models;

public class Ingredient
{
    [Key]
    public int Id { get; set; }

    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    public IdentityUser? User { get; set; }
    [Required(ErrorMessage = "Le nom de l'ingrédient est obligatoire")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [Range(0, 1000, ErrorMessage = "Calories invalides")]
    /// <summary>Calories par unité de mesure de référence (ex: kcal/100g)</summary>
    public double CaloriesPerUnit { get; set; }

    [Required(ErrorMessage = "L'unité est obligatoire")]
    public string Unit { get; set; } = "grams"; // "grams" or "ml"

    public string? Description { get; set; }

    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}