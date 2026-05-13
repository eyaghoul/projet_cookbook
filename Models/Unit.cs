using System.ComponentModel.DataAnnotations;

namespace CookBook.Models;

public class Unit
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Le nom de l'unité est obligatoire")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty; 
    [Required(ErrorMessage = "L'abréviation de l'unité est obligatoire")]
    [StringLength(10)]
    public string Abbreviation { get; set; } = string.Empty; // g, ml, cs, etc.

    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}