using System.ComponentModel.DataAnnotations;

namespace RecipeService.Entities;

public class Ingredient
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required] 
    public string Name { get; set; }
}