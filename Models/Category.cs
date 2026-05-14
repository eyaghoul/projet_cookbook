using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookBook.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    public IdentityUser? User { get; set; }
    
    [Required(ErrorMessage = "Le nom de la catégorie est obligatoire")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty; // Petit-déjeuner, Déjeuner, Dîner, Dessert
   

    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}