using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookBook.Models;

public class RecipeIngredient
{
    [Required]

    public int RecipeId { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; } = null!;
    [Required]
    public int IngredientId { get; set; }
    [ForeignKey("IngredientId")]
    public Ingredient Ingredient { get; set; } = null!;

    /// <summary>Quantité utilisée dans la recette</summary>
    public double Quantity { get; set; }

    /// <summary>Unité utilisée dans la recette</summary>
    public string Unit { get; set; } = "grams"; // "grams" or "ml"

}