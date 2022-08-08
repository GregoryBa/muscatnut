using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeService.Models;

[Table("Recipe")]
public class RecipeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required] 
    public string Title { get; set; }
    
    public string? Description { get; set; }

    public List<IngredientEntity> Ingredients { get; set; } = new List<IngredientEntity>();
}