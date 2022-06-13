using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public List<CookingStep> RecipeSteps { get; set; } = new List<CookingStep>();
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}

public class CookingStep
{
    public int StepNumber { get; set; }
    public string Description { get; set; }
    public DateTime Time { get; set; }
}