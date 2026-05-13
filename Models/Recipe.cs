using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookBook.Models;

public class Recipe
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Le nom de la recette est obligatoire")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 20, ErrorMessage = "Nombre de personnes invalide")]
    public int NumberOfPersons { get; set; }

    [Required(ErrorMessage = "La méthode de préparation est obligatoire")]
    [StringLength(2000)]
    public string CookingMethod { get; set; }
    public int? PrepTimeMinutes { get; set; }
    public int? CookTimeMinutes { get; set; }
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "La catégorie de la recette est obligatoire")]
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    [Required(ErrorMessage = "Le type de cuisine est obligatoire")]
    public int TypeCuisineId { get; set; }

    [ForeignKey("TypeCuisineId")]
    public TypeCuisine TypeCuisine { get; set; }
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
 

   
   public double TotalCalories =>
        RecipeIngredients.Sum(ri => ri.Quantity * ri.Ingredient?.CaloriesPerUnit ?? 0);
 
    public double CaloriesPerPerson =>
        NumberOfPersons > 0 ? TotalCalories / NumberOfPersons : 0;
}