using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipeService.Entities;

[Table("Recipe")]
public class Recipe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CookingTime { get; set; }
    public string RecipeSteps { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}