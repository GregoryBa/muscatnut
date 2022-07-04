using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeService.Entities;

public class Ingredient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required] 
    public string Name { get; set; }
    public int Calories { get; set; }
    public double Protein { get; set; }
    public double Carbohydrates { get; set; }
    public double Fats { get; set; }
}