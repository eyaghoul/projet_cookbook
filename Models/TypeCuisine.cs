using System.ComponentModel.DataAnnotations;

namespace CookBook.Models;

public class TypeCuisine
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Le nom du type de cuisine est obligatoire")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty; // Tunisienne, Française, Italienne, etc.

    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}