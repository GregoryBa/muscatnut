using System.ComponentModel.DataAnnotations;

namespace RecipeService.Models;

public class IngredientEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required] 
    public string Name { get; set; }
}